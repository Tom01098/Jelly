using System.Collections.Generic;

namespace Jelly.Core.Parsing.AST
{
    public class IfBlockNode : IConstructNode
    {
        public List<ConditionalBlockNode> ConditionalBlocks { get; }

        public IfBlockNode(List<ConditionalBlockNode> conditionalBlocks) =>
            ConditionalBlocks = conditionalBlocks;
    }
}
