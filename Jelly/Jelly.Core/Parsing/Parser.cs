using Jelly.Core.Parsing.AST;
using Jelly.Core.Parsing.Tokens;
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

        // function = signature {construct} end;
        // signature = identifier '<' [parameters] '>' EOL;
        private FunctionNode Function()
        {
            throw new NotImplementedException();
        }

        // arguments = value {',' value};
        private ArgumentsNode Arguments()
        {
            throw new NotImplementedException();
        }

        // parameters = identifier {',' identifier};
        private ParametersNode Parameters()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        // number = ['-'] ? NumberToken ?;
        private NumberNode Number()
        {
            throw new NotImplementedException();
        }
    }
}
