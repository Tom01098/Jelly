using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.AST
{
    public class LoopNode : Node, IConstructNode 
    {
        public ConditionalBlockNode Block { get; }

        public LoopNode(ConditionalBlockNode block, Position position)
            : base(position) => Block = block;
    }
}
