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

            // Return the new ast
            // Strip unused functions
            var newAST = new List<IFunction>();

            foreach (var func in functions.Values)
            {
                // TODO Change this to 0 when function stripping is implemented
                if (func.TimesUsed > -1)
                {
                    newAST.Add(func.Function);
                }
            }

            return newAST;
        }

        private void OptimiseFunction(FunctionNode function)
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

            // Put the function back together
            functions[function.Name].Function = new FunctionNode(function.Identifier,
                                                                 function.Parameters,
                                                                 constructs,
                                                                 function.Position);
        }

        // Optimise a construct by removing dead code after a return statement
        private IConstructNode[] OptimiseConstructs(IConstructNode[] constructs,
                                                    Dictionary<string, double?> variables)
        {
            var newConstructs = new List<IConstructNode>();

            foreach (var construct in constructs)
            {
                newConstructs.Add(OptimiseConstruct(construct, variables));

                if (construct is ReturnNode)
                {
                    break;
                }
            }

            return newConstructs.ToArray();
        }

        private IConstructNode OptimiseConstruct(IConstructNode construct,
                                                 Dictionary<string, double?> variables)
        {
            return construct;
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
