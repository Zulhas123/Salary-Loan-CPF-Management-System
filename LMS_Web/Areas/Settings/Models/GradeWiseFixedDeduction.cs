using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Web.Areas.Settings.Models
{
    public class GradeWiseFixedDeduction:BaseClass
    {
        public int Id { get; set; }
        [ForeignKey("Deduction")]
        public int DeductionId { get; set; }
        [ForeignKey("FromGrade")]
        public int FromGradeId { get; set; }
        [ForeignKey("ToGrade")]
        public int ToGradeId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public virtual Grade FromGrade { get; set; }
        public virtual Grade ToGrade { get; set; }
        public virtual Deduction Deduction { get; set; }
    }
}
