using LMS_Web.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_Web.Areas.Loan.Models
{
    public class LoanInstallmentInfo
    {
        public long Id { get; set; }       
        public string AppUserId { get; set; }
        [ForeignKey("UserWiseLoan")]
        public int UserWiseLoanId { get; set; }
        public int LoanHeadId { get; set; }
        public  int CapitalInstallmentNo { get; set; }
        public int InterestInstallmentNo { get;set; }
        public int Month { get; set; }          
        public int Year { get; set; }
        public bool IsCapital { get; set; }
        public virtual UserWiseLoan UserWiseLoan { get; set; }
    }
}
