using System.Collections.Generic;

namespace Jelly.Core.Parsing.AST
{
    public class ArgumentsNode
    {
        public List<IValueNode> Arguments { get; }

        public ArgumentsNode(List<IValueNode> args) =>
            Arguments = args;
    }
}
