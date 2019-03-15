using System.Collections.Generic;

namespace Jelly.Core.Parsing.AST
{
    public class FunctionNode
    {
        public IdentifierNode Identifier { get; }
        public ParametersNode Parameters { get; }
        public List<ConstructNode> Constructs { get; }

        public FunctionNode(IdentifierNode identifier,
                            ParametersNode parameters,
                            List<ConstructNode> constructs) =>
            (Identifier, Parameters, Constructs) = 
            (identifier, parameters, constructs);
            
    }
}
