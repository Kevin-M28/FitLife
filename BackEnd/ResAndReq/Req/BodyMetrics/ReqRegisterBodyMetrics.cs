using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.ResAndReq.Req;


namespace BackEnd.ResAndReq.Req.BodyMetrics
{
    public class ReqRegisterBodyMetrics : ReqBase
    {
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public decimal? BodyFatPercentage { get; set; }
        public decimal? WaistCircumference { get; set; }
        public decimal? ChestCircumference { get; set; }
        public decimal? ArmCircumference { get; set; }
        public decimal? LegCircumference { get; set; }
        public string Notes { get; set; }
    }
}