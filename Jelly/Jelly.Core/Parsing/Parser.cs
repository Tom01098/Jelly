﻿using Jelly.Core.Parsing.AST;
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

        // construct = if_block | statement EOL;
        private IConstructNode Construct()
        {
            if (IsKeyword(tokens.Current, KeywordType.If))
            {
                return IfBlock();
            }
            else
            {
                var statement = Statement();

                if (!(tokens.Current is EOLToken))
                {
                    throw new JellyException("A statement must end with a newline", 
                                             tokens.Current.Position);
                }

                tokens.MoveNext();

                return statement;
            }
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
            if (IsSymbol(tokens.Current, SymbolType.Return))
            {
                return Return();
            }
            else if (IsSymbol(tokens.LookAhead(1), SymbolType.Assignment))
            {
                return Assignment();
            }
            else if (IsSymbol(tokens.LookAhead(1), SymbolType.Mutation))
            {
                return Mutation();
            }
            else if (IsSymbol(tokens.LookAhead(1), SymbolType.OpenAngleParenthesis))
            {
                return Call();
            }
            else
            {
                throw new JellyException("Only return, assignment, mutation, or call can be used as a statement", 
                                         tokens.Current.Position);
            }
        }

        // assignment = identifier '=' value;
        private AssignmentNode Assignment()
        {
            var position = tokens.Current.Position;
            var identifier = Identifier();

            if (!IsSymbol(tokens.Current, SymbolType.Assignment))
            {
                throw new JellyException("Expected '='", tokens.Current.Position);
            }

            tokens.MoveNext();
            var value = Value();

            return new AssignmentNode(identifier, value, position);
        }

        // mutation = identifier '=>' value;
        private MutationNode Mutation()
        {
            var position = tokens.Current.Position;
            var identifier = Identifier();

            if (!IsSymbol(tokens.Current, SymbolType.Mutation))
            {
                throw new JellyException("Expected '=>'", tokens.Current.Position);
            }

            tokens.MoveNext();
            var value = Value();

            return new MutationNode(identifier, value, position);
        }

        // call = identifier '<' [arguments] '>';
        // arguments = value {',' value};
        private CallNode Call()
        {
            var position = tokens.Current.Position;
            var identifier = Identifier();

            if (!IsSymbol(tokens.Current, SymbolType.OpenAngleParenthesis))
            {
                throw new JellyException("Expected '<'", tokens.Current.Position);
            }

            tokens.MoveNext();
            var arguments = new List<IValueNode>();

            if (!IsSymbol(tokens.Current, SymbolType.CloseAngleParenthesis))
            {
                arguments.Add(Value());

                while (!IsSymbol(tokens.Current, SymbolType.CloseAngleParenthesis))
                {
                    if (!IsSymbol(tokens.Current, SymbolType.Comma))
                    {
                        throw new JellyException("Expected ','", tokens.Current.Position);
                    }

                    tokens.MoveNext();
                    arguments.Add(Value());
                }
            }

            tokens.MoveNext();

            return new CallNode(identifier, arguments, position);
        }

        // return = '~' [value];
        private ReturnNode Return()
        {
            var position = tokens.Current.Position;

            if (!IsSymbol(tokens.Current, SymbolType.Return))
            {
                throw new JellyException("Expected '~'", tokens.Current.Position);
            }

            tokens.MoveNext();
            IValueNode value = null;

            if (!(tokens.Current is EOLToken))
            {
                value = Value();
            }

            return new ReturnNode(value, position);
        }

        // value = '(' value ')' | call | operation | number | identifier;
        private IValueNode Value()
        {
            if (IsSymbol(tokens.Current, SymbolType.OpenParenthesis))
            {
                tokens.MoveNext();
                var value = Value();

                if (!IsSymbol(tokens.Current, SymbolType.CloseParenthesis))
                {
                    throw new JellyException("Expected ')'", tokens.Current.Position);
                }

                tokens.MoveNext();

                return value;
            }
            else if (tokens.Current is IdentifierToken && IsSymbol(tokens.LookAhead(1), SymbolType.OpenAngleParenthesis)
                 && (IsSymbol(tokens.LookAhead(2), SymbolType.CloseAngleParenthesis) 
                 || IsSymbol(tokens.LookAhead(3), SymbolType.CloseAngleParenthesis)
                 || IsSymbol(tokens.LookAhead(3), SymbolType.Comma)))
            {
                return Call();
            }
            else if (IsOperator(tokens.LookAhead(1)) 
                && (!IsOperator(tokens.LookAhead(2)) && !(tokens.LookAhead(2) is EOLToken)))
            {
                return Operation();
            }
            else if (IsSymbol(tokens.Current, SymbolType.Subtract) || tokens.Current is NumberToken)
            {
                return Number();
            }
            else if (tokens.Current is IdentifierToken)
            {
                return Identifier();
            }
            else
            {
                throw new JellyException("Only a parenthesised value, call, operation, number, or identifier can be used as a value", 
                                         tokens.Current.Position);
            }
        }

        // operation = term operator value;
        // operator = '+' | '-' | '*' | '/' | '%' | '==' | '!=' | '<' | '>' | '<=' | '>=';
        // term = '(' value ')' | call | number | identifier;
        private OperationNode Operation()
        {
            var position = tokens.Current.Position;

            IValueNode lhs;

            if (IsSymbol(tokens.Current, SymbolType.OpenParenthesis))
            {
                tokens.MoveNext();
                var value = Value();

                if (!IsSymbol(tokens.Current, SymbolType.CloseParenthesis))
                {
                    throw new JellyException("Expected ')'", tokens.Current.Position);
                }

                tokens.MoveNext();

                lhs = value;
            }
            else if (tokens.Current is IdentifierToken && IsSymbol(tokens.LookAhead(1), SymbolType.OpenAngleParenthesis)
                 && (IsSymbol(tokens.LookAhead(2), SymbolType.CloseAngleParenthesis)
                 || IsSymbol(tokens.LookAhead(3), SymbolType.CloseAngleParenthesis)
                 || IsSymbol(tokens.LookAhead(3), SymbolType.Comma)))
            {
                lhs = Call();
            }
            else if (IsSymbol(tokens.Current, SymbolType.Subtract) || tokens.Current is NumberToken)
            {
                lhs = Number();
            }
            else if (tokens.Current is IdentifierToken)
            {
                lhs = Identifier();
            }
            else
            {
                throw new JellyException("Only a parenthesised value, call, number, or identifier can be used as a term",
                                         tokens.Current.Position);
            }

            OperatorType op;

            if (IsSymbol(tokens.Current, SymbolType.Add))
            {
                op = OperatorType.Add;
            }
            else if (IsSymbol(tokens.Current, SymbolType.Subtract))
            {
                op = OperatorType.Subtract;
            }
            else if (IsSymbol(tokens.Current, SymbolType.Multiply))
            {
                op = OperatorType.Multiply;
            }
            else if (IsSymbol(tokens.Current, SymbolType.Divide))
            {
                op = OperatorType.Divide;
            }
            else if (IsSymbol(tokens.Current, SymbolType.Modulo))
            {
                op = OperatorType.Modulo;
            }
            else if (IsSymbol(tokens.Current, SymbolType.EqualTo))
            {
                op = OperatorType.EqualTo;
            }
            else if (IsSymbol(tokens.Current, SymbolType.UnequalTo))
            {
                op = OperatorType.UnequalTo;
            }
            else if (IsSymbol(tokens.Current, SymbolType.OpenAngleParenthesis))
            {
                op = OperatorType.LessThan;
            }
            else if (IsSymbol(tokens.Current, SymbolType.CloseAngleParenthesis))
            {
                op = OperatorType.GreaterThan;
            }
            else if (IsSymbol(tokens.Current, SymbolType.LessThanOrEqualTo))
            {
                op = OperatorType.LessThanOrEqualTo;
            }
            else if (IsSymbol(tokens.Current, SymbolType.GreaterThanOrEqualTo))
            {
                op = OperatorType.GreaterThanOrEqualTo;
            }
            else
            {
                throw new JellyException("Expected an operator", tokens.Current.Position);
            }

            tokens.MoveNext();
            var rhs = Value();

            return new OperationNode(lhs, op, rhs, position);
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
            var position = tokens.Current.Position;
            bool isNegative = false;

            if (IsSymbol(tokens.Current, SymbolType.Subtract))
            {
                isNegative = true;
                tokens.MoveNext();
            }

            if (tokens.Current is NumberToken token)
            {
                var num = isNegative ? -token.Number : token.Number;

                tokens.MoveNext();

                return new NumberNode(num, position);
            }
            else
            {
                throw new JellyException("Expected a number", tokens.Current.Position);
            }
        }
    }
}
