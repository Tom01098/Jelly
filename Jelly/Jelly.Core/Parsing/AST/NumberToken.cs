using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.AST
{
    public class NumberToken : Node, IValueNode
    {
        public double Number { get; }

        public NumberToken(double number, Position position)
            : base(position) => Number = number;
    }
}
