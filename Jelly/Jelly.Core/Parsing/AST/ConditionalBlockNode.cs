using Jelly.Core.Utility;
using System.Collections.Generic;

namespace Jelly.Core.Parsing.AST
{
    public class ConditionalBlockNode : Node
    {
        public ITermNode Condition { get; }
        public List<IConstructNode> Constructs { get; }

        public ConditionalBlockNode(ITermNode condition,
                                    List<IConstructNode> constructs,
                                    Position position)
            : base(position) => 
            (Condition, Constructs) = (condition, constructs);
    }
}
