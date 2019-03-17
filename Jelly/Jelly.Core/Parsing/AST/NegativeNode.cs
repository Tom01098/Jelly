using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.AST
{
    public class NegativeNode : Node, ITermNode
    {
        public ValueNode Value { get; }

        public NegativeNode(ValueNode value, Position position)
            : base(position) => Value = value;
    }
}
