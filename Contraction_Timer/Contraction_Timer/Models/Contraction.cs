using System;
using System.Collections.Generic;

namespace Contraction_Timer.Models
{
    public class Contraction
    {
        public string Filename { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int PainLevel { get; set; }
    }
}
