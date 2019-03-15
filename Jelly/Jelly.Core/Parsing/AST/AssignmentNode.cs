using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.AST
{
    public class AssignmentNode : Node, IStatementNode
    {
        public IdentifierNode Identifier { get; }
        public IValueNode Value { get; }

        public AssignmentNode(IdentifierNode identifier, 
                              IValueNode value,
                              Position position)
            : base(position) =>
            (Identifier, Value) = (identifier, value);
    }
}
