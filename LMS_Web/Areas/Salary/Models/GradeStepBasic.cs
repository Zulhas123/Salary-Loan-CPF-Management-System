using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LMS_Web.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LMS_Web.Areas.Salary.Models
{
    public class GradeStepBasic
    {
        public int Id { get; set; }
        [ForeignKey("Grade")]
        public int GradeId { get; set; }
        public int StepNo { get; set; }
        [Required]
        public decimal Amount { get; set; }

        public virtual Grade Grade { get; set; }

    }
}
