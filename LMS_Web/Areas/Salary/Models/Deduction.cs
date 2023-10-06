using LMS_Web.Areas.Salary.Enum;
using System.Collections;
using System.Collections.Generic;

namespace LMS_Web.Areas.Salary.Models
{
    public class Deduction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsFixed { get; set; }
        public bool IsGradeWiseFixed { get; set; }
        public bool IsUserSpecific { get; set; }
        public DeductionType DeductionType { get; set; }
        public UtilityType UtilityType { get; set; }

        public ICollection<UserDeduction> UserDeductions { get; set; }
    }
}
