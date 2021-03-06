﻿using Jelly.Core.Parsing;
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
                    new IdentifierNode[]
                    {

                    },
                    new IConstructNode[]
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
                    new IdentifierNode[]
                    {

                    },
                    new IConstructNode[]
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
                    new IdentifierNode[]
                    {

                    },
                    new IConstructNode[]
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
                    new IdentifierNode[]
                    {

                    },
                    new IConstructNode[]
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
                    new IdentifierNode[]
                    {

                    },
                    new IConstructNode[]
                    {
                        new ReturnNode
                        (
                            new NumberNode
                            (
                                4,
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
        public void FunctionWithAbsoluteNumber()
        {
            var text = @"
Main<>
    ~|5|
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
                    new IdentifierNode[]
                    {

                    },
                    new IConstructNode[]
                    {
                        new ReturnNode
                        (
                            new AbsoluteNode
                            (
                                new NumberNode
                                (
                                    5,
                                    Position(3, 7)
                                ),
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
                    new IdentifierNode[]
                    {
                        new IdentifierNode
                        (
                            "num",
                            Position(2, 6)
                        )
                    },
                    new IConstructNode[]
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
                    new IdentifierNode[]
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
                    new IConstructNode[]
                    {

                    },
                    Position(2, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FunctionWithEmptyCall()
        {
            var text = @"
Main<>
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
                    new IdentifierNode[]
                    {

                    },
                    new IConstructNode[]
                    {
                        new CallNode
                        (
                            new IdentifierNode
                            (
                                "Test",
                                Position(3, 5)
                            ),
                            new ITermNode[]
                            {

                            },
                            Position(3, 5)
                        )
                    },
                    Position(2, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FunctionWithCallWithArguments()
        {
            var text = @"
Main<>
    Test<x, y>
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
                    new IdentifierNode[]
                    {

                    },
                    new IConstructNode[]
                    {
                        new CallNode
                        (
                            new IdentifierNode
                            (
                                "Test",
                                Position(3, 5)
                            ),
                            new ITermNode[]
                            {
                                new IdentifierNode
                                (
                                    "x",
                                    Position(3, 10)
                                ),
                                new IdentifierNode
                                (
                                    "y",
                                    Position(3, 13)
                                ),
                            },
                            Position(3, 5)
                        )
                    },
                    Position(2, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FunctionWithAssignmentToNegativeNumber()
        {
            var text = @"
Main<>
    x = -3
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
                    new IdentifierNode[]
                    {

                    },
                    new IConstructNode[]
                    {
                        new AssignmentNode
                        (
                            new IdentifierNode
                            (
                                "x",
                                Position(3, 5)
                            ),
                            new NegativeNode
                            (
                                new NumberNode
                                (
                                    3,
                                    Position(3, 10)
                                ),
                                Position(3, 9)
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
        public void FunctionWithMutationToNotNumber()
        {
            var text = @"
Main<>
    x => !0
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
                    new IdentifierNode[]
                    {

                    },
                    new IConstructNode[]
                    {
                        new MutationNode
                        (
                            new IdentifierNode
                            (
                                "x",
                                Position(3, 5)
                            ),
                            new NotNode
                            (
                                new NumberNode
                                (
                                    0,
                                    Position(3, 11)
                                ),
                                Position(3, 10)
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
        public void AssignmentWithOperation()
        {
            var text = @"
Main<>
    sum = 3 % 4
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
                    new IdentifierNode[]
                    {

                    },
                    new IConstructNode[]
                    {
                        new AssignmentNode
                        (
                            new IdentifierNode
                            (
                                "sum",
                                Position(3, 5)
                            ),
                            new ValueNode
                            (
                                new NumberNode
                                (
                                    3,
                                    Position(3, 11)
                                ),
                                OperatorType.Modulo,
                                new NumberNode
                                (
                                    4,
                                    Position(3, 15)
                                ),
                                Position(3, 11)
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
        public void AssignmentWithParenthesisedOperation()
        {
            var text = @"
Main<>
    sum = (3 * 4)
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
                    new IdentifierNode[]
                    {

                    },
                    new IConstructNode[]
                    {
                        new AssignmentNode
                        (
                            new IdentifierNode
                            (
                                "sum",
                                Position(3, 5)
                            ),
                            new ValueNode
                            (
                                new NumberNode
                                (
                                    3,
                                    Position(3, 12)
                                ),
                                OperatorType.Multiply,
                                new NumberNode
                                (
                                    4,
                                    Position(3, 16)
                                ),
                                Position(3, 12)
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
        public void AssignmentWithCallNoArguments()
        {
           var text = @"
Main<>
    result = Test<>
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
                    new IdentifierNode[]
                    {

                    },
                    new IConstructNode[]
                    {
                        new AssignmentNode
                        (
                            new IdentifierNode
                            (
                                "result",
                                Position(3, 5)
                            ),
                            new CallNode
                            (
                                new IdentifierNode
                                (
                                    "Test",
                                    Position(3, 14)
                                ),
                                new ITermNode[]
                                {

                                },
                                Position(3, 14)
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
        public void AssignmentWithCallWithArguments()
        {
            var text = @"
Main<>
    result = Test<3, a>
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
                    new IdentifierNode[]
                    {

                    },
                    new IConstructNode[]
                    {
                        new AssignmentNode
                        (
                            new IdentifierNode
                            (
                                "result",
                                Position(3, 5)
                            ),
                            new CallNode
                            (
                                new IdentifierNode
                                (
                                    "Test",
                                    Position(3, 14)
                                ),
                                new ITermNode[]
                                {
                                    new NumberNode
                                    (
                                        3,
                                        Position(3, 19)
                                    ),
                                    new IdentifierNode
                                    (
                                        "a",
                                        Position(3, 22)
                                    ),
                                },
                                Position(3, 14)
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
        public void EmptyIf()
        {
            var text = @"
Main<>
    if 1

    end
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
                                    new NumberNode
                                    (
                                        1,
                                        Position(3, 8)
                                    ),
                                    new IConstructNode[]
                                    {

                                    },
                                    Position(3, 8)
                                )
                            },
                            Position(3, 5)
                        )
                    },
                    Position(2, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EmptyIfElse()
        {
            var text = @"
Main<>
    if 1

    end
    else

    end
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
                                    new NumberNode
                                    (
                                        1,
                                        Position(3, 8)
                                    ),
                                    new IConstructNode[]
                                    {

                                    },
                                    Position(3, 8)
                                ),
                                new ConditionalBlockNode
                                (
                                    null,
                                    new IConstructNode[]
                                    {

                                    },
                                    Position(6, 9)
                                )
                            },
                            Position(3, 5)
                        )
                    },
                    Position(2, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EmptyLoop()
        {
            var text = @"
Main<>
    loop 1

    end
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
                    new IdentifierNode[]
                    {

                    },
                    new IConstructNode[]
                    {
                        new LoopBlockNode
                        (
                            new ConditionalBlockNode
                            (
                                new NumberNode
                                (
                                    1,
                                    Position(3, 10)
                                ),
                                new IConstructNode[]
                                {

                                },
                                Position(3, 10)
                            ),
                            Position(3, 5)
                        )
                    },
                    Position(2, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }
        #endregion

        #region Complex
        [TestMethod]
        public void IfElifOperations()
        {
            var text = @"
Main<>
    if x >> 5
        ~x
    end
    elif x << (2 * (5 / 4))
        ~
    end
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
                                            Position(3, 13)
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
                                    Position(3, 8)
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
                                        OperatorType.LessThan,
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
                                                Position(6, 21)
                                            ),
                                            Position(6, 16)
                                        ),
                                        Position(6, 10)
                                    ),
                                    new IConstructNode[]
                                    {
                                        new ReturnNode
                                        (
                                            null,
                                            Position(7, 9)
                                        )
                                    },
                                    Position(6, 10)
                                )
                            },
                            Position(3, 5)
                        )
                    },
                    Position(2, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LongOperation()
        {
            var text = @"
Main<>
    ~4 + 5 / 2
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

            CollectionAssertUtility.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CallWithGreaterThan()
        {
            var text = @"
Main<>
    Call<x >> 1, 2>
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
                    new IdentifierNode[]
                    {

                    },
                    new IConstructNode[]
                    {
                        new CallNode
                        (
                            new IdentifierNode
                            (
                                "Call",
                                Position(3, 5)
                            ),
                            new ITermNode[]
                            {
                                new ValueNode
                                (
                                    new IdentifierNode
                                    (
                                        "x",
                                        Position(3, 10)
                                    ),
                                    OperatorType.GreaterThan,
                                    new NumberNode
                                    (
                                        1,
                                        Position(3, 15)
                                    ),
                                    Position(3, 10)
                                ),
                                new NumberNode
                                (
                                    2,
                                    Position(3, 18)
                                ),
                            },
                            Position(3, 5)
                        )
                    },
                    Position(2, 1)
                )
            };

            CollectionAssertUtility.AreEqual(expected, actual);
        }
        #endregion
    }
}
