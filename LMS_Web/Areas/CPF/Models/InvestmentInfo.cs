using LMS_Web.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_Web.Areas.CPF.Models
{
    public class InvestmentInfo
    {
        public int Id { get; set; }
        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }
        public decimal InvestmentAmount { get; set; }
        public decimal TotalInvestment { get; set; }
       // public string FiscalYear { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}
