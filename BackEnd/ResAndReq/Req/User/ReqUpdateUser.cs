using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ResAndReq.Req.User
{
    public class ReqUpdateUser: ReqBase
    {
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
