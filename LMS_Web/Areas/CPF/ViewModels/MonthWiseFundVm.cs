namespace LMS_Web.Areas.CPF.ViewModels
{
    public class MonthWiseFundVm
    {
        public int EmployeeCount { get; set; }
        public decimal WelfareFund { get; set; }
        public decimal GroupInsurance { get; set; }
        public decimal Rehabilitation { get; set; }
        public string MonthName { get; set; }
    }
}
