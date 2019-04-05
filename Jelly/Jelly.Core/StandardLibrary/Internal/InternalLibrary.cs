using Jelly.Core.Utility;
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
            var input = Console.ReadLine();

            if (double.TryParse(input, out var num))
            {
                return num;
            }

            throw new JellyException("User input can only be a number");
        }
        #endregion

        #region Number Utility
        [InternalFunction(true)]
        public static double Pow(double a, double b)
        {
            return Math.Pow(a, b);
        }

        [InternalFunction(true)]
        public static double IsNaN(double x)
        {
            return double.IsNaN(x) ? 1 : 0;
        }

        [InternalFunction(true)]
        public static double Ceil(double x)
        {
            return Math.Ceiling(x);
        }

        [InternalFunction(true)]
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
            if (min % 1 != 0 || max % 1 != 0)
            {
                throw new JellyException($"{nameof(RandomIntBetween)} " +
                    $"can only accept integers but got '{min}' and '{max}'");
            }

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
