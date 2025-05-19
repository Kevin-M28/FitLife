using BackEnd.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ResAndReq.Res.Membership
{
    public class ResGetMembershipTypes : ResBase
    {
        public List<MembershipType> MembershipTypes { get; set; }

        public ResGetMembershipTypes()
        {
            MembershipTypes = new List<MembershipType>();
        }
    }
}