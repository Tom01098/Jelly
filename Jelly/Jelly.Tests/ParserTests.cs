using Jelly.Core.Parsing;
using Jelly.Core.Parsing.AST;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using static Jelly.Tests.TestUtility;

namespace Jelly.Tests
{
    [TestClass]
    public class ParserTests
    {
        public List<FunctionNode> GetAST(string text) =>
            new Parser().Parse(new Lexer().Lex(text, "test"));

        #region Simple
        [TestMethod]
        public void EmptyFunction()
        {
            var text = @"
Main<>

end";

            var actual = GetAST(text);

            var expected = new List<FunctionNode>
            {
                new FunctionNode
                (
                    new IdentifierNode
                    (
                        "Main",
                        Position(2, 1)
                    ),
                    new List<IdentifierNode>
                    {

                    },
                    new List<IConstructNode>
                    {

                    },
                    Position(2, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MultipleEmptyFunctions()
        {
            var text = @"
Main<>

end

Test<>

end";

            var actual = GetAST(text);

            var expected = new List<FunctionNode>
            {
                new FunctionNode
                (
                    new IdentifierNode
                    (
                        "Main",
                        Position(2, 1)
                    ),
                    new List<IdentifierNode>
                    {

                    },
                    new List<IConstructNode>
                    {

                    },
                    Position(2, 1)
                ),
                new FunctionNode
                (
                    new IdentifierNode
                    (
                        "Test",
                        Position(6, 1)
                    ),
                    new List<IdentifierNode>
                    {

                    },
                    new List<IConstructNode>
                    {

                    },
                    Position(6, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FunctionWithReturnNoValue()
        {
            var text = @"
Main<>
    ~
end";

            var actual = GetAST(text);

            var expected = new List<FunctionNode>
            {
                new FunctionNode
                (
                    new IdentifierNode
                    (
                        "Main",
                        Position(2, 1)
                    ),
                    new List<IdentifierNode>
                    {

                    },
                    new List<IConstructNode>
                    {
                        new ReturnNode
                        (
                            null,
                            Position(3, 5)
                        )
                    },
                    Position(2, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FunctionWithReturnNumber()
        {
            var text = @"
Main<>
    ~4
end";

            var actual = GetAST(text);

            var expected = new List<FunctionNode>
            {
                new FunctionNode
                (
                    new IdentifierNode
                    (
                        "Main",
                        Position(2, 1)
                    ),
                    new List<IdentifierNode>
                    {

                    },
                    new List<IConstructNode>
                    {
                        new ReturnNode
                        (
                            new ValueNode
                            (
                                new NumberNode
                                (
                                    4,
                                    Position(3, 6)
                                ),
                                OperatorType.None,
                                null,
                                Position(3, 6)
                            ),
                            Position(3, 5)
                        )
                    },
                    Position(2, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EmptyFunctionWithParameter()
        {
            var text = @"
Main<num>

end";

            var actual = GetAST(text);

            var expected = new List<FunctionNode>
            {
                new FunctionNode
                (
                    new IdentifierNode
                    (
                        "Main",
                        Position(2, 1)
                    ),
                    new List<IdentifierNode>
                    {
                        new IdentifierNode
                        (
                            "num",
                            Position(2, 6)
                        )
                    },
                    new List<IConstructNode>
                    {

                    },
                    Position(2, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EmptyFunctionWithParameters()
        {
            var text = @"
Main<one, two, three>

end";

            var actual = GetAST(text);

            var expected = new List<FunctionNode>
            {
                new FunctionNode
                (
                    new IdentifierNode
                    (
                        "Main",
                        Position(2, 1)
                    ),
                    new List<IdentifierNode>
                    {
                        new IdentifierNode
                        (
                            "one",
                            Position(2, 6)
                        ),
                        new IdentifierNode
                        (
                            "two",
                            Position(2, 11)
                        ),
                        new IdentifierNode
                        (
                            "three",
                            Position(2, 16)
                        )
                    },
                    new List<IConstructNode>
                    {

                    },
                    Position(2, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }
        #endregion
    }
}
