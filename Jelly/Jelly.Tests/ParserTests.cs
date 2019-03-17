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
        public void EmptyFunction()
        {
/*
Main<>
    
end
*/

            var tokens = new List<Token>
            {
                new IdentifierToken("Main", Position(1, 1)),
                new SymbolToken(SymbolType.OpenAngleParenthesis, Position(1, 5)),
                new SymbolToken(SymbolType.CloseAngleParenthesis, Position(1, 6)),
                new EOLToken(Position(1, 7)),
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
                    new List<IdentifierNode>
                    {

                    },
                    new List<IConstructNode>
                    {
                        
                    },
                    Position(1, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FunctionWithReturnNoValue()
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
                    new List<IdentifierNode>
                    {

                    },
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
        public void FunctionWithReturnValue()
        {
/*
Main<>
    ~-4
end
*/

            var tokens = new List<Token>
            {
                new IdentifierToken("Main", Position(1, 1)),
                new SymbolToken(SymbolType.OpenAngleParenthesis, Position(1, 5)),
                new SymbolToken(SymbolType.CloseAngleParenthesis, Position(1, 6)),
                new EOLToken(Position(1, 7)),

                new SymbolToken(SymbolType.Return, Position(2, 5)),
                new SymbolToken(SymbolType.Subtract, Position(2, 6)),
                new NumberToken(4, Position(2, 7)),
                new EOLToken(Position(2, 8)),

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
                    new List<IdentifierNode>
                    {

                    },
                    new List<IConstructNode>
                    {
                        new ReturnNode
                        (
                            new NumberNode
                            (
                                -4,
                                Position(2, 6)
                            ),
                            Position(2, 5)
                        )
                    },
                    Position(1, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FunctionWithParenthesisedReturnValue()
        {
/*
Main<>
    ~(num)
end
*/

            var tokens = new List<Token>
            {
                new IdentifierToken("Main", Position(1, 1)),
                new SymbolToken(SymbolType.OpenAngleParenthesis, Position(1, 5)),
                new SymbolToken(SymbolType.CloseAngleParenthesis, Position(1, 6)),
                new EOLToken(Position(1, 7)),

                new SymbolToken(SymbolType.Return, Position(2, 5)),
                new SymbolToken(SymbolType.OpenParenthesis, Position(2, 6)),
                new IdentifierToken("num", Position(2, 7)),
                new SymbolToken(SymbolType.CloseParenthesis, Position(2, 10)),
                new EOLToken(Position(2, 11)),

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
                    new List<IdentifierNode>
                    {

                    },
                    new List<IConstructNode>
                    {
                        new ReturnNode
                        (
                            new IdentifierNode
                            (
                                "num",
                                Position(2, 7)
                            ),
                            Position(2, 5)
                        )
                    },
                    Position(1, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EmptyFunctionWithSingleParameter()
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
                    new List<IdentifierNode>
                    {
                        new IdentifierNode
                        (
                            "x",
                            Position(1, 6)
                        )
                    },
                    new List<IConstructNode>
                    {
                        
                    },
                    Position(1, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EmptyFunctionWithMultipleParameters()
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
                    new IdentifierNode("Test", Position(1, 1)),
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
                    new List<IConstructNode>
                    {

                    },
                    Position(1, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FunctionWithAssignmentAndMutation()
        {
/*
Main<>
    x = -4.3
    x => 3
end
*/

            var tokens = new List<Token>
            {
                new IdentifierToken("Main", Position(1, 1)),
                new SymbolToken(SymbolType.OpenAngleParenthesis, Position(1, 5)),
                new SymbolToken(SymbolType.CloseAngleParenthesis, Position(1, 6)),
                new EOLToken(Position(1, 7)),

                new IdentifierToken("x", Position(2, 5)),
                new SymbolToken(SymbolType.Assignment, Position(2, 7)),
                new SymbolToken(SymbolType.Subtract, Position(2, 9)),
                new NumberToken(4.3, Position(2, 10)),
                new EOLToken(Position(2, 13)),

                new IdentifierToken("x", Position(3, 5)),
                new SymbolToken(SymbolType.Mutation, Position(3, 7)),
                new NumberToken(3, Position(3, 10)),
                new EOLToken(Position(3, 11)),

                new KeywordToken(KeywordType.End, Position(4, 1)),
                new EOLToken(Position(4, 4)),
                new EOFToken(Position(4, 4)),
            };

            var actual = GetAST(tokens);

            var expected = new List<FunctionNode>
            {
                new FunctionNode
                (
                    new IdentifierNode("Main", Position(1, 1)),
                    new List<IdentifierNode>
                    {

                    },
                    new List<IConstructNode>
                    {
                        new AssignmentNode
                        (
                            new IdentifierNode
                            (
                                "x",
                                Position(2, 5)
                            ),
                            new NumberNode
                            (
                                -4.3,
                                Position(2, 9)
                            ),
                            Position(2, 5)
                        ),
                        new MutationNode
                        (
                            new IdentifierNode
                            (
                                "x",
                                Position(3, 5)
                            ),
                            new NumberNode
                            (
                                3,
                                Position(3, 10)
                            ),
                            Position(3, 5)
                        )
                    },
                    Position(1, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MultipleEmptyFunctions()
        {
/*
Main<>
    
end

Test<>
    
end
*/

            var tokens = new List<Token>
            {
                new IdentifierToken("Main", Position(1, 1)),
                new SymbolToken(SymbolType.OpenAngleParenthesis, Position(1, 5)),
                new SymbolToken(SymbolType.CloseAngleParenthesis, Position(1, 6)),
                new EOLToken(Position(1, 7)),
                new KeywordToken(KeywordType.End, Position(3, 1)),
                new EOLToken(Position(3, 4)),
                new IdentifierToken("Test", Position(5, 1)),
                new SymbolToken(SymbolType.OpenAngleParenthesis, Position(5, 5)),
                new SymbolToken(SymbolType.CloseAngleParenthesis, Position(5, 6)),
                new EOLToken(Position(5, 7)),
                new KeywordToken(KeywordType.End, Position(7, 1)),
                new EOLToken(Position(7, 4)),
                new EOFToken(Position(7, 4)),
            };

            var actual = GetAST(tokens);

            var expected = new List<FunctionNode>
            {
                new FunctionNode
                (
                    new IdentifierNode("Main", Position(1, 1)),
                    new List<IdentifierNode>
                    {

                    },
                    new List<IConstructNode>
                    {
                        
                    },
                    Position(1, 1)
                ),
                new FunctionNode
                (
                    new IdentifierNode("Test", Position(5, 1)),
                    new List<IdentifierNode>
                    {

                    },
                    new List<IConstructNode>
                    {

                    },
                    Position(5, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EmptyIf()
        {
/*
Main<>
    if 1

    end
end
*/

            var tokens = new List<Token>
            {
                new IdentifierToken("Main", Position(1, 1)),
                new SymbolToken(SymbolType.OpenAngleParenthesis, Position(1, 5)),
                new SymbolToken(SymbolType.CloseAngleParenthesis, Position(1, 6)),
                new EOLToken(Position(1, 7)),

                new KeywordToken(KeywordType.If, Position(2, 5)),
                new NumberToken(1, Position(2, 8)),
                new EOLToken(Position(2, 9)),

                new KeywordToken(KeywordType.End, Position(4, 5)),
                new EOLToken(Position(4, 8)),

                new KeywordToken(KeywordType.End, Position(5, 1)),
                new EOLToken(Position(5, 4)),
                new EOFToken(Position(5, 4)),
            };

            var actual = GetAST(tokens);

            var expected = new List<FunctionNode>
            {
                new FunctionNode
                (
                    new IdentifierNode("Main", Position(1, 1)),
                    new List<IdentifierNode>
                    {

                    },
                    new List<IConstructNode>
                    {
                        new IfBlockNode
                        (
                            new List<ConditionalBlockNode>
                            {
                                new ConditionalBlockNode
                                (
                                    new NumberNode
                                    (
                                        1,
                                        Position(2, 8)
                                    ),
                                    new List<IConstructNode>
                                    {

                                    },
                                    Position(2, 8)
                                )
                            },
                            Position(2, 5)
                        )
                    },
                    Position(1, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EmptyIfElse()
        {
/*
Main<>
    if 1

    end
    else

    end
end
*/

            var tokens = new List<Token>
            {
                new IdentifierToken("Main", Position(1, 1)),
                new SymbolToken(SymbolType.OpenAngleParenthesis, Position(1, 5)),
                new SymbolToken(SymbolType.CloseAngleParenthesis, Position(1, 6)),
                new EOLToken(Position(1, 7)),

                new KeywordToken(KeywordType.If, Position(2, 5)),
                new NumberToken(1, Position(2, 8)),
                new EOLToken(Position(2, 9)),

                new KeywordToken(KeywordType.End, Position(4, 5)),
                new EOLToken(Position(4, 8)),

                new KeywordToken(KeywordType.Else, Position(5, 5)),
                new EOLToken(Position(5, 9)),

                new KeywordToken(KeywordType.End, Position(7, 5)),
                new EOLToken(Position(7, 8)),

                new KeywordToken(KeywordType.End, Position(8, 1)),
                new EOLToken(Position(8, 4)),
                new EOFToken(Position(8, 4)),
            };

            var actual = GetAST(tokens);

            var expected = new List<FunctionNode>
            {
                new FunctionNode
                (
                    new IdentifierNode("Main", Position(1, 1)),
                    new List<IdentifierNode>
                    {

                    },
                    new List<IConstructNode>
                    {
                        new IfBlockNode
                        (
                            new List<ConditionalBlockNode>
                            {
                                new ConditionalBlockNode
                                (
                                    new NumberNode
                                    (
                                        1,
                                        Position(2, 8)
                                    ),
                                    new List<IConstructNode>
                                    {

                                    },
                                    Position(2, 8)
                                ),
                                new ConditionalBlockNode
                                (
                                    null,
                                    new List<IConstructNode>
                                    {

                                    },
                                    Position(5, 5)
                                )
                            },
                            Position(2, 5)
                        )
                    },
                    Position(1, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void ReturnOperation()
        {
/*
Double<x>
    ~(x * 2)
end
*/

            var tokens = new List<Token>
            {
                new IdentifierToken("Double", Position(1, 1)),
                new SymbolToken(SymbolType.OpenAngleParenthesis, Position(1, 7)),
                new IdentifierToken("x", Position(1, 8)),
                new SymbolToken(SymbolType.CloseAngleParenthesis, Position(1, 9)),
                new EOLToken(Position(1, 10)),

                new SymbolToken(SymbolType.Return, Position(2, 5)),
                new SymbolToken(SymbolType.OpenParenthesis, Position(2, 6)),
                new IdentifierToken("x", Position(2, 7)),
                new SymbolToken(SymbolType.Multiply, Position(2, 9)),
                new NumberToken(2, Position(2, 11)),
                new SymbolToken(SymbolType.CloseParenthesis, Position(2, 12)),
                new EOLToken(Position(2, 13)),

                new KeywordToken(KeywordType.End, Position(3, 1)),
                new EOLToken(Position(3, 4)),
                new EOFToken(Position(3, 4)),
            };

            var actual = GetAST(tokens);

            var expected = new List<FunctionNode>
            {
                new FunctionNode
                (
                    new IdentifierNode("Double", Position(1, 1)),
                    new List<IdentifierNode>
                    {
                        new IdentifierNode
                        (
                            "x",
                            Position(1, 8)
                        )
                    },
                    new List<IConstructNode>
                    {
                        new ReturnNode
                        (
                            new OperationNode
                            (
                                new IdentifierNode
                                (
                                    "x",
                                    Position(2, 7)
                                ),
                                OperatorType.Multiply,
                                new NumberNode
                                (
                                    2,
                                    Position(2, 11)
                                ),
                                Position(2, 7)
                            ),
                            Position(2, 5)
                        )
                    },
                    Position(1, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }
        #endregion

        #region Complex
        [TestMethod]
        public void MutateWithOperation()
        {
/*
Main<>
    x = -1
    x => 5 - 3
end
*/

            var tokens = new List<Token>
            {
                new IdentifierToken("Main", Position(1, 1)),
                new SymbolToken(SymbolType.OpenAngleParenthesis, Position(1, 5)),
                new SymbolToken(SymbolType.CloseAngleParenthesis, Position(1, 6)),
                new EOLToken(Position(1, 7)),
                
                new IdentifierToken("x", Position(2, 5)),
                new SymbolToken(SymbolType.Assignment, Position(2, 7)),
                new SymbolToken(SymbolType.Subtract, Position(2, 9)),
                new NumberToken(1, Position(2, 10)),
                new EOLToken(Position(2, 11)),

                new IdentifierToken("x", Position(3, 5)),
                new SymbolToken(SymbolType.Mutation, Position(3, 7)),
                new NumberToken(5, Position(3, 10)),
                new SymbolToken(SymbolType.Subtract, Position(3, 12)),
                new NumberToken(3, Position(3, 14)),
                new EOLToken(Position(3, 15)),

                new KeywordToken(KeywordType.End, Position(4, 1)),
                new EOLToken(Position(4, 4)),
                new EOFToken(Position(4, 4)),
            };

            var actual = GetAST(tokens);

            var expected = new List<FunctionNode>
            {
                new FunctionNode
                (
                    new IdentifierNode("Main", Position(1, 1)),
                    new List<IdentifierNode>
                    {

                    },
                    new List<IConstructNode>
                    {
                        new AssignmentNode
                        (
                            new IdentifierNode
                            (
                                "x",
                                Position(2, 5)
                            ),
                            new NumberNode
                            (
                                -1,
                                Position(2, 9)
                            ),
                            Position(2, 5)
                        ),
                        new MutationNode
                        (
                            new IdentifierNode
                            (
                                "x",
                                Position(3, 5)
                            ),
                            new OperationNode
                            (
                                new NumberNode
                                (
                                    5,
                                    Position(3, 10)
                                ),
                                OperatorType.Subtract,
                                new NumberNode
                                (
                                    3,
                                    Position(3, 14)
                                ),
                                Position(3, 10)
                            ),
                            Position(3, 5)
                        )
                    },
                    Position(1, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IfElifElseWithStatements()
        {
/*
Main<>
    if 0
        x = 1
        ~x
    end
    elif 1 + (4 * 5)
        ~0
    end
    else
        ~2 / 3
    end
end
*/

            var tokens = new List<Token>
            {
                new IdentifierToken("Main", Position(1, 1)),
                new SymbolToken(SymbolType.OpenAngleParenthesis, Position(1, 5)),
                new SymbolToken(SymbolType.CloseAngleParenthesis, Position(1, 6)),
                new EOLToken(Position(1, 7)),

                new KeywordToken(KeywordType.If, Position(2, 5)),
                new NumberToken(0, Position(2, 8)),
                new EOLToken(Position(2, 9)),

                new IdentifierToken("x", Position(3, 9)),
                new SymbolToken(SymbolType.Assignment, Position(3, 11)),
                new NumberToken(1, Position(3, 13)),
                new EOLToken(Position(3, 14)),

                new SymbolToken(SymbolType.Return, Position(4, 9)),
                new IdentifierToken("x", Position(4, 10)),
                new EOLToken(Position(4, 11)),

                new KeywordToken(KeywordType.End, Position(5, 5)),
                new EOLToken(Position(5, 8)),

                new KeywordToken(KeywordType.Elif, Position(6, 5)),
                new NumberToken(1, Position(6, 10)),
                new SymbolToken(SymbolType.Add, Position(6, 12)),
                new SymbolToken(SymbolType.OpenParenthesis, Position(6, 14)),
                new NumberToken(4, Position(6, 15)),
                new SymbolToken(SymbolType.Multiply, Position(6, 17)),
                new NumberToken(5, Position(6, 19)),
                new SymbolToken(SymbolType.CloseParenthesis, Position(6, 20)),
                new EOLToken(Position(6, 21)),

                new SymbolToken(SymbolType.Return, Position(7, 9)),
                new NumberToken(0, Position(7, 10)),
                new EOLToken(Position(7, 11)),

                new KeywordToken(KeywordType.End, Position(8, 5)),
                new EOLToken(Position(8, 8)),

                new KeywordToken(KeywordType.Else, Position(9, 5)),
                new EOLToken(Position(9, 9)),

                new SymbolToken(SymbolType.Return, Position(10, 9)),
                new NumberToken(2, Position(10, 10)),
                new SymbolToken(SymbolType.Divide, Position(10, 12)),
                new NumberToken(3, Position(10, 14)),
                new EOLToken(Position(10, 15)),

                new KeywordToken(KeywordType.End, Position(11, 5)),
                new EOLToken(Position(1, 8)),

                new KeywordToken(KeywordType.End, Position(12, 1)),
                new EOLToken(Position(12, 4)),
                new EOFToken(Position(12, 4)),
            };

            var actual = GetAST(tokens);

            var expected = new List<FunctionNode>
            {
                new FunctionNode
                (
                    new IdentifierNode("Main", Position(1, 1)),
                    new List<IdentifierNode>
                    {

                    },
                    new List<IConstructNode>
                    {
                        new IfBlockNode
                        (
                            new List<ConditionalBlockNode>
                            {
                                new ConditionalBlockNode
                                (
                                    new NumberNode
                                    (
                                        0,
                                        Position(2, 8)
                                    ),
                                    new List<IConstructNode>
                                    {
                                        new AssignmentNode
                                        (
                                            new IdentifierNode
                                            (
                                                "x",
                                                Position(3, 9)
                                            ),
                                            new NumberNode
                                            (
                                                1,
                                                Position(3, 13)
                                            ),
                                            Position(3, 9)
                                        ),
                                        new ReturnNode
                                        (
                                            new IdentifierNode
                                            (
                                                "x",
                                                Position(4, 10)
                                            ),
                                            Position(4, 9)
                                        )
                                    },
                                    Position(2, 8)
                                ),
                                new ConditionalBlockNode
                                (
                                    new OperationNode
                                    (
                                        new NumberNode
                                        (
                                            1,
                                            Position(6, 10)
                                        ),
                                        OperatorType.Add,
                                        new OperationNode
                                        (
                                            new NumberNode
                                            (
                                                4,
                                                Position(6, 15)
                                            ),
                                            OperatorType.Multiply,
                                            new NumberNode
                                            (
                                                5,
                                                Position(6, 19)
                                            ),
                                            Position(6, 15)
                                        ),
                                        Position(6, 10)
                                    ),
                                    new List<IConstructNode>
                                    {
                                        new ReturnNode
                                        (
                                            new NumberNode
                                            (
                                                0,
                                                Position(7, 10)
                                            ),
                                            Position(7, 9)
                                        )
                                    },
                                    Position(6, 10)
                                ),
                                new ConditionalBlockNode
                                (
                                    null,
                                    new List<IConstructNode>
                                    {
                                        new ReturnNode
                                        (
                                            new OperationNode
                                            (
                                                new NumberNode
                                                (
                                                    2,
                                                    Position(10, 10)
                                                ),
                                                OperatorType.Divide,
                                                new NumberNode
                                                (
                                                    3,
                                                    Position(10, 14)
                                                ),
                                                Position(10, 10)
                                            ),
                                            Position(10, 9)
                                        )
                                    },
                                    Position(9, 5)
                                )
                            },
                            Position(2, 5)
                        )
                    },
                    Position(1, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MultipleFunctionsWithCall()
        {
/*
Main<>
    Get<3>
end

Get<x>
    ~x
end
*/

            var tokens = new List<Token>
            {
                new IdentifierToken("Main", Position(1, 1)),
                new SymbolToken(SymbolType.OpenAngleParenthesis, Position(1, 5)),
                new SymbolToken(SymbolType.CloseAngleParenthesis, Position(1, 6)),
                new EOLToken(Position(1, 7)),

                new IdentifierToken("Get", Position(2, 5)),
                new SymbolToken(SymbolType.OpenAngleParenthesis, Position(2, 8)),
                new NumberToken(3, Position(2, 9)),
                new SymbolToken(SymbolType.CloseAngleParenthesis, Position(2, 10)),
                new EOLToken(Position(2, 11)),

                new KeywordToken(KeywordType.End, Position(3, 1)),
                new EOLToken(Position(3, 4)),

                new IdentifierToken("Get", Position(5, 1)),
                new SymbolToken(SymbolType.OpenAngleParenthesis, Position(5, 4)),
                new IdentifierToken("x", Position(5, 5)),
                new SymbolToken(SymbolType.CloseAngleParenthesis, Position(5, 6)),
                new EOLToken(Position(5, 7)),

                new SymbolToken(SymbolType.Return, Position(6, 5)),
                new IdentifierToken("x", Position(6, 6)),
                new EOLToken(Position(6, 7)),

                new KeywordToken(KeywordType.End, Position(7, 1)),
                new EOLToken(Position(7, 4)),
                new EOFToken(Position(7, 4)),
            };

            var actual = GetAST(tokens);

            var expected = new List<FunctionNode>
            {
                new FunctionNode
                (
                    new IdentifierNode("Main", Position(1, 1)),
                    new List<IdentifierNode>
                    {

                    },
                    new List<IConstructNode>
                    {
                        new CallNode
                        (
                            new IdentifierNode
                            (
                                "Get",
                                Position(2, 5)
                            ),
                            new List<IValueNode>
                            {
                                new NumberNode
                                (
                                    3,
                                    Position(2, 9)
                                )
                            },
                            Position(2, 5)
                        )
                    },
                    Position(1, 1)
                ),
                new FunctionNode
                (
                    new IdentifierNode("Get", Position(5, 1)),
                    new List<IdentifierNode>
                    {
                        new IdentifierNode
                        (
                            "x",
                            Position(5, 5)
                        )
                    },
                    new List<IConstructNode>
                    {
                        new ReturnNode
                        (
                            new IdentifierNode
                            (
                                "x",
                                Position(6, 6)
                            ),
                            Position(6, 5)
                        )
                    },
                    Position(5, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LongOperation()
        {
/*
Main<>
    x = 1 * 4 + 6 / (-4 - -6)
end
*/

            var tokens = new List<Token>
            {
                new IdentifierToken("Main", Position(1, 1)),
                new SymbolToken(SymbolType.OpenAngleParenthesis, Position(1, 5)),
                new SymbolToken(SymbolType.CloseAngleParenthesis, Position(1, 6)),
                new EOLToken(Position(1, 7)),

                new IdentifierToken("x", Position(2, 5)),
                new SymbolToken(SymbolType.Assignment, Position(2, 7)),
                new NumberToken(1, Position(2, 9)),
                new SymbolToken(SymbolType.Multiply, Position(2, 11)),
                new NumberToken(4, Position(2, 13)),
                new SymbolToken(SymbolType.Add, Position(2, 15)),
                new NumberToken(6, Position(2, 17)),
                new SymbolToken(SymbolType.Divide, Position(2, 19)),
                new SymbolToken(SymbolType.OpenParenthesis, Position(2, 21)),
                new SymbolToken(SymbolType.Subtract, Position(2, 22)),
                new NumberToken(4, Position(2, 23)),
                new SymbolToken(SymbolType.Subtract, Position(2, 25)),
                new SymbolToken(SymbolType.Subtract, Position(2, 27)),
                new NumberToken(6, Position(2, 28)),
                new SymbolToken(SymbolType.CloseParenthesis, Position(2, 29)),
                new EOLToken(Position(2, 30)),

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
                    new List<IdentifierNode>
                    {

                    },
                    new List<IConstructNode>
                    {
                        new AssignmentNode
                        (
                            new IdentifierNode
                            (
                                "x",
                                Position(2, 5)
                            ),
                            new OperationNode
                            (
                                new OperationNode
                                (
                                    new OperationNode
                                    (
                                        new NumberNode
                                        (
                                            1,
                                            Position(2, 9)
                                        ),
                                        OperatorType.Multiply,
                                        new NumberNode
                                        (
                                            4,
                                            Position(2, 13)
                                        ),
                                        Position(2, 9)
                                    ),
                                    OperatorType.Add,
                                    new NumberNode
                                    (
                                        6,
                                        Position(2, 17)
                                    ),
                                    Position(2, 15)
                                ),
                                OperatorType.Divide,
                                new OperationNode
                                (
                                    new NumberNode
                                    (
                                        -4,
                                        Position(2, 22)
                                    ),
                                    OperatorType.Subtract,
                                    new NumberNode
                                    (
                                        -6,
                                        Position(2, 27)
                                    ),
                                    Position(2, 22)
                                ),
                                Position(2, 19)
                            ),
                            Position(2, 5)
                        )
                    },
                    Position(1, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LongOperation2()
        {
/*
Main<>
    x = (5 + 1) * (6 / (8 + 2))
end
*/

            var tokens = new List<Token>
            {
                new IdentifierToken("Main", Position(1, 1)),
                new SymbolToken(SymbolType.OpenAngleParenthesis, Position(1, 5)),
                new SymbolToken(SymbolType.CloseAngleParenthesis, Position(1, 6)),
                new EOLToken(Position(1, 7)),

                new IdentifierToken("x", Position(2, 5)),
                new SymbolToken(SymbolType.Assignment, Position(2, 7)),
                new SymbolToken(SymbolType.OpenParenthesis, Position(2, 9)),
                new NumberToken(5, Position(2, 10)),
                new SymbolToken(SymbolType.Add, Position(2, 12)),
                new NumberToken(1, Position(2, 14)),
                new SymbolToken(SymbolType.CloseParenthesis, Position(2, 15)),
                new SymbolToken(SymbolType.Multiply, Position(2, 17)),
                new SymbolToken(SymbolType.OpenParenthesis, Position(2, 19)),
                new NumberToken(6, Position(2, 20)),
                new SymbolToken(SymbolType.Divide, Position(2, 22)),
                new SymbolToken(SymbolType.OpenParenthesis, Position(2, 24)),
                new NumberToken(8, Position(2, 25)),
                new SymbolToken(SymbolType.Add, Position(2, 27)),
                new NumberToken(2, Position(2, 29)),
                new SymbolToken(SymbolType.CloseParenthesis, Position(2, 30)),
                new SymbolToken(SymbolType.CloseParenthesis, Position(2, 31)),
                new EOLToken(Position(2, 32)),

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
                    new List<IdentifierNode>
                    {

                    },
                    new List<IConstructNode>
                    {
                        new AssignmentNode
                        (
                            new IdentifierNode
                            (
                                "x",
                                Position(2, 5)
                            ),
                            new OperationNode
                            (
                                new OperationNode
                                (
                                    new NumberNode
                                    (
                                        5,
                                        Position(2, 10)
                                    ),
                                    OperatorType.Add,
                                    new NumberNode
                                    (
                                        1,
                                        Position(2, 14)
                                    ),
                                    Position(2, 10)
                                ),
                                OperatorType.Multiply,
                                new OperationNode
                                (
                                    new NumberNode
                                    (
                                        6,
                                        Position(2, 20)
                                    ),
                                    OperatorType.Divide,
                                    new OperationNode
                                    (
                                        new NumberNode
                                        (
                                            8,
                                            Position(2, 25)
                                        ),
                                        OperatorType.Add,
                                        new NumberNode
                                        (
                                            2,
                                            Position(2, 29)
                                        ),
                                        Position(2, 25)
                                    ),
                                    Position(2, 20)
                                ),
                                Position(2, 9)
                            ),
                            Position(2, 5)
                        )
                    },
                    Position(1, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }
        #endregion
    }
}
