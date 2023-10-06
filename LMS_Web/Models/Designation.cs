using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS_Web.Models
{
    public class Designation : BaseClass
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "নাম")]
        public string NameBangla { get; set; }
        public int DisgOrder { get; set; }
        public ICollection<AppUser> AppUsers { get; set; }

    }
}
