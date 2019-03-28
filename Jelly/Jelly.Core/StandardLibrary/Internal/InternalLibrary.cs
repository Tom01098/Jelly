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

        [InternalFunction]
        public static double Pow(double a, double b)
        {
            return Math.Pow(a, b);
        }
        #endregion

        #region Number Utility
        [InternalFunction]
        public static double IsNaN(double x)
        {
            return double.IsNaN(x) ? 1 : 0;
        }
        #endregion

        #region Random
        [InternalFunction]
        public static double RandomInt()
        {
            return new Random().Next();
        }

        [InternalFunction]
        public static double RandomIntBetween(double min, double max)
        {
            return new Random().Next((int)min, (int)max);
        }

        [InternalFunction]
        public static double Random()
        {
            return new Random().NextDouble();
        }
        #endregion
    }
}
