using Jelly.Core.Utility;
using System.Collections.Generic;

namespace Jelly.Core.Parsing.AST
{
    public class CallNode : Node, IValueNode, IStatementNode
    {
        public IdentifierNode Identifier { get; }
        public List<IValueNode> Arguments { get; }

        public CallNode(IdentifierNode identifier,
                        List<IValueNode> args,
                        Position position)
            : base(position) =>
            (Identifier, Arguments) = (identifier, args);
    }
}
