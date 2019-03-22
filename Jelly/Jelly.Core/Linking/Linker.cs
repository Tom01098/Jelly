using Jelly.Core.Interpreting;
using Jelly.Core.Parsing.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Jelly.Core.Linking
{
    /// <summary>
    /// Links the standard library with the program
    /// </summary>
    public class Linker
    {
        public List<IFunction> LinkAST(List<FunctionNode> userAST)
        {
            var internals = GetInternalFunctions();

            internals.AddRange(userAST.Cast<IFunction>().ToList());

            return internals;
        }

        private List<IFunction> GetInternalFunctions()
        {
            var functions = new List<IFunction>();

            // Get Internal library functions
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var method in type.GetRuntimeMethods())
                    {
                        if (!method.IsStatic)
                        {
                            continue;
                        }

                        var attribute = method.GetCustomAttribute<InternalFunctionAttribute>();

                        if (!(attribute is null))
                        {
                            functions.Add(new InternalFunction(method));
                        }
                    }
                }
            }

            return functions;
        }

        private List<IFunction> GetPureFunctions()
        {
            throw new NotImplementedException();
        }
    }
}
