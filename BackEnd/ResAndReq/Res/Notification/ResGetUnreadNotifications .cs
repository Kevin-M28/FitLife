using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ResAndReq.Res.Notification
{
    public class ResGetUnreadNotifications : ResBase
    {
        public List<Entities.Notification> Notifications { get; set; }

        public ResGetUnreadNotifications()
        {
            Notifications = new List<Entities.Notification>();
        }
    }

}
