using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BackEnd.Logic.Notification;
using BackEnd.ResAndReq.Req.Notification;
using BackEnd.ResAndReq.Res.Notification;

namespace Fitlife.Controllers
{
    [RoutePrefix("api/notifications")]
    public class NotificationController : ApiController
    {
        [HttpPost]
        [Route("unread")]
        public ResGetUnreadNotifications GetUnreadNotifications(ReqGetUnreadNotifications req)
        {
            return new LogNotification().GetUnreadNotifications(req);
        }

        [HttpPost]
        [Route("mark-read")]
        public ResMarkNotificationAsRead MarkNotificationAsRead(ReqMarkNotificationAsRead req)
        {
            return new LogNotification().MarkNotificationAsRead(req);
        }

        [HttpPost]
        [Route("send")]
        public ResSendNotification SendNotification(ReqSendNotification req)
        {
            return new LogNotification().SendNotification(req);
        }


    }
}