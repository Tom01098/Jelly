using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.AST
{
    public class NotNode : Node, ITermNode
    {
        public ITermNode Term { get; }

        public NotNode(ITermNode term, Position position)
            : base(position) => Term = term;
    }
}
