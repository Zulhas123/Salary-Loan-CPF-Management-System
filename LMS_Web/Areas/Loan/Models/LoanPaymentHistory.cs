using LMS_Web.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_Web.Areas.Loan.Models
{
    public class LoanPaymentHistory
    {
        public int Id { get; set; }
        [ForeignKey("UserWiseLoan")]
        public int UserWiseLoanId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal PaidAmount { get;set; }
        public int InstallmentNoBeforePaid { get; set; }
        public string Document { get; set; }
        public string BankNo { get; set; }
        public string CreatedBy { get; set; }      
        public string AppUserId { get; set; }
        public virtual UserWiseLoan UserWiseLoan { get; set; }
    }
}
