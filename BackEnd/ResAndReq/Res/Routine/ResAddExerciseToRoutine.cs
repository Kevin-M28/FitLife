using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Entities;

namespace BackEnd.ResAndReq.Res.Routine
{
    public class ResAddExerciseToRoutine : ResBase
    {
        public RoutineExercise Exercise { get; set; }
        public string Message { get; set; }

        public ResAddExerciseToRoutine()
        {
            Exercise = new RoutineExercise();
        }
    }

}
