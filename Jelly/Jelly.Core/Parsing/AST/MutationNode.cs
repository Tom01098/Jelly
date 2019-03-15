namespace Jelly.Core.Parsing.AST
{
    public class MutationNode : IStatementNode
    {
        public IdentifierNode Identifier { get; }
        public IValueNode Value { get; }

        public MutationNode(IdentifierNode identifier, IValueNode value) =>
            (Identifier, Value) = (identifier, value);
    }
}
