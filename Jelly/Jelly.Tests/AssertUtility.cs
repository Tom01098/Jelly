using Jelly.Core.Parsing.AST;
using Jelly.Core.Parsing.Tokens;
using Jelly.Core.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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

// Disable this message because it's very annoying and keeps trying to
// pattern match but it can't work because of the same variable name in the 
// enclosing scope.
#pragma warning disable IDE0020
        public static void AreEqual(Node expected, Node actual)
        {
            // Null check
            if (expected == null ^ actual == null)
            {
                throw new AssertFailedException("One node was null");
            }

            // Types are equal, so recursively check its members
            if (expected.GetType() == actual.GetType())
            {
                

                // Check position
                if (expected.Position.Line != actual.Position.Line || expected.Position.Character != actual.Position.Character)
                {
                    throw new AssertFailedException("Different positions");
                }
            }
            // Types are not equal
            else
            {
                throw new AssertFailedException("Different node types");
            }
        }
#pragma warning restore IDE0020

        public static void ThrowsJellyException(Action action, string message)
        {
            try
            {
                action();
            }
            catch (JellyException e)
            {
                if (e.Message != message)
                {
                    throw new AssertFailedException("Different messages");
                }

                return;
            }

            throw new AssertFailedException("Did not throw JellyException");
        }
    }
}
