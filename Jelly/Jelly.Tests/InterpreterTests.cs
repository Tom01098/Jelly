using Jelly.Core.Interpreting;
using Jelly.Core.Linking;
using Jelly.Core.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Jelly.Tests
{
    [TestClass]
    public class InterpreterTests
    {
        private void Interpret(string text, string expected)
        {
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);

                new Interpreter().Interpret(new Linker().LinkAST(new Parser().Parse(new Lexer().Lex(text, "test"))));

                Console.OpenStandardOutput();
                Assert.AreEqual(expected, writer.ToString());
            }
        }

        #region Single Function
        [TestMethod]
        public void Number()
        {
            var text = @"
Main<>
    WriteLine<1>
end";

            var expected = @"1
";

            Interpret(text, expected);
        }

        [TestMethod]
        public void MultipleNumbers()
        {
            var text = @"
Main<>
    WriteLine<1>
    WriteLine<-2>
    WriteLine<-71.5>
    WriteLine<4.09>
end";

            var expected = @"1
-2
-71.5
4.09
";

            Interpret(text, expected);
        }

        [TestMethod]
        public void Identifier()
        {
            var text = @"
Main<>
    x = 5
    WriteLine<x>
end";

            var expected = @"5
";

            Interpret(text, expected);
        }

        [TestMethod]
        public void MutatedIdentifier()
        {
            var text = @"
Main<>
    x = 5
    WriteLine<x>
    x => 7.4
    WriteLine<x>
end";

            var expected = @"5
7.4
";

            Interpret(text, expected);
        }

        [TestMethod]
        public void Addition()
        {
            var text = @"
Main<>
    WriteLine<5 + 6>
end";

            var expected = @"11
";

            Interpret(text, expected);
        }

        [TestMethod]
        public void Subtraction()
        {
            var text = @"
Main<>
    WriteLine<5 - 6>
end";

            var expected = @"-1
";

            Interpret(text, expected);
        }

        [TestMethod]
        public void Multiplication()
        {
            var text = @"
Main<>
    WriteLine<5 * 6>
end";

            var expected = @"30
";

            Interpret(text, expected);
        }

        [TestMethod]
        public void Division()
        {
            var text = @"
Main<>
    WriteLine<6 / 2>
end";

            var expected = @"3
";

            Interpret(text, expected);
        }

        [TestMethod]
        public void Modulo()
        {
            var text = @"
Main<>
    WriteLine<5 % 3>
end";

            var expected = @"2
";

            Interpret(text, expected);
        }

        [TestMethod]
        public void EqualTo()
        {
            var text = @"
Main<>
    WriteLine<6 == 2>
end";

            var expected = @"0
";

            Interpret(text, expected);
        }

        [TestMethod]
        public void UnequalTo()
        {
            var text = @"
Main<>
    WriteLine<6 == 2>
end";

            var expected = @"0
";

            Interpret(text, expected);
        }

        [TestMethod]
        public void LessThan()
        {
            var text = @"
Main<>
    WriteLine<6 << 2>
end";

            var expected = @"0
";

            Interpret(text, expected);
        }

        [TestMethod]
        public void GreaterThan()
        {
            var text = @"
Main<>
    WriteLine<6 >> 2>
end";

            var expected = @"1
";

            Interpret(text, expected);
        }

        [TestMethod]
        public void LessThanOrEqualTo()
        {
            var text = @"
Main<>
    WriteLine<6 <= 2>
end";

            var expected = @"0
";

            Interpret(text, expected);
        }

        [TestMethod]
        public void GreaterThanOrEqualTo()
        {
            var text = @"
Main<>
    WriteLine<6 >= 2>
end";

            var expected = @"1
";

            Interpret(text, expected);
        }

        [TestMethod]
        public void Not()
        {
            var text = @"
Main<>
    WriteLine<!0>
end";

            var expected = @"1
";

            Interpret(text, expected);
        }

        [TestMethod]
        public void Arithmetic()
        {
            var text = @"
Main<>
    WriteLine<3 * ((5 + 7) / 2)>
end";

            var expected = @"18
";

            Interpret(text, expected);
        }

        [TestMethod]
        public void Arithmetic2()
        {
            var text = @"
Main<>
    WriteLine<4 + 6 * 7>
end";

            var expected = @"46
";

            Interpret(text, expected);
        }

        [TestMethod]
        public void If()
        {
            var text = @"
Main<>
    if 1
        WriteLine<7>
    end
end";

            var expected = @"7
";

            Interpret(text, expected);
        }

        [TestMethod]
        public void IfElif()
        {
            var text = @"
Main<>
    if 0
        WriteLine<7>
    end
    elif 1
        WriteLine<4>
    end
end";

            var expected = @"4
";

            Interpret(text, expected);
        }

        [TestMethod]
        public void NestedIf()
        {
            var text = @"
Main<>
    x = 3
    WriteLine<x>    

    if 1
        if !1
            x => 5
        end
        else
            x => 6
            WriteLine<x>
        end

        WriteLine<x>
    end

    WriteLine<x>
end";

            var expected = @"3
6
6
6
";

            Interpret(text, expected);
        }
        #endregion

        #region Two Functions
        [TestMethod]
        public void Return()
        {
            var text = @"
Main<>
    WriteLine< Value<> >
end

Value<>
    ~7
end";

            var expected = @"7
";

            Interpret(text, expected);
        }

        [TestMethod]
        public void ReturnArgument()
        {
            var text = @"
Main<>
    WriteLine< Value<2> >
end

Value<x>
    ~x
end";

            var expected = @"2
";

            Interpret(text, expected);
        }

        [TestMethod]
        public void WriteInOtherFunction()
        {
            var text = @"
Main<>
    WriteThree<2, 6, -9>
end

WriteThree<x, y, z>
    WriteLine<x>
    WriteLine<y>
    WriteLine<z>
end";

            var expected = @"2
6
-9
";

            Interpret(text, expected);
        }
        #endregion
    }
}
