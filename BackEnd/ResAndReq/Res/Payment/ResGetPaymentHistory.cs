using BackEnd.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ResAndReq.Res.Payment
{
    public class ResGetPaymentHistory : ResBase
    {
        public List<Entities.Payment> Payments { get; set; }

        public ResGetPaymentHistory()
        {
            Payments = new List<Entities.Payment>();
        }
    }
}