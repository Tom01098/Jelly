using System.Collections.Generic;

namespace Jelly.Core.Parsing.AST
{
    public class ParametersNode
    {
        public List<IdentifierNode> Parameters { get; }

        public ParametersNode(List<IdentifierNode> parameters) =>
            Parameters = parameters;
    }
}
