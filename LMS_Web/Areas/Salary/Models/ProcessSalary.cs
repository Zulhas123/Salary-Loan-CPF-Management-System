namespace LMS_Web.Areas.Salary.Models
{
    public class ProcessSalary
    {
        public int Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool IsFinal { get; set; }
    }
}
