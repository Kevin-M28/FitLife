using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ResAndReq.Req.Routine
{
    public class ReqAddExerciseToRoutine
    {
        public string Token { get; set; }
        public string RoutineName { get; set; }
        public int DayNumber { get; set; }
        public string DayName { get; set; }
        public string ExerciseName { get; set; }
        public int Sets { get; set; }
        public int Repetitions { get; set; }
        public int RestTimeSeconds { get; set; }
        public string Notes { get; set; }
    }

}
