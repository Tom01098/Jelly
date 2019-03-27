using Jelly.Core.Linking;
using Jelly.Core.Parsing.AST;
using Jelly.Core.Utility;
using System;
using System.Collections.Generic;

namespace Jelly.Core.Verifying
{
    /// <summary>
    /// Verifies an AST.
    /// </summary>
    public static class Verifier
    {
        private static Dictionary<string, FunctionInfo> functionInfos;

        /// <summary>
        /// Verifies an AST to ensure that errors which can be caught at
        /// compile time are.
        /// </summary>
        public static void Verify(List<IFunction> ast)
        {
            GetFunctionInformation(ast);

            foreach (var function in ast)
            {
                if (function is FunctionNode node)
                {
                    VerifyFunction(node);
                }
            }
        }

        // Retrieve function information from all functions and check for
        // multiple functions that are named the same.
        private static void GetFunctionInformation(List<IFunction> ast)
        {
            functionInfos = new Dictionary<string, FunctionInfo>();

            foreach (var function in ast)
            {
                FunctionInfo info;

                if (function is InternalFunction func)
                {
                    info = new FunctionInfo(func.Name, 
                                            func.ParameterCount,
                                            new Position("Internal", 0, 0));
                }
                else if (function is FunctionNode node)
                {
                    info = new FunctionInfo(node.Identifier.Identifier,
                                            node.Parameters.Count,
                                            node.Position);
                }
                else
                {
                    throw new Exception($"Unrecognised function type {function}");
                }

                if (functionInfos.ContainsKey(info.Name))
                {
                    throw new JellyException($"Multiple functions called {info.Name}",
                                             info.Position);
                }

                functionInfos.Add(info.Name, info);
            }
        }

        // Verify the constructs in a function
        private static void VerifyFunction(FunctionNode node)
        {
            var parameters = node.Parameters.ConvertAll(x => x.Identifier);
            VerifyConstructs(node.Constructs, parameters);
        }

        // Verify that constructs are correctly used
        private static void VerifyConstructs(List<IConstructNode> constructs, 
                                             List<string> definedVariables)
        {
            foreach (var construct in constructs)
            {
                // TODO Construct blocks
                if (construct is IfBlockNode ifBlockNode)
                {

                }
                else if (construct is LoopBlockNode loopBlockNode)
                {

                }
                else
                {
                    VerifyStatement((IStatementNode)construct, definedVariables);
                }
            }
        }

        private static void VerifyStatement(IStatementNode statement,
                                            List<string> definedVariables)
        {
            if (statement is AssignmentNode assignment)
            {
                if (definedVariables.Contains(assignment.Identifier.Identifier))
                {
                    throw new JellyException($"Assigning to an already assigned variable", 
                                             assignment.Position);
                }

                VerifyTerm(assignment.Value, definedVariables);

                definedVariables.Add(assignment.Identifier.Identifier);
            }
            else if (statement is MutationNode mutation)
            {
                if (!definedVariables.Contains(mutation.Identifier.Identifier))
                {
                    throw new JellyException($"Mutating an undefined variable",
                                             mutation.Position);
                }

                VerifyTerm(mutation.Value, definedVariables);
            }
            else if (statement is CallNode call)
            {
                VerifyCall(call, definedVariables);
            }
            else if (statement is ReturnNode @return)
            {
                if (!(@return.Value is null))
                {
                    VerifyTerm(@return.Value, definedVariables);
                }
            }
        }

        private static void VerifyTerm(ITermNode term,
                                       List<string> definedVariables)
        {
            if (term is IdentifierNode identifier)
            {
                if (!definedVariables.Contains(identifier.Identifier))
                {
                    throw new JellyException($"{identifier.Identifier} is undefined",
                                             identifier.Position);
                }
            }
            else if (term is CallNode call)
            {
                VerifyCall(call, definedVariables);
            }
            else if (term is AbsoluteNode abs)
            {
                VerifyTerm(abs.Value, definedVariables);
            }
            else if (term is NegativeNode neg)
            {
                VerifyTerm(neg.Term, definedVariables);
            }
            else if (term is ValueNode value)
            {
                VerifyTerm(value.LHS, definedVariables);
                VerifyTerm(value.RHS, definedVariables);
            }
        }

        private static void VerifyCall(CallNode call,
                                       List<string> definedVariables)
        {
            if (!functionInfos.ContainsKey(call.Identifier.Identifier))
            {
                throw new JellyException($"{call.Identifier.Identifier} is not defined",
                                         call.Position);
            }

            if (call.Arguments.Count != functionInfos[call.Identifier.Identifier].ParameterCount)
            {
                throw new JellyException($"Expected {functionInfos[call.Identifier.Identifier].ParameterCount} arguments",
                                         call.Position);
            }

            foreach (var arg in call.Arguments)
            {
                VerifyTerm(arg, definedVariables);
            }
        }
    }
}
