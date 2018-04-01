using DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Utility
{
    public static class FilePathExtension
    {
        public static string InitDirectory(this string firstPath,
            string folder = null)
        {
            var path = firstPath;

            if (!string.IsNullOrWhiteSpace(folder))
                path = Path.Combine(firstPath, folder);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

        public static IEnumerable<PathInfo> GetChildPaths(this string path,
            Dictionary<int, string> hierarchy = null)
        {
            if (!Directory.Exists(path))
                yield break;

            var directories = path.GetDirectories();

            var files = path.GetFiles();

            if (files.Any() && directories.Any())
                throw new Exception($"File structure error, on path {path}.");

            if (!files.Any() && !directories.Any())
                yield break;

            if (hierarchy == null)
                hierarchy = new Dictionary<int, string>();

            if (files.Any())
            {
                foreach (var pathInfo in GetFileInfos(path, hierarchy, files))
                    yield return pathInfo;
            }
            else
            {
                foreach (var directory in directories)
                    foreach (var pathInfo in GetChildPaths(
                        path.CombinePath(directory),
                        hierarchy.AddHierarchy(directory)))
                        yield return pathInfo;
            }
        }

        private static IEnumerable<string> GetDirectories(this string path)
        {
            return GetDirectoryInfo(path)
                .GetDirectories()
                .Select(d => d.Name);
        }

        private static IEnumerable<string> GetFiles(this string path)
        {
            return GetDirectoryInfo(path)
                .GetFiles()
                .Select(f => f.Name);
        }

        private static DirectoryInfo GetDirectoryInfo(string path)
        {
            return new DirectoryInfo(path);
        }

        private static Dictionary<int, string> AddHierarchy(this Dictionary<int, string> hierarchy, string name)
        {
            return new Dictionary<int, string>(hierarchy)
            {
                { hierarchy.Count, name }
            };
        }

        private static IEnumerable<PathInfo> GetFileInfos(
            string path, Dictionary<int, string> hierarchy, IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                yield return new PathInfo
                {
                    Hierarchy = hierarchy,
                    FullName = path.CombinePath(file),
                    Name = file
                };
            }
        }

        public static string CombinePath(this string rootPath, string nextTarget)
        {
            return Path.Combine(rootPath, nextTarget);
        }
    }
}