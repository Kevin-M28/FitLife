using BackEnd.ResAndReq.Req;
using System;
using System.Collections.Generic;

namespace BackEnd.ResAndReq.Req.Goals
{
    public class ReqGetPreMadeGoals : ReqBase
    {
        public string Difficulty { get; set; } // Optional filter: Beginner, Intermediate, Advanced
    }

    public class ReqAssignPreMadeGoal : ReqBase
    {
        public int PreMadeGoalID { get; set; }
        public decimal? CustomTargetValue { get; set; }
        public DateTime? CustomTargetDate { get; set; }
    }
    public class ReqGetUserGoals : ReqBase
    {
        public string Status { get; set; } // Active, Achieved, Abandoned
        public string GoalType { get; set; } // Specific goal type name filter
    }

    public class ReqGetUserGoalStats : ReqBase
    {
        // No additional properties needed beyond Token from ReqBase
    }
}

