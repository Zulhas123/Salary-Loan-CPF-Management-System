using System.Collections.Generic;
using LMS_Web.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using LMS_Web.Areas.Salary.Enum;

namespace LMS_Web.Areas.Salary.Models
{
    public class Station:BaseClass
    {
        public int Id { get; set; }
        [Display(Name = "নাম")]
        public string Name { get; set; }
        public string NameBangla { get; set; }

        [Display(Name = "ঠিকানা")]
        public string Address { get; set; }
        public StationType StationType { get; set; }
        public ICollection<AppUser> AppUsers { get; set; }

    }
}
