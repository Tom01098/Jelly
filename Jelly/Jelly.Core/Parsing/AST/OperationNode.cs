namespace Jelly.Core.Parsing.AST
{
    public class OperationNode : ValueNode
    {
        public ValueNode LHS { get; }
        public OperatorType Operator { get; }
        public ValueNode RHS { get; }

        public OperationNode(ValueNode lhs, OperatorType op, ValueNode rhs) =>
            (LHS, Operator, RHS) = (lhs, op, rhs);
    }
}
