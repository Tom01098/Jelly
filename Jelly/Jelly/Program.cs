using Jelly.Core.Parsing;
using Jelly.Core.Parsing.AST;
using Jelly.Core.Utility;
using System;
using System.Collections.Generic;
using System.IO;

namespace Jelly
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var files = FindFiles(args[0]);

                var ast = GetAST(files, args[0]);

                Console.WriteLine("Success");
            }
            catch (JellyException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Unhandled error: {e}");
            }

            Console.ReadKey();
        }

        private static string[] FindFiles(string folder)
        {
            Console.WriteLine("Searching directory for .jelly files");
            var files = Directory.GetFiles(folder, "*.jelly", SearchOption.AllDirectories);
            Console.WriteLine("    Found:");

            for (int i = 0; i < files.Length; i++)
            {
                files[i] = files[i].Replace(folder, "");
                Console.WriteLine($"        {files[i]}");
            }

            return files;
        }

        private static List<FunctionNode> GetAST(string[] files, string folder)
        {
            Console.WriteLine("Parsing files");

            var ast = new List<FunctionNode>();

            foreach (var file in files)
            {
                var contents = File.ReadAllText(folder + file);

                ast.AddRange(new Parser().Parse(
                    new Lexer().Lex(contents, file)));
            }

            Console.WriteLine($"    Found {ast.Count} functions");

            return ast;
        }
    }
}
