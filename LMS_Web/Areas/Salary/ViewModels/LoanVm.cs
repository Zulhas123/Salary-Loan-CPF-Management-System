namespace LMS_Web.Areas.Salary.ViewModels
{
    public class LoanVm
    {
        public string Name { get; set; }
        public decimal CapitalAmount { get; set; }
        public int CurrentCapitalInstallment { get; set; }
        public int TotalCapitalInstallment { get; set; }
        public decimal InterestAmount { get; set; }
        public int CurrentInterestInstallment { get; set; }
        public int TotalInterestInstallment { get; set; }
    }
}
