using System;
using System.IO;
using Jelly.Core.Parsing;
using Jelly.Core.Utility;

namespace Jelly
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine($"Begin reading from {args[0]}");
                var text = File.ReadAllText(args[0]);
                Console.WriteLine("Begin lexing");
                var tokens = new Lexer().Lex(text, args[0]);
                Console.WriteLine("Begin parsing");
                var ast = new Parser().Parse(tokens);
                Console.WriteLine("Success");
            }
            catch (JellyException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unhandled error: {e}");
            }

            Console.ReadKey();
        }
    }
}
