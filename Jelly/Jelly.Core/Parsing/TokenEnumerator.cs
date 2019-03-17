using Jelly.Core.Parsing.Tokens;
using System.Collections.Generic;

namespace Jelly.Core.Parsing
{
    /// <summary>
    /// Enumerates over a given list of tokens with look ahead ability.
    /// </summary>
    public class TokenEnumerator
    {
        private List<Token> tokens;
        private int index;

        public TokenEnumerator(List<Token> tokens)
        {
            this.tokens = tokens;
            index = 0;
        }

        public Token Current
        {
            get
            {
                if (index < tokens.Count)
                {
                    return tokens[index];
                }

                return null;
            }
        }

        public void MoveNext()
        {
            index++;
        }

        public Token LookAhead(int num)
        {
            var newIndex = index + num;

            if (newIndex < tokens.Count)
            {
                return tokens[newIndex];
            }

            return null;
        }
    }
}
