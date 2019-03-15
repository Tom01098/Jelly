namespace Jelly.Core.Parsing.AST
{
    public class CallNode : IValueNode, IStatementNode
    {
        public IdentifierNode Identifier { get; }
        public ArgumentsNode Arguments { get; }

        public CallNode(IdentifierNode identifier, ArgumentsNode args) =>
            (Identifier, Arguments) = (identifier, args);
    }
}
