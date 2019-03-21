using System.Collections.Generic;

namespace Jelly.Core.Interpreting
{
    public class ValueStack
    {
        private Stack<ValueStackFrame> values = new Stack<ValueStackFrame>();

        public double this[string index]
        {
            get
            {
                return values.Peek()[index];
            }
        }

        public void PushNewFrame()
        {
            values.Push(new ValueStackFrame());
        }

        public void PopWholeFrame()
        {
            var locals = values.Pop();
        }

        public void PushNewFrameInScope()
        {
            
        }

        public void PopFrame()
        {

        }

        public void Add(string name, double value)
        {
            values.Peek().Add(name, value);
        }

        public void Mutate(string name, double value)
        {
            values.Peek()[name] = value;
        }

        private class ValueStackFrame
        {
            private Dictionary<string, double> values = new Dictionary<string, double>();
            private ValueStackFrame nested;

            public double this[string index]
            {
                get
                {
                    return values[index];
                }
                set
                {
                    values[index] = value;
                }
            }

            public void Push()
            {
                nested = new ValueStackFrame();
            }

            public void Pop()
            {
                nested = null;
            }

            public void Add(string name, double value)
            {
                values.Add(name, value);
            }

            public void Mutate(string name, double value)
            {
                values[name] = value;
            }
        }
    }
}
