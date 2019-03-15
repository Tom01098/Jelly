using Jelly.Core.Utility;
using System.Collections.Generic;

namespace Jelly.Core.Parsing.AST
{
    public class IfBlockNode : Node, IConstructNode
    {
        public List<ConditionalBlockNode> ConditionalBlocks { get; }

        public IfBlockNode(List<ConditionalBlockNode> conditionalBlocks, 
                           Position position)
            : base(position) =>
            ConditionalBlocks = conditionalBlocks;
    }
}
