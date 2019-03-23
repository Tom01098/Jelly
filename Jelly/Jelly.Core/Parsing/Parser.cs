using Jelly.Core.Parsing.AST;
using Jelly.Core.Parsing.Tokens;
using Jelly.Core.Utility;
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

        private bool IsOperator(Token token, OperatorType type)
        {
            return token is OperatorToken o && o.Operator == type;
        }

        private bool IsKeyword(Token token, KeywordType type)
        {
            return token is KeywordToken k && k.Keyword == type;
        }

        private bool IsEOL(Token token)
        {
            return token is EOLToken;
        }

        // absolute = '|' value '|'
        private AbsoluteNode Absolute()
        {
            var position = tokens.Current.Position;

            if (!IsSymbol(tokens.Current, SymbolType.Pipe))
            {
                throw new JellyException("Expected '|'", 
                                         tokens.Current.Position);
            }

            tokens.MoveNext();
            var value = Value();

            if (!IsSymbol(tokens.Current, SymbolType.Pipe))
            {
                throw new JellyException("Expected '|'",
                                         tokens.Current.Position);
            }

            tokens.MoveNext();
            return new AbsoluteNode(value, position);
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
        private List<ITermNode> Arguments()
        {
            var arguments = new List<ITermNode>();

            if (!IsSymbol(tokens.Current, SymbolType.CloseAngleParenthesis))
            {
                arguments.Add(Value());

                while (IsSymbol(tokens.Current, SymbolType.Comma))
                {
                    tokens.MoveNext();
                    arguments.Add(Value());
                }
            }

            return arguments;
        }

        // call = identifier '<' [arguments] '>';
        private CallNode Call()
        {
            var position = tokens.Current.Position;
            var identifier = Identifier();

            if (!IsSymbol(tokens.Current, SymbolType.OpenAngleParenthesis))
            {
                throw new JellyException("Expected '<'", tokens.Current.Position);
            }

            tokens.MoveNext();
            var arguments = Arguments();

            if (!IsSymbol(tokens.Current, SymbolType.CloseAngleParenthesis))
            {
                throw new JellyException("Expected '>'", tokens.Current.Position);
            }

            tokens.MoveNext();
            return new CallNode(identifier, arguments, position);
        }

        // conditional_block = value EOL {construct} end;
        private ConditionalBlockNode ConditionalBlock(bool isElse)
        {
            var position = tokens.Current.Position;
            var condition = isElse ? null : Value();

            if (!IsEOL(tokens.Current))
            {
                throw new JellyException("A conditional value must end with a newline",
                                         tokens.Current.Position);
            }

            tokens.MoveNext();
            var constructs = Constructs();
            End();
            return new ConditionalBlockNode(condition, constructs, position);
        }

        // construct = if_block | loop_block | statement EOL;
        private IConstructNode Construct()
        {
            if (IsKeyword(tokens.Current, KeywordType.If))
            {
                return IfBlock();
            }
            else if (IsKeyword(tokens.Current, KeywordType.Loop))
            {
                return LoopBlock();
            }
            else
            {
                var statement = Statement();

                if (!IsEOL(tokens.Current))
                {
                    throw new JellyException("A statement must end with a newline",
                                             tokens.Current.Position);
                }

                tokens.MoveNext();
                return statement;
            }
        }

        // constructs = {construct}
        private List<IConstructNode> Constructs()
        {
            var constructs = new List<IConstructNode>();

            while (!IsKeyword(tokens.Current, KeywordType.End))
            {
                constructs.Add(Construct());
            }

            return constructs;
        }

        // end = 'end' EOL
        private void End()
        {
            if (!IsKeyword(tokens.Current, KeywordType.End))
            {
                throw new JellyException("Expected 'end'", tokens.Current.Position);
            }

            tokens.MoveNext();

            if (!IsEOL(tokens.Current))
            {
                throw new JellyException("'end' must be followed by a newline", tokens.Current.Position);
            }

            tokens.MoveNext();
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

            if (!IsEOL(tokens.Current))
            {
                throw new JellyException("A function signature must end with a newline",
                                         tokens.Current.Position);
            }

            tokens.MoveNext();
            var constructs = Constructs();
            End();
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
            var position = tokens.Current.Position;
            var blocks = new List<ConditionalBlockNode>();

            if (!IsKeyword(tokens.Current, KeywordType.If))
            {
                throw new JellyException("Expected 'if'", tokens.Current.Position);
            }

            tokens.MoveNext();
            blocks.Add(ConditionalBlock(false));

            while (IsKeyword(tokens.Current, KeywordType.Elif))
            {
                tokens.MoveNext();
                blocks.Add(ConditionalBlock(false));
            }

            if (IsKeyword(tokens.Current, KeywordType.Else))
            {
                tokens.MoveNext();
                blocks.Add(ConditionalBlock(true));
            }

            return new IfBlockNode(blocks, position);
        }

        // loop_block = 'loop' conditional_block;
        private LoopBlockNode LoopBlock()
        {
            var position = tokens.Current.Position;

            if (!IsKeyword(tokens.Current, KeywordType.Loop))
            {
                throw new JellyException("Expected 'loop'", 
                                         tokens.Current.Position);
            }

            tokens.MoveNext();
            return new LoopBlockNode(ConditionalBlock(false), position);
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

        // negative = '-' term;
        private NegativeNode Negative()
        {
            var position = tokens.Current.Position;

            if (!IsOperator(tokens.Current, OperatorType.Subtract))
            {
                throw new JellyException("Expected '-'", tokens.Current.Position);
            }

            tokens.MoveNext();
            return new NegativeNode(Term(), position);
        }

        // not = '!' term;
        private NotNode Not()
        {
            var position = tokens.Current.Position;

            if (!IsSymbol(tokens.Current, SymbolType.Exclamation))
            {
                throw new JellyException("Expected '!'", tokens.Current.Position);
            }

            tokens.MoveNext();
            return new NotNode(Term(), position);
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

            if (IsEOL(tokens.Current))
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

        // term = '(' value ')' | absolute | not | negative | call | number | identifier;
        private ITermNode Term()
        {
            if (IsSymbol(tokens.Current, SymbolType.OpenParenthesis))
            {
                var position = tokens.Current.Position;
                tokens.MoveNext();
                var value = Value();

                if (!IsSymbol(tokens.Current, SymbolType.CloseParenthesis))
                {
                    throw new JellyException("Expected ')'", tokens.Current.Position);
                }

                tokens.MoveNext();
                return value;
            }
            else if (IsSymbol(tokens.Current, SymbolType.Pipe))
            {
                return Absolute();
            }
            else if (IsSymbol(tokens.Current, SymbolType.Exclamation))
            {
                return Not();
            }
            else if (IsOperator(tokens.Current, OperatorType.Subtract))
            {
                return Negative();
            }
            else if (ShouldParseCall())
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
                throw new JellyException("Only a parenthesised value, not, negative, call, number, or identifier can be used as a term",
                                         tokens.Current.Position);
            }

            bool ShouldParseCall()
            {
                return (tokens.Current is IdentifierToken)
                    && IsSymbol(tokens.LookAhead(1), SymbolType.OpenAngleParenthesis);
            }
        }

        // value = term [operator value];
        private ITermNode Value()
        {
            var position = tokens.Current.Position;

            ITermNode lhs = Term();

            if (tokens.Current is OperatorToken op)
            {
                tokens.MoveNext();
                return new ValueNode(lhs, op.Operator, Value(), position);
            }

            return lhs;
        }
    }
}
