using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ResAndReq.Req.Notification
{
    public class ReqMarkNotificationAsRead
    {
        public string Token { get; set; }
        public string NotificationTitle { get; set; }
        public DateTime NotificationSentAt { get; set; }
    }

}
