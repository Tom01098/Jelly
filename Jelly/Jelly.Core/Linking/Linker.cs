using Jelly.Core.Interpreting;
using Jelly.Core.Parsing;
using Jelly.Core.Parsing.AST;
using Jelly.Core.Utility;
using System;
using System.Collections.Generic;
using System.IO;
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
            var libraryFunctions = GetInternalFunctions();

            libraryFunctions.AddRange(GetPureFunctions());
            libraryFunctions.AddRange(userAST.Cast<IFunction>().ToList());

            return libraryFunctions;
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

            Engine.WriteDiagnostic($"Found {functions.Count} internal standard functions.", false);

            return functions;
        }

        private List<IFunction> GetPureFunctions()
        {
            var directory = Path.Combine(Environment.CurrentDirectory, "StandardLibrary/Pure");
            var files = FileUtility.FindJellyFiles(directory);

            var functions = new List<IFunction>();

            foreach (var file in files)
            {
                var text = File.ReadAllText(file);

                functions.AddRange(new Parser().Parse(new Lexer().Lex(text, "Standard Library")));
            }

            Engine.WriteDiagnostic($"Found {functions.Count} pure standard functions.", false);

            return functions;
        }
    }
}
