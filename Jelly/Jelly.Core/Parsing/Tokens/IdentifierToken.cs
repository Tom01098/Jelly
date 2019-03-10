using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.Tokens
{
    public class IdentifierToken : Token
    {
        public string Identifier { get; }

        public IdentifierToken(string identifier, Position position)
            : base(position) => Identifier = identifier;
    }
}
