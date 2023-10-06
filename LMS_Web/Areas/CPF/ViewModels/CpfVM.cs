namespace LMS_Web.Areas.CPF.ViewModels
{
    public class CpfVM
    {
      
        public string EmployeeName { get; set; }      
     
        public string BasicSalary { get; set; }
        public string SelfContribution { get; set; }
        public string GovtContribution { get; set; }
        public string ArrearsBasic { get; set; }
        public string TotalContribution { get; set; }
        public string GrandTotal { get; set; }
        public string CpfFirstLoan { get; set;}
        public string CpfSecondLoan { get; set; }
       public string Month { get; set; }
    }
}
