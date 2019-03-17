namespace Jelly.Core.Utility
{
    /// <summary>
    /// Represents a position of a character in a file.
    /// </summary>
    public readonly struct Position
    {
        public string File { get; }
        public int Line { get; }
        public int Character { get; }

        public Position(string file, int line, int character) => 
            (File, Line, Character) = (file, line, character);

        public override string ToString() => $"({File}, {Line}, {Character})";
    }
}
