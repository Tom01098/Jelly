using Jelly.Core.Utility;
using System.Collections.Generic;

namespace Jelly.Core.Parsing.AST
{
    public class ArgumentsNode : Node
    {
        public List<IValueNode> Arguments { get; }

        public ArgumentsNode(List<IValueNode> args, Position position)
            : base(position) =>
            Arguments = args;
    }
}
