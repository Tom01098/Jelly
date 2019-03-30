using Jelly.Core.Parsing.AST;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jelly.Core.Optimising
{
    /// <summary>
    /// Optimises an AST by performing common optimisations
    /// such as dead code elimination and constant folding.
    /// </summary>
    public class Optimiser
    {
        private Dictionary<string, IFunction> functions = new Dictionary<string, IFunction>();
        private List<IFunction> usedFunctions = new List<IFunction>();

        public List<IFunction> Optimise(List<IFunction> ast)
        {
            // Setup dictionary of available functions
            foreach (var func in ast)
            {
                functions.Add(func.Name, func);
            }

            // Optimise the Main function, this will also determine all of the functions
            // used throughout the program
            OptimiseFunction(functions["Main"]);

            // TODO Remove this, change for usedFunctions return
            return ast;
        }

        private IFunction OptimiseFunction(IFunction function)
        {
            return function;
        }

        private List<IConstructNode> OptimiseConstructs(List<IConstructNode> constructs,
                                                        Dictionary<string, double?> variables)
        {
            return constructs;
        }

        private IStatementNode OptimiseStatement(IStatementNode statement,
                                                 Dictionary<string, double?> variables)
        {
            return statement;
        }

        private ITermNode OptimiseTerm(ITermNode term,
                                       Dictionary<string, double?> variables)
        {
            return term;
        }
    }
}
