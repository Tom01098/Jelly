﻿using Jelly.Core.Parsing.AST;
using Jelly.Core.Utility;
using System.Collections.Generic;

namespace Jelly.Core.Verifying
{
    /// <summary>
    /// Verifies an AST.
    /// </summary>
    public static class Verifier
    {
        private static Dictionary<string, FunctionInfo> functionInfos;
        private static List<Diagnostic> diagnostics;

        /// <summary>
        /// Verifies an AST to ensure that errors which can be caught at
        /// compile time are.
        /// </summary>
        public static void Verify(List<IFunction> ast)
        {
            diagnostics = new List<Diagnostic>();

            GetFunctionInformation(ast);

            foreach (var function in ast)
            {
                if (function is FunctionNode node)
                {
                    VerifyFunction(node);
                }
            }

            if (diagnostics.Count != 0)
            {
                foreach (var diag in diagnostics)
                {
                    Engine.WriteError(diag.ToString());
                }

                throw new JellyException();
            }

            Engine.WriteDiagnostic("Verifying succeeded");
        }

        // Retrieve function information from all functions and check for
        // multiple functions that are named the same.
        private static void GetFunctionInformation(List<IFunction> ast)
        {
            functionInfos = new Dictionary<string, FunctionInfo>();

            foreach (var function in ast)
            {
                var info = new FunctionInfo(function.Name, function.ParameterCount, function.Position);

                if (functionInfos.ContainsKey(info.Name))
                {
                    AddDiagnostic($"Duplicate function called '{info.Name}'",
                                  info.Position);
                }
                else
                {
                    functionInfos.Add(info.Name, info);
                }
            }

            if (!functionInfos.ContainsKey("Main"))
            {
                AddDiagnostic("No Main function", null);
            }
            else if (functionInfos["Main"].ParameterCount != 0)
            {
                AddDiagnostic("Main cannot have any parameters",
                              functionInfos["Main"].Position);
            }
        }

        // Verify the constructs in a function
        private static void VerifyFunction(FunctionNode node)
        {
            var parameters = new List<string>();

            foreach (var parameter in node.Parameters)
            {
                if (parameters.Contains(parameter.Identifier))
                {
                    AddDiagnostic($"Multiple parameters called '{parameter.Identifier}'",
                                  parameter.Position);
                }
                else
                {
                    parameters.Add(parameter.Identifier);
                }
            }

            VerifyConstructs(node.Constructs, parameters);
        }

        // Verify that constructs are correctly used
        private static void VerifyConstructs(IConstructNode[] constructs, 
                                             List<string> definedVariables)
        {
            foreach (var construct in constructs)
            {
                if (construct is IfBlockNode ifBlockNode)
                {
                    foreach (var block in ifBlockNode.Blocks)
                    {
                        VerifyTerm(block.Condition, definedVariables);
                        VerifyConstructs(block.Constructs, new List<string>(definedVariables));
                    }
                }
                else if (construct is LoopBlockNode loopBlockNode)
                {
                    VerifyTerm(loopBlockNode.Block.Condition, definedVariables);
                    VerifyConstructs(loopBlockNode.Block.Constructs, new List<string>(definedVariables));
                }
                else
                {
                    VerifyStatement((IStatementNode)construct, definedVariables);
                }
            }
        }

        // Verify a statement
        private static void VerifyStatement(IStatementNode statement,
                                            List<string> definedVariables)
        {
            if (statement is AssignmentNode assignment)
            {
                if (definedVariables.Contains(assignment.Identifier.Identifier))
                {
                    AddDiagnostic($"'{assignment.Identifier.Identifier}' has already been defined, you can mutate it with '=>'", 
                                  assignment.Position);
                }

                VerifyTerm(assignment.Value, definedVariables);

                definedVariables.Add(assignment.Identifier.Identifier);
            }
            else if (statement is MutationNode mutation)
            {
                if (!definedVariables.Contains(mutation.Identifier.Identifier))
                {
                    AddDiagnostic($"Mutating '{mutation.Identifier.Identifier}' before it has been defined, you need to assign it first with '='",
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

        // Verify a term
        private static void VerifyTerm(ITermNode term,
                                       List<string> definedVariables)
        {
            if (term is IdentifierNode identifier)
            {
                if (!definedVariables.Contains(identifier.Identifier))
                {
                    AddDiagnostic($"'{identifier.Identifier}' must be defined before it is used",
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

        // Verify a call
        // This has been refactored because it would be duplicated in
        // statement and term
        private static void VerifyCall(CallNode call,
                                       List<string> definedVariables)
        {
            if (!functionInfos.ContainsKey(call.Identifier.Identifier))
            {
                AddDiagnostic($"The function '{call.Identifier.Identifier}' is undefined",
                              call.Position);
            }
            else if (call.Arguments.Length != functionInfos[call.Identifier.Identifier].ParameterCount)
            {
                AddDiagnostic($"Expected {functionInfos[call.Identifier.Identifier].ParameterCount} arguments but got {call.Arguments.Length}",
                              call.Position);
            }

            foreach (var arg in call.Arguments)
            {
                VerifyTerm(arg, definedVariables);
            }
        }

        private static void AddDiagnostic(string message, Position? position)
            => diagnostics.Add(new Diagnostic(message, position));

        /// <summary>
        /// An error diagnostic from the <see cref="Verifier"/>.
        /// </summary>
        private class Diagnostic
        {
            public string Message { get; }
            public Position? Position { get; }

            public Diagnostic(string message, Position? position)
                => (Message, Position) = (message, position);

            public override string ToString()
            {
                if (Position is null)
                {
                    return Message;
                }
                else
                {
                    return $"{Message} at {Position}";
                }
            }
        }
    }
}
