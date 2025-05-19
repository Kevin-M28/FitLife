using BackEnd.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ResAndReq.Res.Membership
{
    public class ResCheckMembershipStatus : ResBase
    {
        public string MembershipStatus { get; set; }
        public string MembershipName { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DaysRemaining { get; set; }
    }
}