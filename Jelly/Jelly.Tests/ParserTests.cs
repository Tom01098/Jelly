using Jelly.Core.Parsing;
using Jelly.Core.Parsing.AST;
using Jelly.Core.Parsing.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using static Jelly.Tests.TestUtility;

namespace Jelly.Tests
{
    [TestClass]
    public class ParserTests
    {
        public List<FunctionNode> GetAST(List<Token> tokens) =>
            new Parser().Parse(tokens);

        #region Simple
        [TestMethod]
        public void FunctionWithReturn()
        {
/*
Main<>
    ~
end
*/

            var tokens = new List<Token>
            {
                new IdentifierToken("Main", Position(1, 1)),
                new SymbolToken(SymbolType.OpenAngleParenthesis, Position(1, 5)),
                new SymbolToken(SymbolType.CloseAngleParenthesis, Position(1, 6)),
                new EOLToken(Position(1, 7)),
                new SymbolToken(SymbolType.Return, Position(2, 5)),
                new EOLToken(Position(2, 6)),
                new KeywordToken(KeywordType.End, Position(3, 1)),
                new EOLToken(Position(3, 4)),
                new EOFToken(Position(3, 4)),
            };

            var actual = GetAST(tokens);

            var expected = new List<FunctionNode>
            {
                new FunctionNode
                (
                    new IdentifierNode("Main", Position(1, 1)),
                    new ParametersNode
                    (
                        new List<IdentifierNode>
                        {

                        },
                        Position(1, 5)
                    ),
                    new List<IConstructNode>
                    {
                        new ReturnNode
                        (
                            null,
                            Position(2, 5)
                        )
                    },
                    Position(1, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FunctionWithSingleParameter()
        {
/*
Test<x>
                
end
*/

            var tokens = new List<Token>
            {
                new IdentifierToken("Test", Position(1, 1)),
                new SymbolToken(SymbolType.OpenAngleParenthesis, Position(1, 5)),
                new IdentifierToken("x", Position(1, 6)),
                new SymbolToken(SymbolType.CloseAngleParenthesis, Position(1, 7)),
                new EOLToken(Position(1, 8)),
                new KeywordToken(KeywordType.End, Position(3, 1)),
                new EOLToken(Position(3, 4)),
                new EOFToken(Position(3, 4)),
            };

            var actual = GetAST(tokens);

            var expected = new List<FunctionNode>
            {
                new FunctionNode
                (
                    new IdentifierNode("Test", Position(1, 1)),
                    new ParametersNode
                    (
                        new List<IdentifierNode>
                        {
                            new IdentifierNode
                            (
                                "x",
                                Position(1, 6)
                            )
                        },
                        Position(1, 5)
                    ),
                    new List<IConstructNode>
                    {
                        
                    },
                    Position(1, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FunctionWithMultipleParameters()
        {
/*
Test<one, two, three>

end
*/

            var tokens = new List<Token>
            {
                new IdentifierToken("Test", Position(1, 1)),
                new SymbolToken(SymbolType.OpenAngleParenthesis, Position(1, 5)),
                new IdentifierToken("one", Position(1, 6)),
                new SymbolToken(SymbolType.Comma, Position(1, 9)),
                new IdentifierToken("two", Position(1, 11)),
                new SymbolToken(SymbolType.Comma, Position(1, 14)),
                new IdentifierToken("three", Position(1, 16)),
                new SymbolToken(SymbolType.CloseAngleParenthesis, Position(1, 21)),
                new EOLToken(Position(1, 22)),
                new KeywordToken(KeywordType.End, Position(3, 1)),
                new EOLToken(Position(3, 4)),
                new EOFToken(Position(3, 4)),
            };

            var actual = GetAST(tokens);

            var expected = new List<FunctionNode>
            {
                new FunctionNode
                (
                    new IdentifierNode("Main", Position(1, 1)),
                    new ParametersNode
                    (
                        new List<IdentifierNode>
                        {
                            new IdentifierNode
                            (
                                "one",
                                Position(1, 6)
                            ),
                            new IdentifierNode
                            (
                                "two",
                                Position(1, 11)
                            ),
                            new IdentifierNode
                            (
                                "three",
                                Position(1, 16)
                            )
                        },
                        Position(1, 5)
                    ),
                    new List<IConstructNode>
                    {

                    },
                    Position(1, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }
        #endregion
    }
}
