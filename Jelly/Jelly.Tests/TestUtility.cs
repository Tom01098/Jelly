using Jelly.Core.Utility;

namespace Jelly.Tests
{
    public static class TestUtility
    {
        public static Position Position(int line, int character) =>
            new Position("test", line, character);
    }
}
