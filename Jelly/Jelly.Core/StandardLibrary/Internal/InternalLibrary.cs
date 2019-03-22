using Jelly.Core.Interpreting;
using System;

namespace Jelly.Core.StandardLibrary.Internal
{
    public class InternalLibrary
    {
        [InternalFunction]
        public static double Write(double value)
        {
            Console.Write(value);
            return double.NaN;
        }

        [InternalFunction]
        public static double WriteLine(double value)
        {
            Console.WriteLine(value);
            return double.NaN;
        }

        [InternalFunction]
        public static double WriteNewLine()
        {
            Console.WriteLine();
            return double.NaN;
        }

        [InternalFunction]
        public static double WriteChar(double value)
        {
            Console.Write(char.ConvertFromUtf32((int)value));
            return double.NaN;
        }

        [InternalFunction]
        public static double Read()
        {
            return double.Parse(Console.ReadLine());
        }

        [InternalFunction]
        public static double Pow(double a, double b)
        {
            return Math.Pow(a, b);
        }

        [InternalFunction]
        public static double Sqrt(double x)
        {
            return Math.Sqrt(x);
        }

        [InternalFunction]
        public static double IsNaN(double x)
        {
            return double.IsNaN(x) ? 1 : 0;
        }
    }
}
