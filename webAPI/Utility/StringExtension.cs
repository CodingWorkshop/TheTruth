using System.IO;

namespace Utility
{
    public static class StringExtension
    {
        public static string CreateDirectory(this string firstPath, string folder)
        {
            var path = Path.Combine(firstPath, folder);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }
    }
}