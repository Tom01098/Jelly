namespace Jelly.Core.Utility
{
    public readonly struct Position
    {
        public int Line { get; }
        public int Character { get; }

        public Position(int line, int character) => 
            (Line, Character) = (line, character);

        public override string ToString() => $"({Line}, {Character})";
    }
}
