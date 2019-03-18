using Jelly.Core.Utility;
using System.Collections.Generic;

namespace Jelly.Core.Parsing.AST
{
    public class CallNode : Node, IStatementNode, ITermNode
    {
        public IdentifierNode Identifier { get; }
        public List<ValueNode> Arguments { get; }

        public CallNode(IdentifierNode identifier,
                        List<ValueNode> arguments,
                        Position position)
            : base(position) => 
            (Identifier, Arguments) = (identifier, arguments);
    }
}
