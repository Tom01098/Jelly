using Jelly.Core.Parsing.AST;

namespace Jelly.Core.Optimising
{
    public class OptimisedFunction
    {
        public IFunction Function { get; }
        public bool IsInternal { get; }

        public int TimesUsed { get; set; }
        public bool HasStartedOptimisation { get; set; }

        public OptimisedFunction(IFunction function, bool isInternal)
            => (Function, IsInternal) = (function, isInternal);
    }
}
