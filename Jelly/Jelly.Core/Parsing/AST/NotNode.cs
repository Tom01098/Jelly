using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.AST
{
    public class NotNode : Node, ITermNode
    {
        public ValueNode Value { get; }

        public NotNode(ValueNode value, Position position)
            : base(position) => Value = value;
    }
}
