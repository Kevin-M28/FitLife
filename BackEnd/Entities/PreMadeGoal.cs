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

    public class UserGoal
    {
        public int UserGoalID { get; set; }
        public string GoalTypeName { get; set; }
        public string GoalTypeDescription { get; set; }
        public decimal? TargetValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime TargetDate { get; set; }
        public string Status { get; set; }
        public decimal ProgressPercentage { get; set; }
        public int? DaysRemaining { get; set; }
        public bool IsOverdue { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Computed properties for frontend - Compatible with C# 7.3
        public string FormattedTargetValue => TargetValue?.ToString("F1") ?? "N/A";
        public string FormattedProgress => $"{ProgressPercentage:F1}%";

        public string FormattedDaysRemaining
        {
            get
            {
                if (!DaysRemaining.HasValue || Status != "Active") return "";
                if (DaysRemaining.Value < 0) return "Vencida";
                if (DaysRemaining.Value == 0) return "Hoy";
                if (DaysRemaining.Value == 1) return "Mañana";
                return $"{DaysRemaining.Value} días";
            }
        }

        public string StatusColor
        {
            get
            {
                var status = Status?.ToLower();

                if (status == "active")
                    return IsOverdue ? "#EF4444" : "#10B981"; // Red if overdue, green if active
                else if (status == "achieved")
                    return "#3B82F6"; // Blue
                else if (status == "abandoned")
                    return "#6B7280"; // Gray
                else
                    return "#6B7280"; // Default gray
            }
        }
    }

    public class UserGoalStats
    {
        public int TotalGoals { get; set; }
        public int ActiveGoals { get; set; }
        public int AchievedGoals { get; set; }
        public int AbandonedGoals { get; set; }
        public int OverdueGoals { get; set; }
        public decimal AvgProgressPercentage { get; set; }
        public string MostRecentGoalType { get; set; }
        public DateTime? NextDeadline { get; set; }

        // Computed properties - Compatible with C# 7.3
        public decimal AchievementRate => TotalGoals > 0 ? (decimal)AchievedGoals / TotalGoals * 100 : 0;
        public string FormattedAvgProgress => $"{AvgProgressPercentage:F1}%";
        public string FormattedAchievementRate => $"{AchievementRate:F1}%";

        public string NextDeadlineText
        {
            get
            {
                if (!NextDeadline.HasValue) return "Sin metas próximas";

                var daysLeft = (NextDeadline.Value.Date - DateTime.Today).Days;

                if (daysLeft < 0) return "Meta vencida";
                if (daysLeft == 0) return "Meta vence hoy";
                if (daysLeft == 1) return "Meta vence mañana";

                return $"Próxima meta en {daysLeft} días";
            }
        }
    }
}