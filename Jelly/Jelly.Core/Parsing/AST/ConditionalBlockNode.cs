using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.AST
{
    public class ConditionalBlockNode : Node
    {
        public ITermNode Condition { get; }
        public IConstructNode[] Constructs { get; }

        public ConditionalBlockNode(ITermNode condition,
                                    IConstructNode[] constructs,
                                    Position position)
            : base(position) => 
            (Condition, Constructs) = (condition, constructs);
    }
}
