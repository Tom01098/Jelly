using Jelly.Core.Parsing.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Jelly.Tests
{
    public static class CollectionAssertUtility
    {
        public static void AreEqual(List<Token> expected, List<Token> actual)
        {
            if (expected.Count != actual.Count)
            {
                throw new AssertFailedException("Different number of elements");
            }

            for (int i = 0; i < expected.Count; i++)
            {
                AssertUtility.AreEqual(expected[i], actual[i]);
            }
        }
    }
}
