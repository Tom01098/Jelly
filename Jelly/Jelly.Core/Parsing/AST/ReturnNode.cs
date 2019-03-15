namespace Jelly.Core.Parsing.AST
{
    public class ReturnNode : StatementNode
    {
        public ValueNode Value { get; }

        public ReturnNode(ValueNode value) =>
            Value = value;
    }
}
