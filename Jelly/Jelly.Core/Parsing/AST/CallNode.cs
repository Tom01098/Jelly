using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.AST
{
    public class CallNode : Node, IValueNode, IStatementNode
    {
        public IdentifierNode Identifier { get; }
        public ArgumentsNode Arguments { get; }

        public CallNode(IdentifierNode identifier, 
                        ArgumentsNode args,
                        Position position)
            : base(position) =>
            (Identifier, Arguments) = (identifier, args);
    }
}
