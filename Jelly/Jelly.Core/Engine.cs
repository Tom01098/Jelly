using Jelly.Core.Interpreting;
using Jelly.Core.Linking;
using Jelly.Core.Optimising;
using Jelly.Core.Parsing;
using Jelly.Core.Parsing.AST;
using Jelly.Core.Utility;
using Jelly.Core.Verifying;
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
        private static Action<string> error;

        public static List<IFunction> GetAST(string folder)
        {
            var files = FileUtility.FindJellyFiles(folder);
            WriteDiagnostic($"Found {files.Length} files.");

            var ast = new List<FunctionNode>();

            foreach (var file in files)
            {
                var relativePath = file.Replace(folder, "");
                var text = File.ReadAllText(file);

                var tokens = new Lexer().Lex(text, relativePath);
                var functions = new Parser().Parse(tokens);

                ast.AddRange(functions);
            }

            WriteDiagnostic("Parsing user files succeeded");

            return new Linker().LinkAST(ast);
        }

        public static void Verify(List<IFunction> ast)
        {
            Verifier.Verify(ast);
        }

        public static List<IFunction> Optimise(List<IFunction> ast)
        {
            return new Optimiser().Optimise(ast);
        }

        public static void Execute(List<IFunction> ast)
        {
            new Interpreter().Interpret(ast);
        }

        public static void SetDiagnosticOut(Action<string> diagnostic)
        {
            Engine.diagnostic = diagnostic;
        }

        public static void SetErrorOut(Action<string> error)
        {
            Engine.error = error;
        }

        public static void WriteDiagnostic(string str)
        {
             diagnostic?.Invoke(str);
        }

        public static void WriteError(string str)
        {
            error?.Invoke(str);
        }
    }
}
