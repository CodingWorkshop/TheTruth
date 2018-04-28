using System;
using System.Collections.Generic;

namespace TruthAPI.ViewModels
{
    public class SetVideoViewModel
    {
        public string Id { get; set; }
        public List<string> Codes { get; set; }
        public DateTime StartTime{ get; set; }
        public DateTime EndTime{ get; set; }
    }
}