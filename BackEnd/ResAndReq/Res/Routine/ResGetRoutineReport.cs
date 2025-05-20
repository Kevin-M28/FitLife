using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Entities;

namespace BackEnd.ResAndReq.Res.Routine
{
    public class ResGetRoutineReport : ResBase
    {
        public List<RoutineReportEntry> ReportEntries { get; set; }

        public ResGetRoutineReport()
        {
            ReportEntries = new List<RoutineReportEntry>();
        }
    }

}
