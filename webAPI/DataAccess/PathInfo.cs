using System.Collections.Generic;

namespace DataAccess
{
    public class PathInfo
    {
        public string FullName { get; set; }
        public string Name { get; set; }
        public Dictionary<int, string> Hierarchy { get; set; }
    }
}