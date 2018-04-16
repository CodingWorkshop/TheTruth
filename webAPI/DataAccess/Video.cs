using System;
using System.Globalization;

namespace DataAccess
{
    public class Video
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Category { get; set; }
        public string Date { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }

        public string Code => $"{Category}_{Date}_{Name}";
        public DateTime DateTime => DateTime.ParseExact(Date, "yyyyMMdd", CultureInfo.InvariantCulture);
    }
}