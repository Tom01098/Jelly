using System;

namespace Jelly.Core.Utility
{
    [Serializable]
    public class JellyException : Exception
    {
        public JellyException() { }

        public JellyException(string message, Position position) 
            : base($"{message} at {position}") { }
    }
}
