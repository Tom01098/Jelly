namespace Jelly.Core.Parsing.AST
{
    public class AssignmentNode : IStatementNode
    {
        public IdentifierNode Identifier { get; }
        public IValueNode Value { get; }

        public AssignmentNode(IdentifierNode identifier, IValueNode value) =>
            (Identifier, Value) = (identifier, value);
    }
}
