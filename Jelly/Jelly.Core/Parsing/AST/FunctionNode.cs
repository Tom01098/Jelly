using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.AST
{
    public class FunctionNode : Node, IFunction
    {
        public IdentifierNode Identifier { get; }
        public IdentifierNode[] Parameters { get; }
        public IConstructNode[] Constructs { get; }

        public string Name => Identifier.Identifier;
        public int ParameterCount => Parameters.Length;

        public FunctionNode(IdentifierNode identifier,
                            IdentifierNode[] parameters,
                            IConstructNode[] constructs,
                            Position position)
            : base(position) =>
            (Identifier, Parameters, Constructs) =
            (identifier, parameters, constructs);
    }
}
