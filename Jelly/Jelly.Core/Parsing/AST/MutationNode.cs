namespace Jelly.Core.Parsing.AST
{
    public class MutationNode : StatementNode
    {
        public IdentifierNode Identifier { get; }
        public ValueNode Value { get; }

        public MutationNode(IdentifierNode identifier, ValueNode value) =>
            (Identifier, Value) = (identifier, value);
    }
}
