using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Entities;
using BackEnd.ResAndReq.Res;

namespace BackEnd.ResAndReq.Res.Goals
{
    public class ResGetPreMadeGoals : ResBase
    {
        public List<BackEnd.Entities.PreMadeGoal> PreMadeGoals { get; set; }

        public ResGetPreMadeGoals()
        {
            PreMadeGoals = new List<BackEnd.Entities.PreMadeGoal>();
        }
    }

    public class ResAssignPreMadeGoal : ResBase
    {
        public int? GoalTypeID { get; set; }
        public decimal? TargetValue { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? TargetDate { get; set; }
    }
}