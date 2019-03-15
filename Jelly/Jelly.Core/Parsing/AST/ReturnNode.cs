using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.AST
{
    public class ReturnNode : Node, IStatementNode
    {
        public IValueNode Value { get; }

        public ReturnNode(IValueNode value, Position position)
            : base(position) => Value = value;
    }
}
