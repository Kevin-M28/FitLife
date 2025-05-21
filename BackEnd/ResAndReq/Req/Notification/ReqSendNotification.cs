using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ResAndReq.Req.Notification
{
    public class ReqSendNotification: ReqBase
    {
        public string TargetCedula { get; set; } // puede ser null
        public string NotificationType { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
