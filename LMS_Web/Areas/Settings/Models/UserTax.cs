using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LMS_Web.Models;

namespace LMS_Web.Areas.Settings.Models
{
    public class UserTax : BaseClass
    {
        public int Id { get; set; }
        [ForeignKey("AppUserId")]
        public string AppUserId { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        [Required]
        public decimal DeductedAmount { get; set; }
        [Required]
        public decimal MonthlyDeduction { get; set; }
        [Required]
        // public int CurrentInstallmentNo { get; set; }
        public int TotalInstallment { get; set; }
        [ForeignKey("FiscalYear")]
        public int FiscalYearId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual FiscalYear FiscalYear { get; set; }
    }
}
