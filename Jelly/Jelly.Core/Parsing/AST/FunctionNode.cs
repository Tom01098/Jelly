using System.Collections.Generic;

namespace Jelly.Core.Parsing.AST
{
    public class FunctionNode
    {
        public IdentifierNode Identifier { get; }
        public ParametersNode Parameters { get; }
        public List<IConstructNode> Constructs { get; }

        public FunctionNode(IdentifierNode identifier,
                            ParametersNode parameters,
                            List<IConstructNode> constructs) =>
            (Identifier, Parameters, Constructs) = 
            (identifier, parameters, constructs);
            
    }
}
