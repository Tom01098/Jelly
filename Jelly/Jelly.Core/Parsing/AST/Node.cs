using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.AST
{
    public abstract class Node
    {
        public Position Position { get; set; }

        public Node(Position position) => Position = position;
    }
}
