using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Jelly.Core.Parsing;
using Jelly.Core.Parsing.Tokens;
using Jelly.Core.Utility;

namespace Jelly.Tests
{
    [TestClass]
    public class LexerTests
    {
        public List<Token> GetTokens(string text) =>
            new Lexer().Lex(text);

        #region Single
        [TestMethod]
        public void Identifier()
        {
            var actual = GetTokens("abc");

            var expected = new List<Token>
            {
                new IdentifierToken("abc", new Position(1, 1)),
                new EOFToken(new Position(1, 4))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Number()
        {
            var actual = GetTokens("1");

            var expected = new List<Token>
            {
                new NumberToken(1, new Position(1, 1)),
                new EOFToken(new Position(1, 2))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DecimalNumber()
        {
            var actual = GetTokens("3.5");

            var expected = new List<Token>
            {
                new NumberToken(3.5, new Position(1, 1)),
                new EOFToken(new Position(1, 4))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void End()
        {
            var actual = GetTokens("end");

            var expected = new List<Token>
            {
                new KeywordToken(KeywordType.End, new Position(1, 1)),
                new EOFToken(new Position(1, 4))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void If()
        {
            var actual = GetTokens("if");

            var expected = new List<Token>
            {
                new KeywordToken(KeywordType.If, new Position(1, 1)),
                new EOFToken(new Position(1, 3))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Elif()
        {
            var actual = GetTokens("elif");

            var expected = new List<Token>
            {
                new KeywordToken(KeywordType.Elif, new Position(1, 1)),
                new EOFToken(new Position(1, 5))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Else()
        {
            var actual = GetTokens("else");

            var expected = new List<Token>
            {
                new KeywordToken(KeywordType.Else, new Position(1, 1)),
                new EOFToken(new Position(1, 5))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void OpenAngleParenthesis()
        {
            var actual = GetTokens("<");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.OpenAngleParenthesis, new Position(1, 1)),
                new EOFToken(new Position(1, 2))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CloseAngleParenthesis()
        {
            var actual = GetTokens(">");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.CloseAngleParenthesis, new Position(1, 1)),
                new EOFToken(new Position(1, 2))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Assignment()
        {
            var actual = GetTokens("=");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.Assignment, new Position(1, 1)),
                new EOFToken(new Position(1, 2))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Mutation()
        {
            var actual = GetTokens("<=");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.Mutation, new Position(1, 1)),
                new EOFToken(new Position(1, 3))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Return()
        {
            var actual = GetTokens("~");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.Return, new Position(1, 1)),
                new EOFToken(new Position(1, 2))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EqualTo()
        {
            var actual = GetTokens("==");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.EqualTo, new Position(1, 1)),
                new EOFToken(new Position(1, 3))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UnequalTo()
        {
            var actual = GetTokens("!=");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.UnequalTo, new Position(1, 1)),
                new EOFToken(new Position(1, 3))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GreaterThanOrEqualTo()
        {
            var actual = GetTokens(">=");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.GreaterThanOrEqualTo, new Position(1, 1)),
                new EOFToken(new Position(1, 3))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LessThanOrEqualTo()
        {
            var actual = GetTokens("<=");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.LessThanOrEqualTo, new Position(1, 1)),
                new EOFToken(new Position(1, 3))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Modulo()
        {
            var actual = GetTokens("%");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.Modulo, new Position(1, 1)),
                new EOFToken(new Position(1, 2))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Add()
        {
            var actual = GetTokens("+");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.Add, new Position(1, 1)),
                new EOFToken(new Position(1, 2))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Subtract()
        {
            var actual = GetTokens("-");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.Subtract, new Position(1, 1)),
                new EOFToken(new Position(1, 2))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Multiply()
        {
            var actual = GetTokens("*");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.Multiply, new Position(1, 1)),
                new EOFToken(new Position(1, 2))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Divide()
        {
            var actual = GetTokens("/");

            var expected = new List<Token>
            {
                new SymbolToken(SymbolType.Divide, new Position(1, 1)),
                new EOFToken(new Position(1, 2))
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
                new IdentifierToken("Main", new Position(2, 1)),
                new SymbolToken(SymbolType.OpenAngleParenthesis, new Position(2, 5)),
                new SymbolToken(SymbolType.CloseAngleParenthesis, new Position(2, 6)),
                new EOSToken(new Position(2, 7)),
                new IdentifierToken("x", new Position(3, 5)),
                new SymbolToken(SymbolType.Assignment, new Position(3, 7)),
                new IdentifierToken("In", new Position(3, 9)),
                new SymbolToken(SymbolType.OpenAngleParenthesis, new Position(3, 11)),
                new SymbolToken(SymbolType.CloseAngleParenthesis, new Position(3, 12)),
                new EOSToken(new Position(3, 13)),
                new KeywordToken(KeywordType.End, new Position(4, 1)),
                new EOFToken(new Position(4, 4))
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }
        #endregion
    }
}
