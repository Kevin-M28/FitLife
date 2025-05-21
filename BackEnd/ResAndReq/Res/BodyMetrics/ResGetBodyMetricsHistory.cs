using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Entities;
using BackEnd.ResAndReq.Res;

namespace BackEnd.ResAndReq.Res.BodyMetrics
{
    public class ResGetBodyMetricsHistory : ResBase
    {
        public List<BodyMetric> Metrics { get; set; }

        public ResGetBodyMetricsHistory()
        {
            Metrics = new List<BodyMetric>();
        }
    }
}