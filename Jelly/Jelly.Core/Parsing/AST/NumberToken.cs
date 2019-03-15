namespace Jelly.Core.Parsing.AST
{
    public class NumberToken : IValueNode
    {
        public double Number { get; }

        public NumberToken(double number) => Number = number;
    }
}
