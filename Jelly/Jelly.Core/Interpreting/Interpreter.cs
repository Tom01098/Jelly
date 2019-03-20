using Jelly.Core.Parsing.AST;
using System.Collections.Generic;

namespace Jelly.Core.Interpreting
{
    /// <summary>
    /// Interpret a given AST at run time
    /// </summary>
    public class Interpreter
    {
        private Dictionary<string, FunctionNode> functions;

        public void Interpret(List<FunctionNode> ast)
        {
            CreateDictionary(ast);
        }

        private void CreateDictionary(List<FunctionNode> ast)
        {
            functions = new Dictionary<string, FunctionNode>();

            foreach (var node in ast)
            {
                functions.Add(node.Identifier.Identifier, node);
            }
        }
    }
}
