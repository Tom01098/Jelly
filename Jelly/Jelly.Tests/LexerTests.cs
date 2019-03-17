using Jelly.Core.Parsing;
using Jelly.Core.Parsing.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using static Jelly.Tests.TestUtility;

namespace Jelly.Tests
{
    [TestClass]
    public class LexerTests
    {
        public List<Token> GetTokens(string text) =>
            new Lexer().Lex(text, "test");

        #region Single
        [TestMethod]
        public void Identifier()
        {
            var actual = GetTokens("abc");

            var expected = new List<Token>
            {
                new IdentifierToken("abc", Position(1, 1)),
                new EOLToken(Position(1, 4)),
                new EOFToken(Position(1, 4))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IdentifierWithUnderscores()
        {
            var actual = GetTokens("_I_Hate_Underscores_But_Some_People_Dont_");

            var expected = new List<Token>
            {
                new IdentifierToken("_I_Hate_Underscores_But_Some_People_Dont_", Position(1, 1)),
                new EOLToken(Position(1, 42)),
                new EOFToken(Position(1, 42))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Number()
        {
            var actual = GetTokens("1");

            var expected = new List<Token>
            {
                new NumberToken(1, Position(1, 1)),
                new EOLToken(Position(1, 2)),
                new EOFToken(Position(1, 2))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DecimalNumber()
        {
            var actual = GetTokens("3.5");

            var expected = new List<Token>
            {
                new NumberToken(3.5, Position(1, 1)),
                new EOLToken(Position(1, 4)),
                new EOFToken(Position(1, 4))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LeadingDecimalNumber()
        {
            var actual = GetTokens(".5");

            var expected = new List<Token>
            {
                new NumberToken(.5, Position(1, 1)),
                new EOLToken(Position(1, 3)),
                new EOFToken(Position(1, 3))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void End()
        {
            var actual = GetTokens("end");

            var expected = new List<Token>
            {
                new KeywordToken(KeywordType.End, Position(1, 1)),
                new EOLToken(Position(1, 4)),
                new EOFToken(Position(1, 4))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void If()
        {
            var actual = GetTokens("if");

            var expected = new List<Token>
            {
                new KeywordToken(KeywordType.If, Position(1, 1)),
                new EOLToken(Position(1, 3)),
                new EOFToken(Position(1, 3))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Elif()
        {
            var actual = GetTokens("elif");

            var expected = new List<Token>
            {
                new KeywordToken(KeywordType.Elif, Position(1, 1)),
                new EOLToken(Position(1, 5)),
                new EOFToken(Position(1, 5))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Else()
        {
            var actual = GetTokens("else");

            var expected = new List<Token>
            {
                new KeywordToken(KeywordType.Else, Position(1, 1)),
                new EOLToken(Position(1, 5)),
                new EOFToken(Position(1, 5))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void OpenAngleParenthesis()
        {
            var actual = GetTokens("<");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.OpenAngleParenthesis, Position(1, 1)),
                new EOLToken(Position(1, 2)),
                new EOFToken(Position(1, 2))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CloseAngleParenthesis()
        {
            var actual = GetTokens(">");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.CloseAngleParenthesis, Position(1, 1)),
                new EOLToken(Position(1, 2)),
                new EOFToken(Position(1, 2))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Assignment()
        {
            var actual = GetTokens("=");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.Assignment, Position(1, 1)),
                new EOLToken(Position(1, 2)),
                new EOFToken(Position(1, 2))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Mutation()
        {
            var actual = GetTokens("=>");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.Mutation, Position(1, 1)),
                new EOLToken(Position(1, 3)),
                new EOFToken(Position(1, 3))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Return()
        {
            var actual = GetTokens("~");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.Return, Position(1, 1)),
                new EOLToken(Position(1, 2)),
                new EOFToken(Position(1, 2))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EqualTo()
        {
            var actual = GetTokens("==");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.EqualTo, Position(1, 1)),
                new EOLToken(Position(1, 3)),
                new EOFToken(Position(1, 3))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UnequalTo()
        {
            var actual = GetTokens("!=");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.UnequalTo, Position(1, 1)),
                new EOLToken(Position(1, 3)),
                new EOFToken(Position(1, 3))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GreaterThanOrEqualTo()
        {
            var actual = GetTokens(">=");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.GreaterThanOrEqualTo, Position(1, 1)),
                new EOLToken(Position(1, 3)),
                new EOFToken(Position(1, 3))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LessThanOrEqualTo()
        {
            var actual = GetTokens("<=");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.LessThanOrEqualTo, Position(1, 1)),
                new EOLToken(Position(1, 3)),
                new EOFToken(Position(1, 3))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Modulo()
        {
            var actual = GetTokens("%");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.Modulo, Position(1, 1)),
                new EOLToken(Position(1, 2)),
                new EOFToken(Position(1, 2))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Add()
        {
            var actual = GetTokens("+");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.Add, Position(1, 1)),
                new EOLToken(Position(1, 2)),
                new EOFToken(Position(1, 2))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Subtract()
        {
            var actual = GetTokens("-");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.Subtract, Position(1, 1)),
                new EOLToken(Position(1, 2)),
                new EOFToken(Position(1, 2))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Multiply()
        {
            var actual = GetTokens("*");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.Multiply, Position(1, 1)),
                new EOLToken(Position(1, 2)),
                new EOFToken(Position(1, 2))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Divide()
        {
            var actual = GetTokens("/");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.Divide, Position(1, 1)),
                new EOLToken(Position(1, 2)),
                new EOFToken(Position(1, 2))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Comma()
        {
            var actual = GetTokens(",");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.Comma, Position(1, 1)),
                new EOLToken(Position(1, 2)),
                new EOFToken(Position(1, 2))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void OpenParenthesis()
        {
            var actual = GetTokens("(");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.OpenParenthesis, Position(1, 1)),
                new EOLToken(Position(1, 2)),
                new EOFToken(Position(1, 2))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CloseParenthesis()
        {
            var actual = GetTokens(")");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.CloseParenthesis, Position(1, 1)),
                new EOLToken(Position(1, 2)),
                new EOFToken(Position(1, 2))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Exclamation()
        {
            var actual = GetTokens("!");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.Exclamation, Position(1, 1)),
                new EOLToken(Position(1, 2)),
                new EOFToken(Position(1, 2))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }
        #endregion

        #region Multiple
        [TestMethod]
        public void Function()
        {
            var text = @"
Main<>
    x = In<>
end";

            var actual = GetTokens(text);

            var expected = new List<Token>
            {
                new IdentifierToken("Main", Position(2, 1)),
                new SymbolToken(SymbolType.OpenAngleParenthesis, Position(2, 5)),
                new SymbolToken(SymbolType.CloseAngleParenthesis, Position(2, 6)),
                new EOLToken(Position(2, 7)),
                new IdentifierToken("x", Position(3, 5)),
                new SymbolToken(SymbolType.Assignment, Position(3, 7)),
                new IdentifierToken("In", Position(3, 9)),
                new SymbolToken(SymbolType.OpenAngleParenthesis, Position(3, 11)),
                new SymbolToken(SymbolType.CloseAngleParenthesis, Position(3, 12)),
                new EOLToken(Position(3, 13)),
                new KeywordToken(KeywordType.End, Position(4, 1)),
                new EOLToken(Position(4, 4)),
                new EOFToken(Position(4, 4))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Function2()
        {
            var text = @"
@ Safely divide two numbers
SafeDivide<top, bottom>
    if bottom == 0
        ~ @ Return NaN
    end
    
    ~top / bottom @ Return the result of division
end";

            var actual = GetTokens(text);

            var expected = new List<Token>
            {
                new IdentifierToken("SafeDivide", Position(3, 1)),
                new SymbolToken(SymbolType.OpenAngleParenthesis, Position(3, 11)),
                new IdentifierToken("top", Position(3, 12)),
                new SymbolToken(SymbolType.Comma, Position(3, 15)),
                new IdentifierToken("bottom", Position(3, 17)),
                new SymbolToken(SymbolType.CloseAngleParenthesis, Position(3, 23)),
                new EOLToken(Position(3, 24)),
                new KeywordToken(KeywordType.If, Position(4, 5)),
                new IdentifierToken("bottom", Position(4, 8)),
                new SymbolToken(SymbolType.EqualTo, Position(4, 15)),
                new NumberToken(0, Position(4, 18)),
                new EOLToken(Position(4, 19)),
                new SymbolToken(SymbolType.Return, Position(5, 9)),
                new EOLToken(Position(5, 11)),
                new KeywordToken(KeywordType.End, Position(6, 5)),
                new EOLToken(Position(6, 8)),
                new SymbolToken(SymbolType.Return, Position(8, 5)),
                new IdentifierToken("top", Position(8, 6)),
                new SymbolToken(SymbolType.Divide, Position(8, 10)),
                new IdentifierToken("bottom", Position(8, 12)),
                new EOLToken(Position(8, 19)),
                new KeywordToken(KeywordType.End, Position(9, 1)),
                new EOLToken(Position(9, 4)),
                new EOFToken(Position(9, 4))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Function3()
        {
            var text = @"
@ This is a commented line of code
x = 3 @ And this is a comment after the code on the same line
";

            var actual = GetTokens(text);

            var expected = new List<Token>
            {
                new IdentifierToken("x", Position(3, 1)),
                new SymbolToken(SymbolType.Assignment, Position(3, 3)),
                new NumberToken(3, Position(3, 5)),
                new EOLToken(Position(3, 7)),
                new EOFToken(Position(4, 1)),
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LineContinuation()
        {
            var text = @"
xSquared = x * ; @ This is a line continuation
x
";

            var actual = GetTokens(text);

            var expected = new List<Token>
            {
                new IdentifierToken("xSquared", Position(2, 1)),
                new SymbolToken(SymbolType.Assignment, Position(2, 10)),
                new IdentifierToken("x", Position(2, 12)),
                new SymbolToken(SymbolType.Multiply, Position(2, 14)),
                new IdentifierToken("x", Position(3, 1)),
                new EOLToken(Position(3, 2)),
                new EOFToken(Position(4, 1))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }
        #endregion

        #region Invalid
        [TestMethod]
        public void InvalidCharacter()
        {
            AssertUtility.ThrowsJellyException(() => GetTokens("["),
                "'[' is an invalid character at (test, 1, 1)");
        }

        [TestMethod]
        public void InvalidCharacter2()
        {
            AssertUtility.ThrowsJellyException(() => GetTokens("'"),
                "''' is an invalid character at (test, 1, 1)");
        }

        [TestMethod]
        public void InvalidCharacter3()
        {
            AssertUtility.ThrowsJellyException(() => GetTokens("#"),
                "'#' is an invalid character at (test, 1, 1)");
        }

        [TestMethod]
        public void InvalidLineContinuation()
        {
            var text = @"
asd = vefg + ; 8
      9";

            AssertUtility.ThrowsJellyException(() => GetTokens(text),
                "Only a comment is allowed after a line continuation at (test, 2, 16)");
        }
        #endregion
    }
}
