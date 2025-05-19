using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entities
{
    public class Payment
    {
        public int PaymentID { get; set; }
        public int UserID { get; set; }
        public int UserMembershipID { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethodName { get; set; }
        public DateTime PaymentDate { get; set; }
        public string ReceiptFilePath { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected
        public string Notes { get; set; }
        public string MembershipName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}