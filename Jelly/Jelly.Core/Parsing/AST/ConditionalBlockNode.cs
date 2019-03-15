using System.Collections.Generic;

namespace Jelly.Core.Parsing.AST
{
    public class ConditionalBlockNode
    {
        public IValueNode Condition { get; }
        public List<IConstructNode> Constructs { get; }

        public ConditionalBlockNode(IValueNode condition,
                                    List<IConstructNode> constructs) =>
            (Condition, Constructs) = (condition, constructs);
    }
}
