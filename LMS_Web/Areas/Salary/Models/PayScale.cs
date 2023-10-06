using System.Collections.Generic;
using LMS_Web.Areas.Salary.Enum;

namespace LMS_Web.Areas.Salary.Models
{
    public class PayScale
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsUserSpecific { get; set; }
        public PayScaleType Type { get; set; }
        public ICollection<GradeWisePayScale> GradeWisePayScales { get; set; }

    }
}
