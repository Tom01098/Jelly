using Jelly.Core.Parsing.Tokens;
using System;
using System.Collections.Generic;

namespace Jelly.Core.Parsing
{
    public class Lexer
    {
        public List<Token> Lex(string text)
        {
            var span = text.AsSpan();
            var index = 0;

            var lineNumber = 1;
            var characterNumber = 1;

            var tokens = new List<Token>();



            return tokens;
        }
    }
}
