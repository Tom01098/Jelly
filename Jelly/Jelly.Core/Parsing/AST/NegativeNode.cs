using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.AST
{
    public class NegativeNode : Node, ITermNode
    {
        public ITermNode Term { get; }

        public NegativeNode(ITermNode term, Position position)
            : base(position) => Term = term;
    }
}
