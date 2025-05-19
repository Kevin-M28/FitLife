using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entities
{
    public class Membership
    {
        public string MembershipName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } // Active, Expired, Cancelled
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public int DaysRemaining { get; set; }
        public string MembershipStatus { get; set; } // Active, Inactive, Expired, About to expire
    }
}