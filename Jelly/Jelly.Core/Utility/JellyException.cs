using System;

namespace Jelly.Core.Utility
{
    /// <summary>
    /// Custom exception type using <see cref="Position"/>.
    /// </summary>
    [Serializable]
    public class JellyException : Exception
    {
        public JellyException() { }

        public JellyException(string message, Position position) 
            : base($"{message} at {position}") { }
    }
}
