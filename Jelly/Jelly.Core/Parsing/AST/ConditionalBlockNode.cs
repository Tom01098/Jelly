using Jelly.Core.Utility;
using System.Collections.Generic;

namespace Jelly.Core.Parsing.AST
{
    public class ConditionalBlockNode : Node
    {
        public IValueNode Condition { get; }
        public List<IConstructNode> Constructs { get; }

        public ConditionalBlockNode(IValueNode condition,
                                    List<IConstructNode> constructs,
                                    Position position)
            : base(position) =>
            (Condition, Constructs) = (condition, constructs);
    }
}
