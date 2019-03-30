using Jelly.Core.Utility;

namespace Jelly.Core.Parsing.AST
{
    public interface IFunction
    {
        string Name { get; }
        int ParameterCount { get; }
        Position Position { get; }
    }
}
