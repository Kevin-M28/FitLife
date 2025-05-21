using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.ResAndReq.Req;

namespace BackEnd.ResAndReq.Req.BodyMetrics
{
    public class ReqGetBodyMetricsHistory : ReqBase
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}