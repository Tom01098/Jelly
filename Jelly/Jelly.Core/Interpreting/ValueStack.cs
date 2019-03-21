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
            values.Pop();
        }

        public void PushNewFrameInScope()
        {
            values.Peek().Push();
        }

        public void PopFrame()
        {
            values.Peek().Pop();
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
                    if (nested is null)
                    {
                        return values[index];
                    }
                    else
                    {
                        return nested[index];
                    }
                }
                set
                {
                    if (nested is null)
                    {
                        values[index] = value;
                    }
                    else
                    {
                        nested[index] = value;
                    }
                }
            }

            public void Push()
            {
                nested = new ValueStackFrame
                {
                    values = new Dictionary<string, double>(values)
                };
            }

            public void Pop()
            {
                if (!(nested is null))
                {
                    if (!(nested.values is null))
                    {
                        foreach (var value in nested.values)
                        {
                            if (values.ContainsKey(value.Key))
                            {
                                values[value.Key] = value.Value;
                            }
                        }

                        nested = null;
                    }
                    else
                    {
                        nested.Pop();
                    }
                }
            }

            public void Add(string name, double value)
            {
                if (nested is null)
                {
                    values.Add(name, value);
                }
                else
                {
                    nested.Add(name, value);
                }
            }

            public void Mutate(string name, double value)
            {
                if (nested is null)
                {
                    values[name] = value;
                }
                else
                {
                    nested.Mutate(name, value);
                }
            }
        }
    }
}
