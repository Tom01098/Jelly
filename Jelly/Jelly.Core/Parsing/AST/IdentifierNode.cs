namespace Jelly.Core.Parsing.AST
{
    public class IdentifierNode : ValueNode
    {
        public string Identifier { get; }

        public IdentifierNode(string identifier) => Identifier = identifier;
    }
}
