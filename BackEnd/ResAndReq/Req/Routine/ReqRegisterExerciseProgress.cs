using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ResAndReq.Req.Routine
{
    public class ReqRegisterExerciseProgress
    {
        public string Token { get; set; }
        public string RoutineName { get; set; }
        public string ExerciseName { get; set; }
        public int CompletedSets { get; set; }
        public int CompletedRepetitions { get; set; }
        public decimal? Weight { get; set; }
        public string Notes { get; set; }
    }

}
