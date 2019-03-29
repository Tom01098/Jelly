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
        public List<IFunction> Optimise(List<IFunction> ast)
        {
            return ast;
        }
    }
}
