using Jelly.Core.Utility;

namespace Jelly.Core.Verifying
{
    public struct FunctionInfo
    {
        public string Name { get; }
        public int ParameterCount { get; }
        public Position Position { get; }

        public FunctionInfo(string name, int parameterCount, Position position)
            => (Name, ParameterCount, Position) 
            = (name, parameterCount, position);
    }
}
