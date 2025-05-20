using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ResAndReq.Req.Routine
{
    public class ReqRateRoutine
    {
        public string Token { get; set; }
        public string RoutineName { get; set; }
        public int DifficultyRating { get; set; }
        public int EffectivenessRating { get; set; }
        public int EnjoymentRating { get; set; }
        public string Comments { get; set; }
    }

}
