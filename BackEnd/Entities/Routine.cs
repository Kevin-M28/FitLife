using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entities
{
    public class Routine
    {
        public int RoutineID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DifficultyLevelID { get; set; }
        public string DifficultyLevelName { get; set; }
        public int CreatedByUserID { get; set; }
        public string CreatedByUser { get; set; }
        public int DurationInDays { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class ActiveRoutine
    {
        public string RoutineName { get; set; }
        public string DifficultyLevelName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public decimal? ProgressPercentage { get; set; }
    }
    public class RoutineDayExercise
    {
        public int DayNumber { get; set; }
        public string DayName { get; set; }
        public string ExerciseName { get; set; }
        public string ExerciseDescription { get; set; }
        public string Category { get; set; }
        public string TargetMuscleGroup { get; set; }
        public int Sets { get; set; }
        public int Repetitions { get; set; }
        public int RestTimeSeconds { get; set; }
        public string Instructions { get; set; }
        public string VideoURL { get; set; }
        public string ImageURL { get; set; }
        public decimal TodayProgress { get; set; }
    }
    public class ExerciseProgress
    {
        public string RoutineName { get; set; }
        public string ExerciseName { get; set; }
        public int CompletedSets { get; set; }
        public int CompletedRepetitions { get; set; }
        public decimal? Weight { get; set; }
        public string Notes { get; set; }
    }
    public class RoutineRating
    {
        public string RoutineName { get; set; }
        public int DifficultyRating { get; set; }
        public int EffectivenessRating { get; set; }
        public int EnjoymentRating { get; set; }
        public string Comments { get; set; }
    }
    public class UserAssignedRoutine
    {
        public int UserAssignedRoutineID { get; set; }
        public string RoutineName { get; set; }
        public string UserName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }
    public class RoutineExercise
    {
        public int RoutineExerciseID { get; set; }
        public string RoutineName { get; set; }
        public int DayNumber { get; set; }
        public string DayName { get; set; }
        public string ExerciseName { get; set; }
        public string CategoryName { get; set; }
        public int Sets { get; set; }
        public int Repetitions { get; set; }
        public int RestTimeSeconds { get; set; }
        public string Notes { get; set; }
    }
    public class RoutineReportEntry
    {
        public string Cedula { get; set; }
        public string FullName { get; set; }
        public string RoutineName { get; set; }
        public string RoutineDescription { get; set; }
        public string DifficultyLevel { get; set; }
        public int DurationInDays { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public decimal ProgressPercentage { get; set; }
        public int? DaysRemaining { get; set; }
        public int ExercisesCompleted { get; set; }
        public int TotalExercises { get; set; }
    }

}
