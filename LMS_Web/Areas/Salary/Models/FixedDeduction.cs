using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LMS_Web.Models;

namespace LMS_Web.Areas.Salary.Models
{
    public class FixedDeduction:BaseClass
    {
        public int Id { get; set; }
        [ForeignKey("Deduction")]
        public int DeductionId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public virtual  Deduction Deduction { get; set; }
    }
}
