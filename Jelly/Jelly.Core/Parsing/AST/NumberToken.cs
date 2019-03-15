namespace Jelly.Core.Parsing.AST
{
    public class NumberToken : ValueNode
    {
        public double Number { get; }

        public NumberToken(double number) => Number = number;
    }
}
