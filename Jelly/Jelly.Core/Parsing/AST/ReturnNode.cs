using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.AST
{
    public class ReturnNode : Node, IStatementNode
    {
        public ITermNode Value { get; }

        public ReturnNode(ITermNode value, Position position)
            : base(position) => Value = value;
    }
}
