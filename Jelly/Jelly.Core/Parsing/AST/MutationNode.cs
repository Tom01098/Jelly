using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.AST
{
    public class MutationNode : Node, IStatementNode
    {
        public IdentifierNode Identifier { get; }
        public ITermNode Value { get; }

        public MutationNode(IdentifierNode identifier,
                            ITermNode value,
                            Position position)
            : base(position) => (Identifier, Value) = (identifier, value);
    }
}
