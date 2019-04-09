using Jelly.Core.Linking;
using Jelly.Core.Parsing.AST;
using Jelly.Core.Parsing.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jelly.Core.Optimising
{
    /// <summary>
    /// Optimises an AST by performing common optimisations
    /// such as dead code elimination and constant folding.
    /// </summary>
    public class Optimiser
    {
        private Dictionary<string, OptimisedFunction> functions = new Dictionary<string, OptimisedFunction>();

        public List<IFunction> Optimise(List<IFunction> ast)
        {
            // Setup dictionary of available functions
            foreach (var func in ast)
            {
                functions.Add(func.Name, new OptimisedFunction(func));
            }

            // Optimise the Main function, this will also determine all of the functions
            // used throughout the program
            functions["Main"].IsReferenced = true;
            OptimiseFunction("Main");

            // Return the new ast
            // Strip unused functions
            var newAST = new List<IFunction>();

            foreach (var func in functions.Values)
            {
                if (func.IsReferenced)
                {
                    newAST.Add(func.Function);
                }
            }

            Engine.WriteDiagnostic("Optimisation succeeded");
            return newAST;
        }

        private void OptimiseFunction(string name)
        {
            var functionWrapper = functions[name];

            if (functionWrapper.HasStartedOptimisation || functionWrapper.IsInternal) return;

            functionWrapper.HasStartedOptimisation = true;
            var function = (FunctionNode)functionWrapper.Function;

            // Initialise parameter values to null as, without calling
            // context, the values are unknown.
            var variables = new Dictionary<string, double?>();

            foreach (var param in function.Parameters)
            {
                variables.Add(param.Identifier, null);
            }

            // Optimise the constructs within the function
            var constructs = FoldConstructs(function.Constructs, variables);

            // Remove unneeded assignments and mutations
            var referencedVariables = new List<string>();
            EliminateDeadCode(constructs, referencedVariables);

            // Put the function back together
            functions[function.Name].Function = new FunctionNode(function.Identifier,
                                                                 function.Parameters,
                                                                 constructs.ToArray(),
                                                                 function.Position);
        }

        #region First Pass - Constant Folding
        #region Constructs
        // Optimise a construct by removing dead code after a return statement
        private List<IConstructNode> FoldConstructs(IConstructNode[] constructs,
                                                    Dictionary<string, double?> variables)
        {
            var newConstructs = new List<IConstructNode>();

            foreach (var construct in constructs)
            {
                var newConstruct = FoldConstruct(construct, variables);

                if (!(newConstruct is null))
                {
                    newConstructs.Add(newConstruct);
                }

                if (construct is ReturnNode)
                {
                    break;
                }
            }

            return newConstructs;
        }

        // Call the relevant method for this construct
        private IConstructNode FoldConstruct(IConstructNode construct,
                                                 Dictionary<string, double?> variables)
        {
            switch (construct)
            {
                case IfBlockNode ifBlock:
                    return FoldIfBlock(ifBlock, variables);
                case LoopBlockNode loopBlock:
                    return FoldLoopBlock(loopBlock, variables);
                case IStatementNode statement:
                    return FoldStatement(statement, variables);
            }

            throw new Exception();
        }

        // Optimise all of the blocks in an if block
        private IfBlockNode FoldIfBlock(IfBlockNode ifBlock,
                                            Dictionary<string, double?> variables)
        {
            var blocks = new ConditionalBlockNode[ifBlock.Blocks.Length];

            for (int i = 0; i < ifBlock.Blocks.Length; i++)
            {
                ITermNode condition = null;

                if (!(ifBlock.Blocks[i].Condition is null))
                {
                    condition = FoldTerm(ifBlock.Blocks[i].Condition, variables);
                }

                // Enter a scope as the current variables are guaranteed to
                // persist into it.
                var scopedVariables = new Dictionary<string, double?>(variables);
                var constructs = FoldConstructs(ifBlock.Blocks[i].Constructs, scopedVariables);

                // Remove variables that have changed from this scope
                foreach (var variable in scopedVariables)
                {
                    if (variables.ContainsKey(variable.Key)
                        && variable.Value != variables[variable.Key])
                    {
                        variables[variable.Key] = null;
                    }
                }

                blocks[i] = new ConditionalBlockNode(condition, constructs.ToArray(), ifBlock.Position);
            }

            variables.Clear();

            return new IfBlockNode(blocks, ifBlock.Position);
        }

        // Optimise a loop block
        private LoopBlockNode FoldLoopBlock(LoopBlockNode loopBlock,
                                                Dictionary<string, double?> variables)
        {
            // The condition of a loop block cannot be optimised to be constant
            // as it could be modified within the loop body.
            // This is why this scope needs to be treated as completely separate
            // during optimisation (without further context).

            var constructs = FoldConstructs(loopBlock.Block.Constructs, new Dictionary<string, double?>());

            variables.Clear();

            return new LoopBlockNode(new ConditionalBlockNode(loopBlock.Block.Condition,
                                                              constructs.ToArray(),
                                                              loopBlock.Position),
                                                              loopBlock.Position);
        }
        #endregion

        #region Statements
        // Call the relevant method for this statement
        private IStatementNode FoldStatement(IStatementNode statement,
                                                 Dictionary<string, double?> variables)
        {
            switch (statement)
            {
                case AssignmentNode assignment:
                    return FoldAssignment(assignment, variables);
                case MutationNode mutation:
                    return FoldMutation(mutation, variables);
                case CallNode call:
                    return FoldStatementCall(call, variables);
                case ReturnNode @return:
                    return FoldReturn(@return, variables);
            }

            throw new Exception();
        }

        // Optimise an assignment by optimising the term
        private AssignmentNode FoldAssignment(AssignmentNode assignment,
                                                  Dictionary<string, double?> variables)
        {
            var value = FoldTerm(assignment.Value, variables);

            if (value is NumberNode num)
            {
                variables[assignment.Identifier.Identifier] = num.Number;
            }
            else
            {
                variables[assignment.Identifier.Identifier] = null;
            }

            return new AssignmentNode(assignment.Identifier, value, assignment.Position);
        }

        // Optimise a mutation by optimising the term, if it is a constant
        // then the mutation can be removed completely
        private MutationNode FoldMutation(MutationNode assignment,
                                              Dictionary<string, double?> variables)
        {
            var value = FoldTerm(assignment.Value, variables);

            if (value is NumberNode num)
            {
                variables[assignment.Identifier.Identifier] = num.Number;
            }
            else
            {
                variables[assignment.Identifier.Identifier] = null;
            }

            return new MutationNode(assignment.Identifier, value, assignment.Position);
        }

        // Optimise a call's arguments
        private CallNode FoldStatementCall(CallNode call,
                                      Dictionary<string, double?> variables)
        {
            var args = new ITermNode[call.Arguments.Length];

            for (int i = 0; i < call.Arguments.Length; i++)
            {
                args[i] = FoldTerm(call.Arguments[i], variables);
            }

            if (!functions[call.Identifier.Identifier].IsInternal)
            {
                OptimiseFunction(call.Identifier.Identifier);
            }

            functions[call.Identifier.Identifier].IsReferenced = true;

            return new CallNode(call.Identifier, args, call.Position);
        }

        // Optimise the value of a return statement
        private ReturnNode FoldReturn(ReturnNode @return,
                                          Dictionary<string, double?> variables)
        {
            if (@return.Value is null)
            {
                return @return;
            }

            return new ReturnNode(FoldTerm(@return.Value, variables), @return.Position);
        }
        #endregion

        #region Terms
        // Recursively use constant folding techniques to simplify a term
        private ITermNode FoldTerm(ITermNode term,
                                       Dictionary<string, double?> variables)
        {
            switch (term)
            {
                case ValueNode value:
                    return FoldValue(value, variables);
                case AbsoluteNode abs:
                    return FoldAbs(abs, variables);
                case NotNode not:
                    return FoldNot(not, variables);
                case NegativeNode negative:
                    return FoldNegative(negative, variables);
                case CallNode call:
                    return FoldTermCall(call, variables);
                case IdentifierNode identifier:
                    return FoldIdentifier(identifier, variables);
                case NumberNode number:
                    return number;
            }

            throw new Exception();
        }

        // Optimise the LHS and RHS, if they're both numbers then the
        // result of the operation can be determined
        private ITermNode FoldValue(ValueNode value,
                                        Dictionary<string, double?> variables)
        {
            var lhs = FoldTerm(value.LHS, variables);
            var rhs = FoldTerm(value.RHS, variables);

            if (lhs is NumberNode && rhs is NumberNode)
            {
                var lNum = ((NumberNode)lhs).Number;
                var rNum = ((NumberNode)rhs).Number;

                switch (value.Operator)
                {
                    case OperatorType.Add:
                        return new NumberNode(lNum + rNum, value.Position);
                    case OperatorType.Subtract:
                        return new NumberNode(lNum - rNum, value.Position);
                    case OperatorType.Multiply:
                        return new NumberNode(lNum * rNum, value.Position);
                    case OperatorType.Divide:
                        return new NumberNode(lNum / rNum, value.Position);
                    case OperatorType.Modulo:
                        return new NumberNode(lNum % rNum, value.Position);
                    case OperatorType.EqualTo:
                        return new NumberNode(lNum == rNum ? 1 : 0, value.Position);
                    case OperatorType.UnequalTo:
                        return new NumberNode(lNum != rNum ? 1 : 0, value.Position);
                    case OperatorType.LessThan:
                        return new NumberNode(lNum < rNum ? 1 : 0, value.Position);
                    case OperatorType.GreaterThan:
                        return new NumberNode(lNum > rNum ? 1 : 0, value.Position);
                    case OperatorType.LessThanOrEqualTo:
                        return new NumberNode(lNum <= rNum ? 1 : 0, value.Position);
                    case OperatorType.GreaterThanOrEqualTo:
                        return new NumberNode(lNum >= rNum ? 1 : 0, value.Position);
                }
            }

            return new ValueNode(lhs, value.Operator, rhs, value.Position);
        }

        // Apply the abs operation if the value is known
        private ITermNode FoldAbs(AbsoluteNode abs,
                                      Dictionary<string, double?> variables)
        {
            var value = FoldTerm(abs.Value, variables);

            if (value is NumberNode num)
            {
                return new NumberNode(Math.Abs(num.Number), num.Position);
            }

            return new AbsoluteNode(value, abs.Position);
        }

        // Apply the not operation if the value is known
        private ITermNode FoldNot(NotNode not,
                                      Dictionary<string, double?> variables)
        {
            var value = FoldTerm(not.Term, variables);

            if (value is NumberNode num)
            {
                return new NumberNode(num.Number == 0 ? 1 : 0, num.Position);
            }

            return new NotNode(value, not.Position);
        }

        // Apply the negative operation if the value is known
        private ITermNode FoldNegative(NegativeNode negative,
                                           Dictionary<string, double?> variables)
        {
            var value = FoldTerm(negative.Term, variables);

            if (value is NumberNode num)
            {
                return new NumberNode(-num.Number, num.Position);
            }

            return new NegativeNode(value, negative.Position);
        }

        // Optimise a call's arguments and potentially execute it
        // if it is deterministic
        private ITermNode FoldTermCall(CallNode call,
                                          Dictionary<string, double?> variables)
        {
            var args = new ITermNode[call.Arguments.Length];
            bool areAllArgumentsNumbers = true;

            for (int i = 0; i < call.Arguments.Length; i++)
            {
                args[i] = FoldTerm(call.Arguments[i], variables);

                if (!(args[i] is NumberNode))
                {
                    areAllArgumentsNumbers = false;
                }
            }

            if (functions[call.Identifier.Identifier].IsInternal)
            {
                var func = (InternalFunction)functions[call.Identifier.Identifier].Function;

                if (func.Deterministic)
                {
                    if (areAllArgumentsNumbers)
                    {
                        var result = func.Execute(args.Select(x => ((NumberNode)x).Number).ToArray());

                        return new NumberNode(result, call.Position);
                    }
                }
            }
            else
            {
                OptimiseFunction(call.Identifier.Identifier);
            }

            functions[call.Identifier.Identifier].IsReferenced = true;

            return new CallNode(call.Identifier, args, call.Position);
        }

        // If the value of the identifier is constant, switch it out
        // for the constant number
        private ITermNode FoldIdentifier(IdentifierNode identifier,
                                             Dictionary<string, double?> variables)
        {
            if (variables.ContainsKey(identifier.Identifier))
            {
                var constant = variables[identifier.Identifier];

                if (constant is null)
                {
                    return identifier;
                }

                return new NumberNode(constant.Value, identifier.Position);
            }

            return identifier;
        }
        #endregion
        #endregion

        #region Second Pass - Dead Code Elimination
        // Eliminate dead code from this construct.
        private void EliminateDeadCode(List<IConstructNode> constructs, 
                                       List<string> referencedVariables)
        {
            // Loop backwards through the constructs.
            for (int i = constructs.Count - 1; i >= 0; i--)
            {
                bool removed = false;

                // If the variable being assigned/mutated is not
                // being used after this construct, remove it.
                if (constructs[i] is AssignmentNode assignment)
                {
                    if (!referencedVariables.Contains(assignment.Identifier.Identifier))
                    {
                        constructs.RemoveAt(i);
                        removed = true;
                    }
                }
                else if (constructs[i] is MutationNode mutation)
                {
                    if (!referencedVariables.Contains(mutation.Identifier.Identifier))
                    {
                        constructs.RemoveAt(i);
                        removed = true;
                    }
                }

                // Get the variables used in this construct
                // to determine if an assignment or mutation
                // is unnecessary in the constructs before it.
                if (!removed)
                {
                    GetReferencedVariables((Node)constructs[i],
                                            referencedVariables);
                }
            }
        }

        // Get the variables referenced in this node recursively.
        private void GetReferencedVariables(Node node, List<string> variables)
        {
            switch (node)
            {
                case AbsoluteNode absolute:
                    GetReferencedVariables((Node)absolute.Value, variables);
                    break;
                case AssignmentNode assignment:
                    GetReferencedVariables((Node)assignment.Value, variables);
                    break;
                case CallNode call:
                    foreach (var arg in call.Arguments)
                    {
                        GetReferencedVariables((Node)arg, variables);
                    }
                    break;
                case ConditionalBlockNode conditionalBlock:
                    GetReferencedVariables((Node)conditionalBlock.Condition, variables);

                    foreach (var construct in conditionalBlock.Constructs)
                    {
                        GetReferencedVariables((Node)construct, variables);
                    }
                    break;
                case IdentifierNode identifier:
                    variables.Add(identifier.Identifier);
                    break;
                case IfBlockNode ifBlock:
                    foreach (var block in ifBlock.Blocks)
                    {
                        GetReferencedVariables(block, variables);
                    }
                    break;
                case LoopBlockNode loopBlock:
                    GetReferencedVariables(loopBlock.Block, variables);
                    break;
                case MutationNode mutation:
                    GetReferencedVariables((Node)mutation.Value, variables);
                    break;
                case NegativeNode negative:
                    GetReferencedVariables((Node)negative.Term, variables);
                    break;
                case NotNode not:
                    GetReferencedVariables((Node)not.Term, variables);
                    break;
                case ReturnNode @return:
                    GetReferencedVariables((Node)@return.Value, variables);
                    break;
                case ValueNode value:
                    GetReferencedVariables((Node)value.LHS, variables);
                    GetReferencedVariables((Node)value.RHS, variables);
                    break;
            }
        }
        #endregion
    }
}
