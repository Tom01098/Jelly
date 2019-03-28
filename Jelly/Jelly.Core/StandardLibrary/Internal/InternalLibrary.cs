using System;

namespace Jelly.Core.StandardLibrary.Internal
{
    public class InternalLibrary
    {
        #region IO
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
        public static double WriteChar(double value)
        {
            Console.Write((char)value);
            return double.NaN;
        }

        [InternalFunction]
        public static double Read()
        {
            return double.Parse(Console.ReadLine());
        }
        #endregion

        #region Number Utility
        [InternalFunction]
        public static double Pow(double a, double b)
        {
            return Math.Pow(a, b);
        }

        [InternalFunction]
        public static double IsNaN(double x)
        {
            return double.IsNaN(x) ? 1 : 0;
        }

        [InternalFunction]
        public static double Ceil(double x)
        {
            return Math.Ceiling(x);
        }

        [InternalFunction]
        public static double Floor(double x)
        {
            return Math.Floor(x);
        }
        #endregion

        #region Random
        private static readonly Random random = new Random();

        [InternalFunction]
        public static double RandomInt()
        {
            return random.Next();
        }

        [InternalFunction]
        public static double RandomIntBetween(double min, double max)
        {
            return random.Next((int)min, (int)max);
        }

        [InternalFunction]
        public static double Random()
        {
            return random.NextDouble();
        }
        #endregion
    }
}
