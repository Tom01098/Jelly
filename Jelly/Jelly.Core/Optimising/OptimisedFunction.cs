using Jelly.Core.Linking;
using Jelly.Core.Parsing.AST;

namespace Jelly.Core.Optimising
{
    public class OptimisedFunction
    {
        public bool IsInternal { get; }

        public IFunction Function { get; set; }
        public bool IsReferenced { get; set; }
        public bool HasStartedOptimisation { get; set; }

        public OptimisedFunction(IFunction function)
            => (Function, IsInternal) = (function, function is InternalFunction);
    }
}
