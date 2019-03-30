using Jelly.Core.Linking;
using Jelly.Core.Parsing.AST;
using System.Collections.Generic;

namespace Jelly.Core.Optimising
{
    /// <summary>
    /// Optimises an AST by performing common optimisations
    /// such as dead code elimination and constant folding.
    /// </summary>
    public class Optimiser
    {
        private Dictionary<string, OptimisedFunction> functions = new Dictionary<string, OptimisedFunction>();

        public List<IFunction> Optimise(List<IFunction> ast)
        {
            // Setup dictionary of available functions
            foreach (var func in ast)
            {
                functions.Add(func.Name, 
                              new OptimisedFunction(func, func is InternalFunction));
            }

            // Optimise the Main function, this will also determine all of the functions
            // used throughout the program
            functions["Main"].TimesUsed++;
            functions["Main"].HasStartedOptimisation = true;
            OptimiseFunction((FunctionNode)functions["Main"].Function);

            // TODO Replace
            return ast;
        }

        private FunctionNode OptimiseFunction(FunctionNode function)
        {
            // Initialise parameter values to null as, without calling
            // context, the values are unknown.
            var variables = new Dictionary<string, double?>();

            foreach (var param in function.Parameters)
            {
                variables.Add(param.Identifier, null);
            }

            // Optimise the constructs within the function
            var constructs = OptimiseConstructs(function.Constructs, variables);

            // Put the function back together and return it
            return new FunctionNode(function.Identifier, 
                                    function.Parameters, 
                                    constructs, 
                                    function.Position);
        }

        private IConstructNode[] OptimiseConstructs(IConstructNode[] constructs,
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
