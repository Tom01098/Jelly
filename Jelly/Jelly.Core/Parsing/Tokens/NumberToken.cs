using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.Tokens
{
    public class NumberToken : Token
    {
        public double Number { get; }

        public NumberToken(double number, Position position)
            : base(position) => Number = number;
    }
}
