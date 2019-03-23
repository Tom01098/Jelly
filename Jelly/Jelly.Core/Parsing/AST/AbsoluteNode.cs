using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.AST
{
    public class AbsoluteNode : Node, ITermNode
    {
        public ITermNode Value { get; }

        public AbsoluteNode(ITermNode value, Position position)
            : base(position) => Value = value;
    }
}
