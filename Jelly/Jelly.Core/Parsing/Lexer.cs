using Jelly.Core.Parsing.Tokens;
using Jelly.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Char;

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

            while (index < span.Length)
            {
                // Identifiers/Keywords
                if (IsLetter(span[index]))
                {
                    var start = index;
                    var position = GetPosition();

                    do
                    {
                        NextChar();
                    }
                    while (index < span.Length && IsLetter(span[index]));

                    var identifier = span.Slice(start, index - start).ToString();

                    switch (identifier)
                    {
                        case "if":
                            tokens.Add(new KeywordToken(KeywordType.If, position));
                            break;
                        case "elif":
                            tokens.Add(new KeywordToken(KeywordType.Elif, position));
                            break;
                        case "else":
                            tokens.Add(new KeywordToken(KeywordType.Else, position));
                            break;
                        case "end":
                            tokens.Add(new KeywordToken(KeywordType.End, position));
                            break;
                        default:
                            tokens.Add(new IdentifierToken(identifier, position));
                            break;
                    }

                    continue;
                }
                // Numbers
                else if (IsDigit(span[index]) || span[index] == '.')
                {
                    var start = index;
                    var position = GetPosition();

                    do
                    {
                        NextChar();
                    }
                    while (index < span.Length && (IsDigit(span[index]) || span[index] == '.'));

                    var number = double.Parse(span.Slice(start, index - start));

                    tokens.Add(new NumberToken(number, position));

                    continue;
                }

                NextChar();
            }

            // If an End-Of-Line token was not the last token, append one
            if (!(tokens.Last() is EOLToken))
            {
                tokens.Add(new EOLToken(GetPosition()));
            }

            // Add an End-Of-File token
            tokens.Add(new EOFToken(GetPosition()));

            return tokens;

            // Local functions
            Position GetPosition() => new Position(lineNumber, characterNumber);

            void NextChar()
            {
                index++;
                characterNumber++;
            }

            void NextLine()
            {
                index++;
                lineNumber++;
                characterNumber = 1;
            }
        }
    }
}
