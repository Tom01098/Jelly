using Jelly.Core.Parsing;
using Jelly.Core.Parsing.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using static Jelly.Tests.TestUtility;

namespace Jelly.Tests
{
    [TestClass]
    public class TokenEnumeratorTests
    {
        [TestMethod]
        public void Enumerate()
        {
            var tokens = new List<Token>
            {
                new IdentifierToken("x", Position(1, 1)),
                new SymbolToken(SymbolType.Assignment, Position(1, 2)),
                new NumberToken(4.2, Position(1, 3)),
                new EOLToken(Position(1, 6)),
                new EOFToken(Position(1, 6))
            };

            var enumerator = new TokenEnumerator(tokens);
            
            Assert.AreEqual(tokens[0], enumerator.Current);
            enumerator.MoveNext();
            Assert.AreEqual(tokens[1], enumerator.Current);
            enumerator.MoveNext();
            Assert.AreEqual(tokens[2], enumerator.Current);
            enumerator.MoveNext();
            Assert.AreEqual(tokens[3], enumerator.Current);
            enumerator.MoveNext();
            Assert.AreEqual(tokens[4], enumerator.Current);
            enumerator.MoveNext();
            Assert.AreEqual(null, enumerator.Current);
            enumerator.MoveNext();
        }

        [TestMethod]
        public void EnumerateWithLookAhead()
        {
            var tokens = new List<Token>
            {
                new IdentifierToken("x", Position(1, 1)),
                new SymbolToken(SymbolType.Assignment, Position(1, 2)),
                new NumberToken(4.2, Position(1, 3)),
                new EOLToken(Position(1, 6)),
                new EOFToken(Position(1, 6))
            };

            var enumerator = new TokenEnumerator(tokens);
            
            Assert.AreEqual(tokens[0], enumerator.Current);
            enumerator.MoveNext();
            Assert.AreEqual(tokens[3], enumerator.LookAhead(2));
            Assert.AreEqual(tokens[1], enumerator.Current);
            enumerator.MoveNext();
            Assert.AreEqual(tokens[2], enumerator.Current);
            enumerator.MoveNext();
            Assert.AreEqual(tokens[3], enumerator.Current);
            Assert.AreEqual(tokens[4], enumerator.LookAhead(1));
            Assert.AreEqual(null, enumerator.LookAhead(2));
            enumerator.MoveNext();
            Assert.AreEqual(tokens[4], enumerator.Current);
            enumerator.MoveNext();
            Assert.AreEqual(null, enumerator.Current);
            enumerator.MoveNext();
        }
    }
}
