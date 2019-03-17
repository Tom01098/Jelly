using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.AST
{
    public class IdentifierNode : Node, ITermNode
    {
        public string Identifier { get; }

        public IdentifierNode(string identifier, Position position)
            : base(position) => Identifier = identifier;
    }
}
