using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ResAndReq.Req.Payment
{
    public class ReqApprovePayment
    {
        public DateTime PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public string UserCedula { get; set; }
    }
}