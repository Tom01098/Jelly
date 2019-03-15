using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.AST
{
    public class OperationNode : Node, IValueNode
    {
        public IValueNode LHS { get; }
        public OperatorType Operator { get; }
        public IValueNode RHS { get; }

        public OperationNode(IValueNode lhs,
                             OperatorType op, 
                             IValueNode rhs,
                             Position position) 
            : base(position) =>
            (LHS, Operator, RHS) = (lhs, op, rhs);
    }
}
