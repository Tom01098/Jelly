using Jelly.Core.Interpreting;
using Jelly.Core.Linking;
using Jelly.Core.Parsing;
using Jelly.Core.Parsing.AST;
using Jelly.Core.Utility;
using System;
using System.Collections.Generic;
using System.IO;

namespace Jelly.Core
{
    /// <summary>
    /// Controls both the parsing and interpretation stages of Jelly
    /// </summary>
    public static class Engine
    {
        private static Action<string> diagnostic;
        private static bool importantOnly;

        public static List<IFunction> GetAST(string folder)
        {
            var files = FileUtility.FindJellyFiles(folder);
            WriteDiagnostic($"Found {files.Length} files.", false);

            var ast = new List<FunctionNode>();

            foreach (var file in files)
            {
                var relativePath = file.Replace(folder, "");
                var text = File.ReadAllText(file);

                var tokens = new Lexer().Lex(text, relativePath);
                var functions = new Parser().Parse(tokens);

                ast.AddRange(functions);
            }

            WriteDiagnostic($"Found {ast.Count} user functions.", false);

            return new Linker().LinkAST(ast);
        }

        public static void Execute(List<IFunction> ast)
        {
            WriteDiagnostic("START PROGRAM\n");
            new Interpreter().Interpret(ast);
            WriteDiagnostic("\nEND PROGRAM");
        }

        public static void SetDiagnosticOut(Action<string> diagnostic, bool importantOnly = false)
        {
            Engine.diagnostic = diagnostic;
            Engine.importantOnly = importantOnly;
        }

        public static void WriteDiagnostic(string str, bool important = true)
        {
            if (important || !importantOnly)
            {
                diagnostic?.Invoke(str);
            }
        }
    }
}
