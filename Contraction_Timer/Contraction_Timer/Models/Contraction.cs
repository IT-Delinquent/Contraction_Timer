using System;

namespace Contraction_Timer.Models
{
    /// <summary>
    /// Class for holding information about a contraction
    /// </summary>
    public class Contraction
    {
        /// <summary>
        /// The file string to save the contraction data under
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// The start time of the contraction
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// The end time of the contraction
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// The duration of the contraction
        /// </summary>
        public string Duration { get; set; }

        /// <summary>
        /// The pain level of the contraction
        /// </summary>
        public int PainLevel { get; set; }
    }
}