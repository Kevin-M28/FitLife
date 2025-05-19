using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ResAndReq.Req.Payment
{
    public class ReqRegisterPayment
    {
        public string MembershipTypeName { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethodName { get; set; }
        public string ReceiptFilePath { get; set; }
    }
}