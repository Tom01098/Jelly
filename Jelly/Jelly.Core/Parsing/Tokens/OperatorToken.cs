using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.Tokens
{
    public class OperatorToken : Token
    {
        public OperatorType Operator { get; }

        public OperatorToken(OperatorType @operator, Position position)
            : base(position) => Operator = @operator;
    }
}
