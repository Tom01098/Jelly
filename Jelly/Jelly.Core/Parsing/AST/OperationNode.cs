namespace Jelly.Core.Parsing.AST
{
    public class OperationNode : IValueNode
    {
        public IValueNode LHS { get; }
        public OperatorType Operator { get; }
        public IValueNode RHS { get; }

        public OperationNode(IValueNode lhs, OperatorType op, IValueNode rhs) =>
            (LHS, Operator, RHS) = (lhs, op, rhs);
    }
}
