using Jelly.Core.Utility;
using System.Collections.Generic;

namespace Jelly.Core.Parsing.AST
{
    public class FunctionNode : Node
    {
        public IdentifierNode Identifier { get; }
        public List<IdentifierNode> Parameters { get; }
        public List<IConstructNode> Constructs { get; }

        public FunctionNode(IdentifierNode identifier,
                            List<IdentifierNode> parameters,
                            List<IConstructNode> constructs,
                            Position position)
            : base(position) =>
            (Identifier, Parameters, Constructs) = 
            (identifier, parameters, constructs);
            
    }
}
