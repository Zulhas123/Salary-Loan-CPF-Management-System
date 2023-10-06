using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS_Web.Models
{
    public class Wing : BaseClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "নাম")]
        public string NameBangla { get; set; }
        public bool IsActive { get; set; }
        public ICollection<AppUser> AppUser { get; set; }
    }
}
