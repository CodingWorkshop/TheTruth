using System;
using System.Collections.Generic;

namespace TruthAPI.ViewModels
{
    public class SetVideoViewModel
    {
        public string Id { get; set; }
        public List<string> Codes { get; set; }
        public DateTime? startTime{ get; set; }
        public DateTime? endTime{ get; set; }
    }
}