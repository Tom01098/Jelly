using CommandLine;
using Jelly.Core;
using Jelly.Core.Utility;
using System;

namespace Jelly
{
    internal class Program
    {
        /// <summary>
        /// CommandLineParser options
        /// </summary>
        private class Options
        {
            [Value(0)]
            public string Path { get; set; }

            [Option('o')]
            public bool Optimise { get; set; }
        }

        private static void Main(string[] args)
        {
            try
            {
                // Parse the command line arguments
                var options = new Options();
                Parser.Default.ParseArguments<Options>(args).WithParsed(result =>
                {
                    options.Path = result.Path;
                    options.Optimise = result.Optimise;
                });

                // Set diagnostics
                Engine.SetDiagnosticOut(x =>
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(x);
                });

                Engine.SetErrorOut(x =>
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(x);
                });

                // Parse and verify the given files
                var ast = Engine.GetAST(options.Path);
                Engine.Verify(ast);

                // Optimise the ast if the option was selected
                if (options.Optimise)
                {
                    ast = Engine.Optimise(ast);
                }

                // Repetitively execute the ast
                do
                {
                    Engine.Execute(ast);
                    Console.WriteLine("Press 'y' to execute again, any other key to exit.");
                }
                while (char.ToLower(Console.ReadKey(true).KeyChar) == 'y');
            }
            // Exception handling
            catch (JellyException e)
            {
                if (e.Message != "")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                }

                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Unhandled error: {e}");
                Console.ReadKey();
            }

            // Cleanup
            Console.ForegroundColor = ConsoleColor.White;
            Engine.SetDiagnosticOut(null);
            Engine.SetErrorOut(null);
        }
    }
}
