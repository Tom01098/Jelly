using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.Tokens
{
    public abstract class Token
    {
        public Position Position { get; }

        public Token(Position position) => Position = position;
    }
}
