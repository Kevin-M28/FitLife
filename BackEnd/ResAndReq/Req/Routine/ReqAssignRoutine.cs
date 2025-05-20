using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ResAndReq.Req.Routine
{
    public class ReqAssignRoutine
    {
        public string Token { get; set; }
        public string RoutineName { get; set; }
        public string TargetUserCedula { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
