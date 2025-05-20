using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ResAndReq.Req.Routine
{
    public class ReqGetRoutineReport
    {
        public string Token { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string RoutineName { get; set; }
        public string UserCedula { get; set; }
    }
}
