using System;

namespace BackEnd.Entities
{
    public class ScheduledMeasurement
    {
        public int ScheduleID { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string Status { get; set; } // Scheduled, Completed, Missed
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}