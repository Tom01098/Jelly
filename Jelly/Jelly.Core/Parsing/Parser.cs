using Jelly.Core.Parsing.AST;
using Jelly.Core.Parsing.Tokens;
using Jelly.Core.Utility;
using System;
using System.Collections.Generic;

namespace Jelly.Core.Parsing
{
    public class Parser
    {
        private TokenEnumerator tokens;

        public List<FunctionNode> Parse(List<Token> list)
        {
            tokens = new TokenEnumerator(list);
            var functions = new List<FunctionNode>();

            tokens.MoveNext();

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

        // function = signature {construct} end;
        // signature = identifier '<' [parameters] '>' EOL;
        // parameters = identifier {',' identifier};
        private FunctionNode Function()
        {
            var position = tokens.Current.Position;
            var identifier = Identifier();

            if (!IsSymbol(tokens.Current, SymbolType.OpenAngleParenthesis))
            {
                throw new JellyException("Expected '<'", tokens.Current.Position);
            }

            tokens.MoveNext();
            
            var parameters = new List<IdentifierNode>();

            if (!IsSymbol(tokens.Current, SymbolType.CloseAngleParenthesis))
            {
                parameters.Add(Identifier());

                while (!IsSymbol(tokens.Current, SymbolType.CloseAngleParenthesis))
                {
                    if (!IsSymbol(tokens.Current, SymbolType.Comma))
                    {
                        throw new JellyException("Expected ','", tokens.Current.Position);
                    }

                    tokens.MoveNext();
                    parameters.Add(Identifier());
                }
            }

            tokens.MoveNext();

            if (!(tokens.Current is EOLToken))
            {
                throw new JellyException("A function signature must end with a newline", 
                                         tokens.Current.Position);
            }

            tokens.MoveNext();

            var constructs = new List<IConstructNode>();

            while (!IsKeyword(tokens.Current, KeywordType.End))
            {
                constructs.Add(Construct());
            }

            tokens.MoveNext();

            if (!(tokens.Current is EOLToken))
            {
                throw new JellyException("Expected newline", tokens.Current.Position);
            }

            tokens.MoveNext();

            return new FunctionNode(identifier, parameters, constructs, position);
        }

        // construct = statement EOL | if_block;
        private IConstructNode Construct()
        {
            throw new NotImplementedException();
        }

        // if_block = 'if' conditional_block {'elif' conditional_block} ['else' EOL {construct} end];
        private IfBlockNode IfBlock()
        {
            throw new NotImplementedException();
        }

        // conditional_block = value EOL {construct} end;
        private ConditionalBlockNode ConditionalBlock()
        {
            throw new NotImplementedException();
        }

        // statement = return | assignment | mutation | call;
        private IStatementNode Statement()
        {
            throw new NotImplementedException();
        }

        // assignment = identifier '=' value;
        private AssignmentNode Assignment()
        {
            throw new NotImplementedException();
        }

        // mutation = identifier '=>' value;
        private MutationNode Mutation()
        {
            throw new NotImplementedException();
        }

        // call = identifier '<' [arguments] '>';
        // arguments = value {',' value};
        private CallNode Call()
        {
            throw new NotImplementedException();
        }

        // return = '~' value;
        private ReturnNode Return()
        {
            throw new NotImplementedException();
        }

        // value = '(' value ')' | number | identifier | call | operation;
        private IValueNode Value()
        {
            throw new NotImplementedException();
        }

        // operation = value operator value;
        // operator = '+' | '-' | '*' | '/' | '%' | '==' | '!=' | '<' | '>' | '<=' | '>=';
        private OperationNode Operation()
        {
            throw new NotImplementedException();
        }

        // identifier = ? IdentifierToken ?;
        private IdentifierNode Identifier()
        {
            if (tokens.Current is IdentifierToken token)
            {
                var identifier = token.Identifier;
                var pos = token.Position;

                tokens.MoveNext();

                return new IdentifierNode(identifier, pos);
            }
            else
            {
                throw new JellyException("Expected an identifier", tokens.Current.Position);
            }
        }

        // number = ['-'] ? NumberToken ?;
        private NumberNode Number()
        {
            bool isNegative = false;

            if (IsSymbol(tokens.Current, SymbolType.Subtract))
            {
                isNegative = true;
                tokens.MoveNext();
            }

            if (tokens.Current is NumberToken token)
            {
                var num = isNegative ? -token.Number : token.Number;
                var pos = token.Position;

                tokens.MoveNext();

                return new NumberNode(num, pos);
            }
            else
            {
                throw new JellyException("Expected a number", tokens.Current.Position);
            }
        }
    }
}
