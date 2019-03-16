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
                // AssignmentNode
                if (expected is AssignmentNode)
                {
                    var e = (AssignmentNode)expected;
                    var a = (AssignmentNode)actual;

                    AreEqual(e.Identifier, a.Identifier);
                    AreEqual((Node)e.Value, (Node)a.Value);
                }
                // CallNode
                else if (expected is CallNode)
                {
                    var e = (CallNode)expected;
                    var a = (CallNode)actual;

                    AreEqual(e.Identifier, a.Identifier);

                    if (e.Arguments.Count != a.Arguments.Count)
                    {
                        throw new AssertFailedException("Different number of arguments");
                    }

                    for (int i = 0; i < e.Arguments.Count; i++)
                    {
                        AreEqual((Node)e.Arguments[i], (Node)a.Arguments[i]);
                    }
                }
                // ConditionalBlockNode
                else if (expected is ConditionalBlockNode)
                {
                    var e = (ConditionalBlockNode)expected;
                    var a = (ConditionalBlockNode)actual;

                    if (!((a.Condition == null || e.Condition == null) && (a.Condition == e.Condition)))
                    {
                        AreEqual((Node)e.Condition, (Node)a.Condition);
                    }

                    if (e.Constructs.Count != a.Constructs.Count)
                    {
                        throw new AssertFailedException("Different number of constructs");
                    }

                    for (int i = 0; i < e.Constructs.Count; i++)
                    {
                        AreEqual((Node)e.Constructs[i], (Node)a.Constructs[i]);
                    }
                }
                // FunctionNode
                else if (expected is FunctionNode)
                {
                    var e = (FunctionNode)expected;
                    var a = (FunctionNode)actual;

                    AreEqual(e.Identifier, a.Identifier);

                    if (e.Parameters.Count != a.Parameters.Count)
                    {
                        throw new AssertFailedException("Different number of parameters");
                    }

                    for (int i = 0; i < e.Parameters.Count; i++)
                    {
                        AreEqual(e.Parameters[i], a.Parameters[i]);
                    }

                    if (e.Constructs.Count != a.Constructs.Count)
                    {
                        throw new AssertFailedException("Different number of constructs");
                    }

                    for (int i = 0; i < e.Constructs.Count; i++)
                    {
                        AreEqual((Node)e.Constructs[i], (Node)a.Constructs[i]);
                    }
                }
                // IdentifierNode
                else if (expected is IdentifierNode)
                {
                    var e = (IdentifierNode)expected;
                    var a = (IdentifierNode)actual;

                    Assert.AreEqual(e.Identifier, a.Identifier);
                }
                // IfBlockNode
                else if (expected is IfBlockNode)
                {
                    var e = (IfBlockNode)expected;
                    var a = (IfBlockNode)actual;

                    if (e.ConditionalBlocks.Count != a.ConditionalBlocks.Count)
                    {
                        throw new AssertFailedException("Different number of constructs");
                    }

                    for (int i = 0; i < e.ConditionalBlocks.Count; i++)
                    {
                        AreEqual(e.ConditionalBlocks[i], a.ConditionalBlocks[i]);
                    }
                }
                // MutationNode
                else if (expected is MutationNode)
                {
                    var e = (MutationNode)expected;
                    var a = (MutationNode)actual;

                    AreEqual(e.Identifier, a.Identifier);
                    AreEqual((Node)e.Value, (Node)a.Value);
                }
                // NumberNode
                else if (expected is NumberNode)
                {
                    var e = (NumberNode)expected;
                    var a = (NumberNode)actual;

                    Assert.AreEqual(e.Number, a.Number);
                }
                // OperationNode
                else if (expected is OperationNode)
                {
                    var e = (OperationNode)expected;
                    var a = (OperationNode)actual;

                    AreEqual((Node)e.LHS, (Node)a.LHS);
                    Assert.AreEqual(e.Operator, a.Operator);
                    AreEqual((Node)e.RHS, (Node)a.RHS);
                }
                // ReturnNode
                else if (expected is ReturnNode)
                {
                    var e = (ReturnNode)expected;
                    var a = (ReturnNode)actual;

                    if (!((a.Value == null || e.Value == null) && (a.Value == e.Value)))
                    {
                        AreEqual((Node)e.Value, (Node)a.Value);
                    }
                }
                // Node is not of a valid node type
                else
                {
                    throw new AssertFailedException($"Unknown node type {expected.GetType()}");
                }

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
