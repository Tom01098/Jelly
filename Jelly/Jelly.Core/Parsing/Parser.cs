using Jelly.Core.Parsing.AST;
using Jelly.Core.Parsing.Tokens;
using System;
using System.Collections.Generic;

namespace Jelly.Core.Parsing
{
    /// <summary>
    /// Parses an input list of tokens into a syntax tree.
    /// </summary>
    public class Parser
    {
        private TokenEnumerator tokens;

        public List<FunctionNode> Parse(List<Token> list)
        {
            tokens = new TokenEnumerator(list);
            var functions = new List<FunctionNode>();

            while (!(tokens.Current is EOFToken))
            {
                functions.Add(Function());
            }

            return functions;
        }

        private bool IsSymbol(Token token, SymbolType type)
        {
            return token is SymbolToken s && s.Symbol == type;
        }

        private bool IsKeyword(Token token, KeywordType type)
        {
            return token is KeywordToken k && k.Keyword == type;
        }

        private bool IsOperator(Token token)
        {
            return token is SymbolToken s && (
                s.Symbol == SymbolType.Add ||
                s.Symbol == SymbolType.Subtract ||
                s.Symbol == SymbolType.Multiply ||
                s.Symbol == SymbolType.Divide ||
                s.Symbol == SymbolType.Modulo ||
                s.Symbol == SymbolType.EqualTo ||
                s.Symbol == SymbolType.UnequalTo ||
                s.Symbol == SymbolType.OpenAngleParenthesis ||
                s.Symbol == SymbolType.CloseAngleParenthesis ||
                s.Symbol == SymbolType.LessThanOrEqualTo ||
                s.Symbol == SymbolType.GreaterThanOrEqualTo);
        }

        // assignment = identifier '=' value;
        private AssignmentNode Assignment()
        {
            throw new NotImplementedException();
        }

        // arguments = value {',' value};
        private List<ValueNode> Arguments()
        {
            throw new NotImplementedException();
        }

        // call = identifier '<' [arguments] '>';
        private CallNode Call()
        {
            throw new NotImplementedException();
        }

        // conditional_block = value EOL {construct} end;
        private ConditionalBlockNode ConditionalBlock()
        {
            throw new NotImplementedException();
        }

        // construct = if_block | statement EOL;
        private IConstructNode Construct()
        {
            throw new NotImplementedException();
        }

        // {construct}
        private List<IConstructNode> Constructs()
        {
            throw new NotImplementedException();
        }

        // function = signature {construct} end;
        // signature = identifier '<' [parameters] '>' EOL;
        private FunctionNode Function()
        {
            throw new NotImplementedException();
        }

        // identifier = ? IdentifierToken ?;
        private IdentifierNode Identifier()
        {
            throw new NotImplementedException();
        }

        // if_block = 'if' conditional_block {'elif' conditional_block} ['else' EOL {construct} end;
        private IfBlockNode IfBlock()
        {
            throw new NotImplementedException();
        }

        // mutation = identifier '=>' value;
        private MutationNode Mutation()
        {
            throw new NotImplementedException();
        }

        // negative = ['-'] value;
        private NegativeNode Negative()
        {
            throw new NotImplementedException();
        }

        // not = '!' value;
        private NotNode Not()
        {
            throw new NotImplementedException();
        }

        // number = ? NumberToken ?;
        private NumberNode Number()
        {
            throw new NotImplementedException();
        }

        // parameters = identifier {',' identifier};
        private List<IdentifierNode> Parameters()
        {
            throw new NotImplementedException();
        }

        // return = '~' [value];
        private ReturnNode Return()
        {
            throw new NotImplementedException();
        }

        // statement = return | assignment | mutation | call;
        private IStatementNode Statement()
        {
            throw new NotImplementedException();
        }

        // term = '(' value ')' | not | negative | call | number | identifier;
        private ITermNode Term()
        {
            throw new NotImplementedException();
        }

        // value = term {operator term};
        private ValueNode Value()
        {
            throw new NotImplementedException();
        }
    }
}
