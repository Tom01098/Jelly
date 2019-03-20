using Jelly.Core.Parsing.AST;
using Jelly.Core.StandardLibrary.Internal;
using System;
using System.Collections.Generic;

namespace Jelly.Core.Interpreting
{
    /// <summary>
    /// Interpret a given AST at run time
    /// </summary>
    public class Interpreter
    {
        private Dictionary<string, IFunction> functions;
        private ValueStack values;

        public void Interpret(List<FunctionNode> ast)
        {
            values = new ValueStack();
            CreateFunctionDictionary(ast);

            ExecuteFunction(functions["Main"], new List<double> { });
        }

        // Create a dictionary of functions from the AST and the internal functions
        private void CreateFunctionDictionary(List<FunctionNode> ast)
        {
            functions = new Dictionary<string, IFunction>();

            foreach (var node in ast)
            {
                functions.Add(node.Identifier.Identifier, node);
            }

            // TODO This needs to be made general rather than a special case
            // through reflection in the InternalFunction class
            functions.Add("Write", new InternalFunction(new Write()));
        }

        // Execute a function with the given arguments
        private double ExecuteFunction(IFunction function, List<double> arguments)
        {
            // Execute the internal function if it is one
            if (function is InternalFunction internalFunction)
            {
                return internalFunction.Execute(arguments);
            }

            var node = (FunctionNode)function;

            // Push the arguments to the call stack
            values.New();

            for (int i = 0; i < arguments.Count; i++)
            {
                values.Add(node.Parameters[i].Identifier, arguments[i]);
            }

            // Execute each construct
            foreach (var construct in node.Constructs)
            {
                if (construct is IfBlockNode ifBlock)
                {
                    foreach (var block in ifBlock.Blocks)
                    {
                        if (Evaluate(block.Condition) != 0)
                        {
                            values.NewInScope();



                            values.Pop();
                            break;
                        }
                    }
                }
                else if (construct is IStatementNode statement)
                {
                    if (statement is ReturnNode returnNode)
                    {
                        var result = Evaluate(returnNode.Value);
                        values.Pop();
                        return result;
                    }
                    else if (statement is AssignmentNode assignmentNode)
                    {
                        values.Add(assignmentNode.Identifier.Identifier, Evaluate(assignmentNode.Value));
                    }
                    else if (statement is MutationNode mutationNode)
                    {
                        values.Mutate(mutationNode.Identifier.Identifier, Evaluate(mutationNode.Value));
                    }
                    else if (statement is CallNode callNode)
                    {
                        var args = new List<double>();

                        foreach (var arg in callNode.Arguments)
                        {
                            args.Add(Evaluate(arg));
                        }

                        ExecuteFunction(functions[callNode.Identifier.Identifier], args);
                    }
                    else
                    {
                        throw new Exception($"Cannot execute {statement}");
                    }
                }
                else
                {
                    throw new Exception($"Cannot execute {construct}");
                }
            }

            // No return, so return NaN
            values.Pop();
            return double.NaN;
        }

        // Evaluate a value
        private double Evaluate(ITermNode term)
        {
            if (term is ValueNode valueNode)
            {
                var lhs = Evaluate(valueNode.LHS);
                var rhs = Evaluate(valueNode.RHS);

                switch (valueNode.Operator)
                {
                    case OperatorType.Add:
                        return lhs + rhs;
                    case OperatorType.Subtract:
                        return lhs - rhs;
                    case OperatorType.Multiply:
                        return lhs * rhs;
                    case OperatorType.Divide:
                        return lhs / rhs;
                    case OperatorType.Modulo:
                        return lhs % rhs;
                    case OperatorType.EqualTo:
                        return lhs == rhs ? 1 : 0;
                    case OperatorType.UnequalTo:
                        return lhs != rhs ? 1 : 0;
                    case OperatorType.LessThan:
                        return lhs < rhs ? 1 : 0;
                    case OperatorType.GreaterThan:
                        return lhs > rhs ? 1 : 0;
                    case OperatorType.LessThanOrEqualTo:
                        return lhs <= rhs ? 1 : 0;
                    case OperatorType.GreaterThanOrEqualTo:
                        return lhs >= rhs ? 1 : 0;
                    default:
                        throw new Exception($"Invalid operator {valueNode.Operator}");
                }
            }
            else if (term is NotNode notNode)
            {
                return Evaluate(notNode.Term) == 0 ? 1 : 0;
            }
            else if (term is NegativeNode negativeNode)
            {
                return -Evaluate(negativeNode.Term);
            }
            else if (term is CallNode callNode)
            {
                var args = new List<double>();

                foreach (var arg in callNode.Arguments)
                {
                    args.Add(Evaluate(arg));
                }

                return ExecuteFunction(functions[callNode.Identifier.Identifier], args);
            }
            else if (term is NumberNode numberNode)
            {
                return numberNode.Number;
            }
            else if (term is IdentifierNode identifierNode)
            {
                return values[identifierNode.Identifier];
            }
            else
            {
                throw new Exception($"Cannot execute {term}");
            }
        }
    }
}
