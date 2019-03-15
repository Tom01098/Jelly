using System.Collections.Generic;

namespace Jelly.Core.Parsing.AST
{
    public class ArgumentsNode
    {
        public List<ValueNode> Arguments { get; }

        public ArgumentsNode(List<ValueNode> args) =>
            Arguments = args;
    }
}
