using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ResAndReq.Res.Routine
{
    /// <summary>
    /// ✅ ENHANCED: Updated response class with comprehensive progress tracking
    /// </summary>
    public class ResRegisterExerciseProgress : ResBase
    {
        public string Message { get; set; }

        // ✅ Enhanced progress tracking fields - Updated to match SP output
        public string ExerciseName { get; set; }
        public string RoutineName { get; set; }
        public int? DayNumber { get; set; }
        public string DayName { get; set; }
        public int? CompletedSets { get; set; }
        public int? CompletedRepetitions { get; set; }
        public decimal? Weight { get; set; }

        // ✅ Progress indicators
        public decimal? ExerciseProgressPercentage { get; set; } // Previously: UpdatedProgressPercentage
        public bool? IsExerciseCompleted { get; set; }
        public bool? IsDayCompleted { get; set; }
        public bool? IsRoutineCompleted { get; set; }

        // ✅ Today's workout statistics
        public int? TodayTotalSets { get; set; } // Previously: TotalSetsCompleted
        public int? TodayTotalReps { get; set; } // Previously: TotalRepsCompleted
        public decimal? TodayTotalWeight { get; set; } // Previously: TotalWeightLifted

        // ✅ Optional gamification fields (for future use)
        public int? ExperiencePoints { get; set; }
        public int? StreakDays { get; set; }

        /// <summary>
        /// ✅ Constructor with sensible default values
        /// </summary>
        public ResRegisterExerciseProgress()
        {
            // Initialize with default values to prevent null reference issues
            ExerciseProgressPercentage = 0;
            IsExerciseCompleted = false;
            IsDayCompleted = false;
            IsRoutineCompleted = false;
            TodayTotalSets = 0;
            TodayTotalReps = 0;
            TodayTotalWeight = 0;
        }
    }
}