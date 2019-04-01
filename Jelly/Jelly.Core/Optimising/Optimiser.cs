using Jelly.Core.Parsing.AST;
using System;
using System.Collections.Generic;

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
            functions["Main"].TimesUsed++;
            functions["Main"].HasStartedOptimisation = true;
            OptimiseFunction("Main");

            // Return the new ast
            // Strip unused functions
            var newAST = new List<IFunction>();

            foreach (var func in functions.Values)
            {
                // TODO Change this to 0 when function stripping is implemented
                if (func.TimesUsed > -1)
                {
                    newAST.Add(func.Function);
                }
            }

            return newAST;
        }

        private void OptimiseFunction(string name)
        {
            var functionWrapper = functions[name];

            if (!functionWrapper.HasStartedOptimisation || functionWrapper.IsInternal) return;

            var function = (FunctionNode)functionWrapper.Function;

            // Initialise parameter values to null as, without calling
            // context, the values are unknown.
            var variables = new Dictionary<string, double?>();

            foreach (var param in function.Parameters)
            {
                variables.Add(param.Identifier, null);
            }

            // Optimise the constructs within the function
            var constructs = OptimiseConstructs(function.Constructs, variables);

            // Put the function back together
            functions[function.Name].Function = new FunctionNode(function.Identifier,
                                                                 function.Parameters,
                                                                 constructs,
                                                                 function.Position);
        }

        // Optimise a construct by removing dead code after a return statement
        private IConstructNode[] OptimiseConstructs(IConstructNode[] constructs,
                                                    Dictionary<string, double?> variables)
        {
            var newConstructs = new List<IConstructNode>();

            foreach (var construct in constructs)
            {
                var newConstruct = OptimiseConstruct(construct, variables);

                if (!(newConstruct is null))
                {
                    newConstructs.Add(newConstruct);
                }

                if (construct is ReturnNode)
                {
                    break;
                }
            }

            return newConstructs.ToArray();
        }

        private IConstructNode OptimiseConstruct(IConstructNode construct,
                                                 Dictionary<string, double?> variables)
        {
            switch (construct)
            {
                case IfBlockNode ifBlock:
                    return ifBlock;
                case LoopBlockNode loopBlock:
                    return loopBlock;
                case IStatementNode statement:
                    return OptimiseStatement(statement, variables);
            }

            throw new Exception();
        }

        private IStatementNode OptimiseStatement(IStatementNode statement,
                                                 Dictionary<string, double?> variables)
        {
            switch (statement)
            {
                case AssignmentNode assignment:
                    return OptimiseAssignment(assignment, variables);
                case MutationNode mutation:
                    return OptimiseMutation(mutation, variables);
                case CallNode call:
                    return call;
                case ReturnNode @return:
                    return @return;
            }

            throw new Exception();
        }

        private AssignmentNode OptimiseAssignment(AssignmentNode assignment,
                                                  Dictionary<string, double?> variables)
        {
            var value = OptimiseTerm(assignment.Value, variables);

            if (value is NumberNode num)
            {
                variables[assignment.Identifier.Identifier] = num.Number;
                return null;
            }

            variables[assignment.Identifier.Identifier] = null;
            return new AssignmentNode(assignment.Identifier, value, assignment.Position);
        }

        private MutationNode OptimiseMutation(MutationNode assignment,
                                              Dictionary<string, double?> variables)
        {
            var value = OptimiseTerm(assignment.Value, variables);

            if (value is NumberNode num)
            {
                variables[assignment.Identifier.Identifier] = num.Number;
                return null;
            }

            variables[assignment.Identifier.Identifier] = null;
            return new MutationNode(assignment.Identifier, value, assignment.Position);
        }

        private ITermNode OptimiseTerm(ITermNode term,
                                       Dictionary<string, double?> variables)
        {
            return term;
        }
    }
}
