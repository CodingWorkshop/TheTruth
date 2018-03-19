using System;

namespace DataAccess
{
    public class Video
    {
        public string Category { get; set; }
        public string Date { get; set; }
        public string Url { get; set; }

        public string Code => $"{Category}_{Date}";
    }
}