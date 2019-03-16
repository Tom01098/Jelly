using Jelly.Core.Parsing.Tokens;
using System.Collections.Generic;

namespace Jelly.Core.Parsing
{
    public class TokenEnumerator
    {
        private List<Token> tokens;
        private Stack<int> indices;

        public TokenEnumerator(List<Token> tokens)
        {
            this.tokens = tokens;
            indices = new Stack<int>();
        }

        public Token Current
        {
            get
            {
                if (indices.TryPeek(out var index))
                {
                    if (index < tokens.Count)
                    {
                        return tokens[index];
                    }
                }

                return null;
            }
        }

        public void MoveNext()
        {
            if (indices.TryPop(out var index))
            {
                indices.Push(index + 1);
            }
            else
            {
                indices.Push(0);
            }
        }

        public Token LookAhead(int num)
        {
            if (indices.TryPeek(out var index))
            {
                var newIndex = index + num;

                if (newIndex < tokens.Count)
                {
                    return tokens[newIndex];
                }
            }

            return null;
        }
    }
}
