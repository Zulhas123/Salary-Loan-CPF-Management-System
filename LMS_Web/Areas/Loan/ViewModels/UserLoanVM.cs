using System;

namespace LMS_Web.Areas.Loan.ViewModels
{
    public class UserLoanVM
    {
        public string EmployeeName { get; set; }
        public string LoanName { get; set; }
        public decimal CapitalAmount { get; set; }
        public int CurrentCapitalInstallment { get; set; } 
        public decimal MonthlyInterestAmount { get; set; }
        public int CurrentInterestInstallment { get; set; }
        public int TotalInterestInstallment { get; set; }
        public int TotalCapitalInstallment { get; set; }
        public decimal CapitalDeduction { get; set; }
        public string loanType { get; set; }
        public string ApproveDate { get; set;}

    }
}
