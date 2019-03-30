using Jelly.Core.Parsing.AST;
using Jelly.Core.Parsing.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using static Jelly.Tests.TestUtility;

namespace Jelly.Tests
{
    [TestClass]
    public class CollectionAssertUtilityTests
    {
        #region Equality
        [TestMethod]
        public void Equal()
        {
            var a = new List<FunctionNode>
            {
                new FunctionNode
                (
                    new IdentifierNode("Main", Position(1, 1)),
                    new IdentifierNode[]
                    {

                    },
                    new IConstructNode[]
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

            CollectionAssertUtility.AreEqual(a, a);
        }

        [TestMethod]
        public void Equal2()
        {
            var a = new List<FunctionNode>
            {
                new FunctionNode
                (
                    new IdentifierNode
                    (
                        "Main",
                        Position(2, 1)
                    ),
                    new IdentifierNode[]
                    {

                    },
                    new IConstructNode[]
                    {
                        new IfBlockNode
                        (
                            new ConditionalBlockNode[]
                            {
                                new ConditionalBlockNode
                                (
                                    new ValueNode
                                    (
                                        new IdentifierNode
                                        (
                                            "x",
                                            Position(3, 8)
                                        ),
                                        OperatorType.GreaterThan,
                                        new NumberNode
                                        (
                                            5,
                                            Position(3, 12)
                                        ),
                                        Position(3, 8)
                                    ),
                                    new IConstructNode[]
                                    {
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
                                    Position(3, 5)
                                ),
                                new ConditionalBlockNode
                                (
                                    new ValueNode
                                    (
                                        new IdentifierNode
                                        (
                                            "x",
                                            Position(6, 10)
                                        ),
                                        OperatorType.LessThanOrEqualTo,
                                        new ValueNode
                                        (
                                            new NumberNode
                                            (
                                                2,
                                                Position(6, 16)
                                            ),
                                            OperatorType.Multiply,
                                            new ValueNode
                                            (
                                                new NumberNode
                                                (
                                                    5,
                                                    Position(6, 21)
                                                ),
                                                OperatorType.Divide,
                                                new NumberNode
                                                (
                                                    4,
                                                    Position(6, 25)
                                                ),
                                                Position(6, 20)
                                            ),
                                            Position(6, 15)
                                        ),
                                        Position(6, 15)
                                    ),
                                    new IConstructNode[]
                                    {
                                        new ReturnNode
                                        (
                                            null,
                                            Position(7, 9)
                                        )
                                    },
                                    Position(6, 5)
                                )
                            },
                            Position(3, 5)
                        )
                    },
                    Position(2, 1)
                )
            };

            CollectionAssertUtility.AreEqual(a, a);
        }
        #endregion

        #region Inequality
        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void Unequal()
        {
            var a = new List<FunctionNode>
            {
                new FunctionNode
                (
                    new IdentifierNode("Maix", Position(1, 1)),
                    new IdentifierNode[]
                    {

                    },
                    new IConstructNode[]
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

            var b = new List<FunctionNode>
            {
                new FunctionNode
                (
                    new IdentifierNode("Main", Position(1, 1)),
                    new IdentifierNode[]
                    {

                    },
                    new IConstructNode[]
                    {
                        new ReturnNode
                        (
                            new NumberNode
                            (
                                1,
                                Position(1, 2)
                            ),
                            Position(2, 5)
                        )
                    },
                    Position(1, 1)
                )
            };

            CollectionAssertUtility.AreEqual(a, b);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void Unequal2()
        {
            var a = new List<FunctionNode>
            {
                new FunctionNode
                (
                    new IdentifierNode
                    (
                        "Main",
                        Position(2, 1)
                    ),
                    new IdentifierNode[]
                    {

                    },
                    new IConstructNode[]
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
                                OperatorType.Add,
                                new ValueNode
                                (
                                    new NumberNode
                                    (
                                        5,
                                        Position(3, 10)
                                    ),
                                    OperatorType.Divide,
                                    new NumberNode
                                    (
                                        2,
                                        Position(3, 14)
                                    ),
                                    Position(3, 10)
                                ),
                                Position(3, 6)
                            ),
                            Position(3, 5)
                        )
                    },
                    Position(2, 1)
                )
            };

            var b = new List<FunctionNode>
            {
                new FunctionNode
                (
                    new IdentifierNode
                    (
                        "Main",
                        Position(2, 1)
                    ),
                    new IdentifierNode[]
                    {

                    },
                    new IConstructNode[]
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
                                OperatorType.Add,
                                new ValueNode
                                (
                                    new NumberNode
                                    (
                                        5,
                                        Position(3, 10)
                                    ),
                                    OperatorType.Divide,
                                    null,
                                    Position(3, 10)
                                ),
                                Position(3, 6)
                            ),
                            Position(3, 5)
                        )
                    },
                    Position(2, 1)
                )
            };

            CollectionAssertUtility.AreEqual(a, b);
        }
        #endregion
    }
}
