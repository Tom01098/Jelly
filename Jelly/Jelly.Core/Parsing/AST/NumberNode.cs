using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.AST
{
    public class NumberNode : Node, IValueNode
    {
        public double Number { get; }

        public NumberNode(double number, Position position)
            : base(position) => Number = number;
    }
}
