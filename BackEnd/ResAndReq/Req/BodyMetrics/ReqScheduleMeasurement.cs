using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.ResAndReq.Req;


namespace BackEnd.ResAndReq.Req.BodyMetrics
{
    public class ReqScheduleMeasurement : ReqBase
    {
        public DateTime ScheduledDate { get; set; }
    }
}