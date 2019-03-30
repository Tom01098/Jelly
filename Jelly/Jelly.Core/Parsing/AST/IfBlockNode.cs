using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.AST
{
    public class IfBlockNode : Node, IConstructNode
    {
        public ConditionalBlockNode[] Blocks { get; }

        public IfBlockNode(ConditionalBlockNode[] blocks,
                           Position position)
            : base(position) => Blocks = blocks;
    }
}
