using Jelly.Core.Parsing.AST;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Jelly.Core.Interpreting
{
    public class InternalFunction : IFunction
    {
        private MethodInfo info;

        public InternalFunction(MethodInfo info)
        {
            this.info = info;
        }

        public double Execute(List<double> arguments)
        {
            return (double)info.Invoke(null, arguments.Cast<object>().ToArray());
        }
    }
}
