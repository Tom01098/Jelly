using Jelly.Core.Utility;
using System.Collections.Generic;

namespace Jelly.Core.Parsing.AST
{
    public class IfBlockNode : Node, IConstructNode
    {
        public List<ConditionalBlockNode> Blocks { get; }

        public IfBlockNode(List<ConditionalBlockNode> blocks,
                           Position position)
            : base(position) => Blocks = blocks;
    }
}
