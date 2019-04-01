using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.AST
{
    public class CallNode : Node, IStatementNode, ITermNode
    {
        public IdentifierNode Identifier { get; }
        public ITermNode[] Arguments { get; }

        public CallNode(IdentifierNode identifier,
                        ITermNode[] arguments,
                        Position position)
            : base(position) => 
            (Identifier, Arguments) = (identifier, arguments);
    }
}
