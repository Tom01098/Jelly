using Jelly.Core.Parsing.AST;
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

            CollectionAssertUtility.AreEqual(a, a);
        }

        [TestMethod]
        public void Equal2()
        {
            var a = new List<FunctionNode>
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

            CollectionAssertUtility.AreEqual(a, a);
        }

        [TestMethod]
        public void Equal3()
        {
            var a = new List<FunctionNode>
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

            CollectionAssertUtility.AreEqual(a, a);
        }

        [TestMethod]
        public void Equals4()
        {
            var a = new List<FunctionNode>
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
                                    Position(2, 5)
                                )
                            },
                            Position(2, 5)
                        )
                    },
                    Position(1, 1)
                )
            };

            CollectionAssertUtility.AreEqual(a, a);
        }

        [TestMethod]
        public void Equal5()
        {
            var a = new List<FunctionNode>
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
                                    Position(2, 5)
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

            var b = new List<FunctionNode>
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

            var b = new List<FunctionNode>
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
                                OperatorType.Add,
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

            CollectionAssertUtility.AreEqual(a, b);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void Unequal3()
        {
            var a = new List<FunctionNode>
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

            var b = new List<FunctionNode>
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
                                Position(1, 23123)
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

            CollectionAssertUtility.AreEqual(a, b);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void Unequals4()
        {
            var a = new List<FunctionNode>
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
                                    Position(2, 5)
                                )
                            },
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
                                    Position(2, 5)
                                )
                            },
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
        public void Unequal5()
        {
            var a = new List<FunctionNode>
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
                                    Position(2, 5)
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

            var b = new List<FunctionNode>
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
                                    Position(2, 5)
                                ),
                                new ConditionalBlockNode
                                (
                                    new NumberNode
                                    (
                                        6,
                                        Position(1, 1)
                                    ),
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

            CollectionAssertUtility.AreEqual(a, b);
        }
        #endregion
    }
}
