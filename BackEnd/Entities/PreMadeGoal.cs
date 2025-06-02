using System;

namespace BackEnd.Entities
{
    public class PreMadeGoal
    {
        public int PreMadeGoalID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GoalTypeName { get; set; }
        public decimal DefaultTargetValue { get; set; }
        public int DefaultDurationDays { get; set; }
        public string Difficulty { get; set; }
    }
}