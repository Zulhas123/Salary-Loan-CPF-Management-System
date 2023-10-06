using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Models;

namespace LMS_Web.Areas.Loan.Models
{
    public class UserWiseLoan:BaseClass
    {
        public int Id { get; set; }
        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }
        public int LoanHeadId { get; set; }
        [Required]
        public decimal LoanAmount { get; set; }
        [Required]
        public int NoOfInstallment { get; set; }
     
        public decimal CapitalDeductionAmount { get; set; }
        public decimal? InterestDeductionAmount { get; set; }
       
        public int? NoOfInstallmentForInterest { get; set; }
        public bool IsPaid { get; set; }
        public bool IsRefundable { get; set; }
        public bool IsApprove { get; set; }
        public int? NonRefundableLoanNo { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string MemorandumNo { get; set; }
        public decimal? FromMain { get; set; }
        public decimal? FromInterest { get; set; }
        public virtual AppUser AppUsers { get; set; }
        public virtual LoanHead LoanHeads { get; set; }
    }
}
