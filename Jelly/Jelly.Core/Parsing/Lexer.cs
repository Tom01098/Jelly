using Jelly.Core.Parsing.Tokens;
using Jelly.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Char;

namespace Jelly.Core.Parsing
{
    /// <summary>
    /// Converts an input string into a list of tokens with the same meaning.
    /// </summary>
    public class Lexer
    {
        public List<Token> Lex(string text, string file)
        {
            var span = text.AsSpan();
            var index = 0;

            var lineNumber = 1;
            var characterNumber = 1;

            var tokens = new List<Token>();

            while (index < span.Length)
            {
                // Identifiers/Keywords
                if (IsLetter(span[index]) || span[index] == '_')
                {
                    var start = index;
                    var position = GetPosition();

                    do
                    {
                        NextChar();
                    }
                    while (index < span.Length && (IsLetter(span[index]) || IsDigit(span[index]) || span[index] == '_'));

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
                        case "loop":
                            tokens.Add(new KeywordToken(KeywordType.Loop, position));
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
                // Return
                else if (span[index] == '~')
                {
                    tokens.Add(new SymbolToken(SymbolType.Return, GetPosition()));
                }
                // Open angle parenthesis / Less than (or equal to)
                else if (span[index] == '<')
                {
                    var position = GetPosition();
                    NextChar();

                    if (index < span.Length && span[index] == '=')
                    {
                        tokens.Add(new OperatorToken(OperatorType.LessThanOrEqualTo, position));
                    }
                    else if (index < span.Length && span[index] == '<')
                    {
                        tokens.Add(new OperatorToken(OperatorType.LessThan, position));
                    }
                    else
                    {
                        tokens.Add(new SymbolToken(SymbolType.OpenAngleParenthesis, position));
                        continue;
                    }
                }
                // Close angle parenthesis / Greater than (or equal to)
                else if (span[index] == '>')
                {
                    var position = GetPosition();
                    NextChar();

                    if (index < span.Length && span[index] == '=')
                    {
                        tokens.Add(new OperatorToken(OperatorType.GreaterThanOrEqualTo, position));
                    }
                    else if (index < span.Length && span[index] == '>')
                    {
                        tokens.Add(new OperatorToken(OperatorType.GreaterThan, position));
                    }
                    else
                    {
                        tokens.Add(new SymbolToken(SymbolType.CloseAngleParenthesis, position));
                        continue;
                    }
                }
                // Mutation / Equal to / Assignment
                else if (span[index] == '=')
                {
                    var position = GetPosition();
                    NextChar();

                    if (index < span.Length && span[index] == '>')
                    {
                        tokens.Add(new SymbolToken(SymbolType.Mutation, position));
                    }
                    else if (index < span.Length && span[index] == '=')
                    {
                        tokens.Add(new OperatorToken(OperatorType.EqualTo, position));
                    }
                    else
                    {
                        tokens.Add(new SymbolToken(SymbolType.Assignment, position));
                        continue;
                    }
                }
                // Unequal to
                else if (span[index] == '!')
                {
                    var position = GetPosition();
                    NextChar();

                    if (index < span.Length && span[index] == '=')
                    {
                        tokens.Add(new OperatorToken(OperatorType.UnequalTo, position));
                    }
                    else
                    {
                        tokens.Add(new SymbolToken(SymbolType.Exclamation, position));
                        continue;
                    }
                }
                // Open parenthesis
                else if (span[index] == '(')
                {
                    tokens.Add(new SymbolToken(SymbolType.OpenParenthesis, GetPosition()));
                }
                // Close parenthesis
                else if (span[index] == ')')
                {
                    tokens.Add(new SymbolToken(SymbolType.CloseParenthesis, GetPosition()));
                }
                // Add
                else if (span[index] == '+')
                {
                    tokens.Add(new OperatorToken(OperatorType.Add, GetPosition()));
                }
                // Subtract
                else if (span[index] == '-')
                {
                    tokens.Add(new OperatorToken(OperatorType.Subtract, GetPosition()));
                }
                // Multiply
                else if (span[index] == '*')
                {
                    tokens.Add(new OperatorToken(OperatorType.Multiply, GetPosition()));
                }
                // Divide
                else if (span[index] == '/')
                {
                    tokens.Add(new OperatorToken(OperatorType.Divide, GetPosition()));
                }
                // Modulo
                else if (span[index] == '%')
                {
                    tokens.Add(new OperatorToken(OperatorType.Modulo, GetPosition()));
                }
                // Comma
                else if (span[index] == ',')
                {
                    tokens.Add(new SymbolToken(SymbolType.Comma, GetPosition()));
                }
                // Pipe
                else if (span[index] == '|')
                {
                    tokens.Add(new SymbolToken(SymbolType.Pipe, GetPosition()));
                }
                // Newline
                else if (span[index] == '\r')
                {
                    if (tokens.Count != 0 && !(tokens.Last() is EOLToken))
                    {
                        tokens.Add(new EOLToken(GetPosition()));
                    }

                    lineNumber++;
                    characterNumber = -1;
                    NextChar();
                }
                // Line continuation
                else if (span[index] == ';')
                {
                    bool isComment = false;

                    do
                    {
                        NextChar();

                        if (span[index] == '@')
                        {
                            isComment = true;
                        }
                        else if (!isComment && !IsWhiteSpace(span[index]))
                        {
                            throw new JellyException(
                                "Only a comment is allowed after a line continuation",
                                GetPosition());
                        }
                    }
                    while (index < span.Length && 
                    !(span[index] == '\r' 
                    || (!isComment && !IsWhiteSpace(span[index]))));

                    lineNumber++;
                    characterNumber = -1;
                }
                // Comment
                else if (span[index] == '@')
                {
                    if (tokens.Count != 0 && !(tokens.Last() is EOLToken))
                    {
                        tokens.Add(new EOLToken(GetPosition()));
                    }

                    do
                    {
                        NextChar();
                    }
                    while (index < span.Length && span[index] != '\r');

                    lineNumber++;
                    characterNumber = -1;
                    NextChar();
                }
                // Invalid
                else if (!IsWhiteSpace(span[index]))
                {
                    throw new JellyException(
                        $"'{span[index]}' is an invalid character", 
                        GetPosition());
                }

                NextChar();
            }

            // If an End-Of-Line token was not the last token, append one
            if (tokens.Count != 0 && !(tokens.Last() is EOLToken))
            {
                tokens.Add(new EOLToken(GetPosition()));
            }

            // Add an End-Of-File token
            tokens.Add(new EOFToken(GetPosition()));

            return tokens;

            // Local functions
            Position GetPosition() => new Position(file, lineNumber, characterNumber);

            void NextChar()
            {
                index++;
                characterNumber++;
            }
        }
    }
}
