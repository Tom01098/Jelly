using Jelly.Core.Parsing.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jelly.Tests
{
    public static class AssertUtility
    {
        public static void AreEqual(Token expected, Token actual)
        {
            var eType = expected.GetType();
            var aType = actual.GetType();

            if (eType != aType)
            {
                throw new AssertFailedException("Different types");
            }
            else if (expected.Position.Line != actual.Position.Line || expected.Position.Character != actual.Position.Character)
            {
                throw new AssertFailedException("Different positions");
            }
            else if (expected is IdentifierToken)
            {
                if (((IdentifierToken)expected).Identifier != ((IdentifierToken)actual).Identifier)
                {
                    throw new AssertFailedException("Different identifiers");
                }
            }
            else if (expected is NumberToken)
            {
                if (((NumberToken)expected).Number != ((NumberToken)actual).Number)
                {
                    throw new AssertFailedException("Different numbers");
                }
            }
            else if (expected is KeywordToken)
            {
                if (((KeywordToken)expected).Keyword != ((KeywordToken)actual).Keyword)
                {
                    throw new AssertFailedException("Different keywords");
                }
            }
            else if (expected is SymbolToken)
            {
                if (((SymbolToken)expected).Symbol != ((SymbolToken)actual).Symbol)
                {
                    throw new AssertFailedException("Different symbols");
                }
            }
        }
    }
}
