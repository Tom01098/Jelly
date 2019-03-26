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

        }
    }
}
