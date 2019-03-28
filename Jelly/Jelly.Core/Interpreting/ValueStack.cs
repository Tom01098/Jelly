using System.Collections.Generic;

namespace Jelly.Core.Interpreting
{
    /// <summary>
    /// A stack of <see cref="ValueStackFrame"/>'s, one for 
    /// each function call.
    /// </summary>
    public class ValueStack
    {
        private Stack<ValueStackFrame> values = new Stack<ValueStackFrame>();

        // Return the value for the given index from the current stack frame
        public double this[string index]
        {
            get
            {
                return values.Peek()[index];
            }
        }

        // Used when entering a new function
        public void PushIndependentFrame()
        {
            values.Push(new ValueStackFrame());
        }

        // Used when exiting a function
        public void PopIndependentFrame()
        {
            values.Pop();
        }

        // Used when entering a new scope within a function
        public void PushRelativeFrame()
        {
            values.Peek().Push();
        }

        // Used when exiting a scope within a function
        public void PopRelativeFrame()
        {
            values.Peek().Pop();
        }

        // Add a value to the current stack frame
        public void Add(string name, double value)
        {
            values.Peek().Add(name, value);
        }

        // Mutate a value in the current stack frame
        public void Mutate(string name, double value)
        {
            values.Peek().Mutate(name, value);
        }

        /// <summary>
        /// Contains the local values in a function,
        /// as well as constraining them to scopes within the function
        /// (such as ifs and loops).
        /// </summary>
        private class ValueStackFrame
        {
            private Dictionary<string, double> values = new Dictionary<string, double>();
            private ValueStackFrame nested;

            // If this is the top frame, return the value from it.
            // Otherwise, return from the frame nested within recursively.
            public double this[string index]
            {
                get
                {
                    if (nested is null)
                    {
                        return values[index];
                    }

                    return nested[index];
                }
            }

            // Push a new stack frame by copying the values.
            // Recursively call to find the top frame.
            public void Push()
            {
                if (nested is null)
                {
                    nested = new ValueStackFrame()
                    {
                        values = new Dictionary<string, double>(values)
                    };
                }
                else
                {
                    nested.Push();
                }
            }

            // Pop the top frame and copy the values to the new top frame.
            public void Pop()
            {
                // If nested.nested is null, the next frame is the top frame.
                if (nested.nested is null)
                {
                    // Copy values from the top scope into this scope
                    // if they exist in it.
                    foreach (var keyValue in nested.values)
                    {
                        if (values.ContainsKey(keyValue.Key))
                        {
                            values[keyValue.Key] = keyValue.Value;
                        }
                    }

                    nested = null;
                }
                // Recursively call until the top frame is reached.
                else
                {
                    nested.Pop();
                }
            }

            // Add a value to the top frame.
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

            // Mutate a value in the top frame.
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
