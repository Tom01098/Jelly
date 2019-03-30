using Jelly.Core.Parsing.AST;
using Jelly.Core.Utility;
using System.Collections.Generic;
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

        public double Execute(List<double> arguments)
        {
            return (double)info.Invoke(null, arguments.Cast<object>().ToArray());
        }
    }
}
