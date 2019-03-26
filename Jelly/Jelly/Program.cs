using Jelly.Core;
using Jelly.Core.Utility;
using System;

namespace Jelly
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Engine.SetDiagnosticOut(x => Console.WriteLine(x));
                var ast = Engine.GetAST(args[0]);

                do
                {
                    Engine.Execute(ast);
                    Console.WriteLine("Press 'y' to execute again, any other key to exit.");
                }
                while (char.ToLower(Console.ReadKey(true).KeyChar) == 'y');
            }
            catch (JellyException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Unhandled error: {e}");
                Console.ReadKey();
            }

            Console.ForegroundColor = ConsoleColor.White;
            Engine.SetDiagnosticOut(null);
        }
    }
}
