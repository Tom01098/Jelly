﻿using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.AST
{
    public class ReturnNode : Node, IStatementNode
    {
        public ValueNode Value { get; }

        public ReturnNode(ValueNode value, Position position)
            : base(position) => Value = value;
    }
}
