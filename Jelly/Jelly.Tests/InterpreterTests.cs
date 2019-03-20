using Jelly.Core.Interpreting;
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

                new Interpreter().Interpret(new Parser().Parse(new Lexer().Lex(text, "test")));

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
    Write<1>
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
    Write<1>
    Write<-2>
    Write<-71.5>
    Write<4.09>
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
    Write<x>
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
    Write<x>
    x => 7.4
    Write<x>
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
    Write<5 + 6>
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
    Write<5 - 6>
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
    Write<5 * 6>
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
    Write<6 / 2>
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
    Write<5 % 3>
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
    Write<6 == 2>
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
    Write<6 == 2>
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
    Write<6 << 2>
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
    Write<6 >> 2>
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
    Write<6 <= 2>
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
    Write<6 >= 2>
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
    Write<!0>
end";

            var expected = @"1
";

            Interpret(text, expected);
        }
        #endregion
    }
}
