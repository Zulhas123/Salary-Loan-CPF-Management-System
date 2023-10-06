using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LMS_Web.Models;

namespace LMS_Web.Areas.Salary.Models
{
    public class UserDeduction : BaseClass
    {
        public int Id { get; set; }
        public int DeductionId { get; set; }
        public string AppUserId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public bool IsSameEveryMonth { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Remarks { get; set; }

        public virtual Deduction Deduction { get; set; }
        public virtual AppUser AppUser { get; set; }


    }
}
