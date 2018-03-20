using System;
using System.Globalization;

namespace DataAccess
{
    public class Video
    {
        public string Category { get; set; }
        public string Date { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }

        public string Code => $"{Category}_{Date}";
        public DateTime DateTime => DateTime.ParseExact(Date, "yyyyMMdd", CultureInfo.InvariantCulture);
    }
}