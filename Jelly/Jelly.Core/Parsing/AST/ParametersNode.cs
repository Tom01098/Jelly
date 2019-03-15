using Jelly.Core.Utility;
using System.Collections.Generic;

namespace Jelly.Core.Parsing.AST
{
    public class ParametersNode : Node
    {
        public List<IdentifierNode> Parameters { get; }

        public ParametersNode(List<IdentifierNode> parameters, 
                              Position position)
            : base(position) =>
            Parameters = parameters;
    }
}
