using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.Tokens
{
    public class SymbolToken : Token
    {
        public SymbolType Symbol { get; }

        public SymbolToken(SymbolType symbol, Position position)
            : base(position) => Symbol = symbol;
    }
}
