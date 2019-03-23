using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.AST
{
    public class LoopBlockNode : Node, IConstructNode 
    {
        public ConditionalBlockNode Block { get; }

        public LoopBlockNode(ConditionalBlockNode block, Position position)
            : base(position) => Block = block;
    }
}
