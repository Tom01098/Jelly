using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.Tokens
{
    public class EOFToken : Token
    {
        public EOFToken(Position position)
            : base(position) { }
    }
}
