using Jelly.Core.Parsing.AST;
using System;

namespace Jelly.Core.StandardLibrary.Internal
{
    public class Write : IFunction
    {
        public double Execute(double value)
        {
            Console.WriteLine(value);
            return double.NaN;
        }
    }
}
