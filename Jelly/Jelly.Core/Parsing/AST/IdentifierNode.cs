namespace Jelly.Core.Parsing.AST
{
    public class IdentifierNode : IValueNode
    {
        public string Identifier { get; }

        public IdentifierNode(string identifier) => Identifier = identifier;
    }
}
