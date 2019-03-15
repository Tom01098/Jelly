namespace Jelly.Core.Parsing.AST
{
    public class ReturnNode : IStatementNode
    {
        public IValueNode Value { get; }

        public ReturnNode(IValueNode value) =>
            Value = value;
    }
}
