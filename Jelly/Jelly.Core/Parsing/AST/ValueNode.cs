using Jelly.Core.Parsing.Tokens;
using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.AST
{
    public class ValueNode : Node, ITermNode
    {
        public ITermNode LHS { get; }
        public OperatorType Operator { get; }
        public ITermNode RHS { get; }

        public ValueNode(ITermNode lhs,
                         OperatorType op,
                         ITermNode rhs,
                         Position position)
            : base(position) =>
            (LHS, Operator, RHS) = (lhs, op, rhs);
    }
}
