using Jelly.Core.Parsing.AST;
using Jelly.Core.Parsing.Tokens;
using Jelly.Core.Utility;
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
            var position = tokens.Current.Position;
            var identifier = Identifier();

            if (!IsSymbol(tokens.Current, SymbolType.Assignment))
            {
                throw new JellyException("Expected '='", 
                                         tokens.Current.Position);
            }

            tokens.MoveNext();
            return new AssignmentNode(identifier, Value(), position);
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

        // {construct}
        private List<IConstructNode> Constructs()
        {
            var constructs = new List<IConstructNode>();

            while (!IsKeyword(tokens.Current, KeywordType.End))
            {
                constructs.Add(Construct());
            }

            return constructs;
        }

        // function = signature {construct} end;
        // signature = identifier '<' [parameters] '>' EOL;
        private FunctionNode Function()
        {
            var position = tokens.Current.Position;
            var identifier = Identifier();

            if (!IsSymbol(tokens.Current, SymbolType.OpenAngleParenthesis))
            {
                throw new JellyException("Expected '<'", tokens.Current.Position);
            }

            tokens.MoveNext();
            var parameters = Parameters();

            if (!IsSymbol(tokens.Current, SymbolType.CloseAngleParenthesis))
            {
                throw new JellyException("Expected '>'", tokens.Current.Position);
            }

            tokens.MoveNext();

            if (!(tokens.Current is EOLToken))
            {
                throw new JellyException("A function signature must end with a newline",
                                         tokens.Current.Position);
            }

            tokens.MoveNext();
            var constructs = Constructs();

            if (!IsKeyword(tokens.Current, KeywordType.End))
            {
                throw new JellyException("Expected 'end'", tokens.Current.Position);
            }

            tokens.MoveNext();

            if (!(tokens.Current is EOLToken))
            {
                throw new JellyException("'end' must be followed by a newline",
                                         tokens.Current.Position);
            }

            tokens.MoveNext();
            return new FunctionNode(identifier, parameters, constructs, position);
        }

        // identifier = ? IdentifierToken ?;
        private IdentifierNode Identifier()
        {
            if (tokens.Current is IdentifierToken token)
            {
                tokens.MoveNext();
                return new IdentifierNode(token.Identifier, token.Position);
            }

            throw new JellyException("Expected an identifier", tokens.Current.Position);
        }

        // if_block = 'if' conditional_block {'elif' conditional_block} ['else' EOL {construct} end;
        private IfBlockNode IfBlock()
        {
            throw new NotImplementedException();
        }

        // mutation = identifier '=>' value;
        private MutationNode Mutation()
        {
            var position = tokens.Current.Position;
            var identifier = Identifier();

            if (!IsSymbol(tokens.Current, SymbolType.Mutation))
            {
                throw new JellyException("Expected '=>'",
                                         tokens.Current.Position);
            }

            tokens.MoveNext();
            return new MutationNode(identifier, Value(), position);
        }

        // negative = '-' value;
        private NegativeNode Negative()
        {
            var position = tokens.Current.Position;

            if (!IsSymbol(tokens.Current, SymbolType.Subtract))
            {
                throw new JellyException("Expected '-'", tokens.Current.Position);
            }

            tokens.MoveNext();
            return new NegativeNode(Value(), position);
        }

        // not = '!' value;
        private NotNode Not()
        {
            var position = tokens.Current.Position;

            if (!IsSymbol(tokens.Current, SymbolType.Exclamation))
            {
                throw new JellyException("Expected '!'", tokens.Current.Position);
            }

            tokens.MoveNext();
            return new NotNode(Value(), position);
        }

        // number = ? NumberToken ?;
        private NumberNode Number()
        {
            if (tokens.Current is NumberToken token)
            {
                tokens.MoveNext();
                return new NumberNode(token.Number, token.Position);
            }

            throw new JellyException("Expected a number", tokens.Current.Position);
        }

        // parameters = identifier {',' identifier};
        private List<IdentifierNode> Parameters()
        {
            var parameters = new List<IdentifierNode>();

            if (!IsSymbol(tokens.Current, SymbolType.CloseAngleParenthesis))
            {
                parameters.Add(Identifier());

                while (IsSymbol(tokens.Current, SymbolType.Comma))
                {
                    tokens.MoveNext();
                    parameters.Add(Identifier());
                }
            }

            return parameters;
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

            if (tokens.Current is EOLToken)
            {
                return new ReturnNode(null, position);
            }

            return new ReturnNode(Value(), position);
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

        // term = '(' value ')' | not | negative | call | number | identifier;
        private ITermNode Term()
        {
            if (IsSymbol(tokens.Current, SymbolType.OpenParenthesis))
            {
                var position = tokens.Current.Position;
                tokens.MoveNext();
                var value = Value();
                value.Position = position;

                if (!IsSymbol(tokens.Current, SymbolType.CloseParenthesis))
                {
                    throw new JellyException("Expected ')'", tokens.Current.Position);
                }

                tokens.MoveNext();
                return value;
            }
            else if (IsSymbol(tokens.Current, SymbolType.Exclamation))
            {
                return Not();
            }
            else if (IsSymbol(tokens.Current, SymbolType.Subtract))
            {
                return Negative();
            }
            else if (IsSymbol(tokens.LookAhead(1), SymbolType.OpenAngleParenthesis))
            {
                return Call();
            }
            else if (tokens.Current is NumberToken)
            {
                return Number();
            }
            else if (tokens.Current is IdentifierToken)
            {
                return Identifier();
            }
            else
            {
                throw new JellyException("Only a parenthesised value, not, negative, call, number, or identifier can be used as a term.",
                                         tokens.Current.Position);
            }
        }

        // value = term {operator term};
        private ValueNode Value()
        {
            var position = tokens.Current.Position;

            // TODO Operator tree
            return new ValueNode(Term(), OperatorType.None, null, position);
        }
    }
}
