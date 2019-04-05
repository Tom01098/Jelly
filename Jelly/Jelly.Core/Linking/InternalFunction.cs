using Jelly.Core.Parsing.AST;
using Jelly.Core.Utility;
using System.Linq;
using System.Reflection;

namespace Jelly.Core.Linking
{
    public class InternalFunction : IFunction
    {
        public string Name { get; }
        public bool Deterministic { get; }
        public int ParameterCount { get; }
        public Position Position { get; }

        private MethodInfo info;

        public InternalFunction(MethodInfo info, bool deterministic)
        {
            Name = info.Name;
            Deterministic = deterministic;
            ParameterCount = info.GetParameters().Length;
            Position = new Position("Internal", 0,  0);

            this.info = info;
        }

        public double Execute(double[] arguments)
        {
            try
            {
                return (double)info.Invoke(null, arguments.Cast<object>().ToArray());
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
