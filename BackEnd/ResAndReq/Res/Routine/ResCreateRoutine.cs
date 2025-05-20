using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ResAndReq.Res.Routine
{
    public class ResCreateRoutine : ResBase
    {
        public Entities.Routine Routine { get; set; }

        public ResCreateRoutine()
        {
            Routine = new Entities.Routine();
        }

        public void FromSPResult(dynamic resultado)
        {
            if (resultado != null && resultado.Result == "SUCCESS")
            {
                this.Result = true;
                this.Routine.RoutineID = resultado.RoutineID ?? 0;
                this.Routine.Name = resultado.Name;
                this.Routine.Description = resultado.Description;
                this.Routine.DifficultyLevelName = resultado.DifficultyLevelName;
                this.Routine.DurationInDays = resultado.DurationInDays ?? 0;
                this.Routine.CreatedByUser = resultado.CreatedByUser;
                this.Routine.CreatedAt = resultado.CreatedAt ?? DateTime.Now;
            }
        }
    }

}
