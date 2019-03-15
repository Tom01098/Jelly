using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.AST
{
    public class MutationNode : Node, IStatementNode
    {
        public IdentifierNode Identifier { get; }
        public IValueNode Value { get; }

        public MutationNode(IdentifierNode identifier, 
                            IValueNode value,
                            Position position)
            : base(position) =>
            (Identifier, Value) = (identifier, value);
    }
}
