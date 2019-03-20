using System.Collections.Generic;

namespace Jelly.Core.Interpreting
{
    public class ValueStack
    {
        private Stack<Dictionary<string, double>> values = new Stack<Dictionary<string, double>>();

        public double this[string index]
        {
            get
            {
                return values.Peek()[index];
            }
        }

        public void New()
        {
            values.Push(new Dictionary<string, double>());
        }

        public void NewInScope()
        {
            values.Push(values.Peek());
        }

        public void Pop()
        {
            var locals = values.Pop();

            if (values.Count != 0)
            {
                foreach (var name in locals.Keys)
                {
                    if (values.Peek().ContainsKey(name))
                    {
                        values.Peek()[name] = locals[name];
                    }
                }
            }
        }

        public void Add(string name, double value)
        {
            values.Peek().Add(name, value);
        }

        public void Mutate(string name, double value)
        {
            values.Peek()[name] = value;
        }
    }
}
