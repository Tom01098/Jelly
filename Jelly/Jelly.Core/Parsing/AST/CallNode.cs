using Jelly.Core.Utility;
using System.Collections.Generic;

namespace Jelly.Core.Parsing.AST
{
    public class CallNode : Node, IStatementNode, ITermNode
    {
        public IdentifierNode Identifier { get; }
        public List<ITermNode> Arguments { get; }

        public CallNode(IdentifierNode identifier,
                        List<ITermNode> arguments,
                        Position position)
            : base(position) => 
            (Identifier, Arguments) = (identifier, arguments);
    }
}
