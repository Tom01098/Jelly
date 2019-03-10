using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.Tokens
{
    public class KeywordToken : Token
    {
        public KeywordType Keyword { get; }

        public KeywordToken(KeywordType keyword, Position position)
            : base(position) => Keyword = keyword;
    }
}
