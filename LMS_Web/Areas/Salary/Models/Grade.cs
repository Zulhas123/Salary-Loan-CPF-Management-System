using System.Collections;
using System.Collections.Generic;
using LMS_Web.Models;

namespace LMS_Web.Areas.Salary.Models
{
    public class Grade:BaseClass
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<AppUser> AppUsers { get; set; }
        public ICollection<GradeWisePayScale> GradeWisePayScales { get; set; }

    }
}
