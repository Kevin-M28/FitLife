using BackEnd.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ResAndReq.Res.Membership
{
    public class ResGetActiveMembership : ResBase
    {
        public Entities.Membership Membership { get; set; }

        public ResGetActiveMembership()
        {
            Membership = new Entities.Membership();
        }
    }
}