using System.IO;

namespace Jelly.Core.Utility
{
    public static class FileUtility
    {
        public static string[] FindJellyFiles(string folder)
        {
            return Directory.GetFiles(folder, "*.jelly", SearchOption.AllDirectories);
        }
    }
}
