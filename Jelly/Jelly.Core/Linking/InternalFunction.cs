using Jelly.Core.Parsing.AST;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Jelly.Core.Linking
{
    public class InternalFunction : IFunction
    {
        public string Name { get; }
        public int ParameterCount { get; }

        private MethodInfo info;

        public InternalFunction(MethodInfo info)
        {
            this.info = info;
            Name = info.Name;
            ParameterCount = info.GetParameters().Length;
        }

        public double Execute(List<double> arguments)
        {
            return (double)info.Invoke(null, arguments.Cast<object>().ToArray());
        }
    }
}
