using Jelly.Core.Linking;
using Jelly.Core.Parsing;
using Jelly.Core.Utility;
using Jelly.Core.Verifying;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jelly.Tests
{
    [TestClass]
    public class VerifierTests
    {
        public void Verify(string text)
        {
            var tokens = new Lexer().Lex(text, "test");
            var ast = new Parser().Parse(tokens);
            var linkedAst = new Linker().LinkAST(ast);
            Verifier.Verify(linkedAst);
        }

        #region Valid
        [TestMethod]
        public void UsingVariable()
        {
            var text = @"
Main<>
    x = 3
    Write<x>
end";

            Verify(text);
        }

        [TestMethod]
        public void MultipleDifferentVariables()
        {
            var text = @"
Main<>
    x = 3
    y = 50
end";

            Verify(text);
        }

        [TestMethod]
        public void CallingFunction()
        {
            var text = @"
Main<>
    DoThing<>
end

DoThing<>
    @ Do the thing
end";

            Verify(text);
        }

        [TestMethod]
        public void MutatingVariable()
        {
            var text = @"
Main<>
    x = 3
    x => 4
end";

            Verify(text);
        }

        [TestMethod]
        public void UsingVariableInIfScope()
        {
            var text = @"
Main<>
    x = 3

    if x
        x => 4
    end

    Write<x>
end";

            Verify(text);
        }
        #endregion

        #region Invalid
        [TestMethod]
        [ExpectedException(typeof(JellyException))]
        public void MultipleNamedFunctions()
        {
            var text = @"
Main<>
    
end

Main<>

end";

            Verify(text);
        }

        [TestMethod]
        [ExpectedException(typeof(JellyException))]
        public void MultipleAssignments()
        {
            var text = @"
Main<>
    x = 3
    x = 50
end";

            Verify(text);
        }

        [TestMethod]
        [ExpectedException(typeof(JellyException))]
        public void MultipleParametersSameName()
        {
            var text = @"
Main<>
    
end

Do<a, b, a>
    
end";

            Verify(text);
        }

        [TestMethod]
        [ExpectedException(typeof(JellyException))]
        public void UsingUndefinedVariable()
        {
            var text = @"
Main<>
    Write<x>
end";

            Verify(text);
        }

        [TestMethod]
        [ExpectedException(typeof(JellyException))]
        public void UsingUndefinedVariable2()
        {
            var text = @"
Main<>
    x = |y|
end";

            Verify(text);
        }

        [TestMethod]
        [ExpectedException(typeof(JellyException))]
        public void UsingUndefinedVariable3()
        {
            var text = @"
Main<>
    x = 4
    x => 6 + ((12 / 3) - y)
end";

            Verify(text);
        }

        [TestMethod]
        [ExpectedException(typeof(JellyException))]
        public void CallingUndefinedFunction()
        {
            var text = @"
Main<>
    Blarg<>
end";

            Verify(text);
        }

        [TestMethod]
        [ExpectedException(typeof(JellyException))]
        public void MutatingUndefinedVariable()
        {
            var text = @"
Main<>
    x => 4
end";

            Verify(text);
        }

        [TestMethod]
        [ExpectedException(typeof(JellyException))]
        public void CallingFunctionWithIncorrectParameters()
        {
            var text = @"
Main<>
    Pow<2, 3, 4>
end";

            Verify(text);
        }

        [TestMethod]
        [ExpectedException(typeof(JellyException))]
        public void ReturnUndefinedVariable()
        {
            var text = @"
Main<>
    ~x
end";

            Verify(text);
        }

        [TestMethod]
        [ExpectedException(typeof(JellyException))]
        public void NoMainFunction()
        {
            var text = @"
Yhduwhd<>
    
end";

            Verify(text);
        }

        [TestMethod]
        [ExpectedException(typeof(JellyException))]
        public void MainWithParameters()
        {
            var text = @"
Main<x>
    
end";

            Verify(text);
        }

        [TestMethod]
        [ExpectedException(typeof(JellyException))]
        public void UsingIfVariableOutsideScope()
        {
            var text = @"
Main<>
    if 1
        x = 4
    end

    x => 3
end";

            Verify(text);
        }

        [TestMethod]
        [ExpectedException(typeof(JellyException))]
        public void DeclaringVariableAgainInIfScope()
        {
            var text = @"
Main<>
    x = 5

    if 1
        x = 4
    end
end";

            Verify(text);
        }

        [TestMethod]
        [ExpectedException(typeof(JellyException))]
        public void UndefinedIfCondition()
        {
            var text = @"
Main<>
    if x
        
    end
end";

            Verify(text);
        }

        [TestMethod]
        [ExpectedException(typeof(JellyException))]
        public void UsingVariableOutOfLoop()
        {
            var text = @"
Main<>
    loop 1

    end

    ~x
end";

            Verify(text);
        }
        #endregion
    }
}
