using System;

namespace LMS_Web.Areas.Loan.ViewModels
{
    public class IndividualLoansVm
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string LoanName { get; set; }
        public string LoanType { get; set; }
        public DateTime? ApproveDate { get; set; }
        public decimal LoanAmount { get; set; }

    }
}
