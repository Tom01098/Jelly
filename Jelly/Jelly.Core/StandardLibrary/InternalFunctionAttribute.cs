using System;

namespace Jelly.Core.StandardLibrary
{
    [AttributeUsage(AttributeTargets.Method)]
    public class InternalFunctionAttribute : Attribute
    {
        public bool Deterministic { get; }

        public InternalFunctionAttribute(bool deterministic = false)
            => Deterministic = deterministic;
    }
}
