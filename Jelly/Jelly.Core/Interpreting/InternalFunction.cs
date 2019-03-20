using Jelly.Core.Parsing.AST;
using Jelly.Core.StandardLibrary.Internal;
using System.Collections.Generic;

namespace Jelly.Core.Interpreting
{
    public class InternalFunction : IFunction
    {
        private Write obj;

        public InternalFunction(Write obj)
        {
            this.obj = obj;
        }

        public double Execute(List<double> arguments)
        {
            return obj.Execute(arguments[0]);
        }
    }
}
