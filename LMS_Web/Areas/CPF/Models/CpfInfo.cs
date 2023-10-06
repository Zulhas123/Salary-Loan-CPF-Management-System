using LMS_Web.Areas.Loan.Models;
using LMS_Web.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_Web.Areas.CPF.Models
{
    public class CpfInfo
    {
        public int Id { get; set; }
        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal SelfContribution { get; set; }
        public decimal GovtContribution { get; set; }
        public decimal ArrearsBasic { get; set; }
        public decimal TotalContribution { get; set; }
        public decimal GrandTotal { get; set;}
        public int Month { get; set;}
        public int Year { get; set;}
        public virtual AppUser AppUser { get; set; }




    }
}
