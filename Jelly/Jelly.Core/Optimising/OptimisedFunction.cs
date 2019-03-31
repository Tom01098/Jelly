using Jelly.Core.Parsing.AST;

namespace Jelly.Core.Optimising
{
    public class OptimisedFunction
    {
        public bool IsInternal { get; }

        public IFunction Function { get; set; }
        public int TimesUsed { get; set; }
        public bool HasStartedOptimisation { get; set; }

        public OptimisedFunction(IFunction function, bool isInternal)
            => (Function, IsInternal) = (function, isInternal);
    }
}
