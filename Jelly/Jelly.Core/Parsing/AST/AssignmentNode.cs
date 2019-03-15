namespace Jelly.Core.Parsing.AST
{
    public class AssignmentNode : StatementNode
    {
        public IdentifierNode Identifier { get; }
        public ValueNode Value { get; }

        public AssignmentNode(IdentifierNode identifier, ValueNode value) =>
            (Identifier, Value) = (identifier, value);
    }
}
