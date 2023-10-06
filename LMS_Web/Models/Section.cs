using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS_Web.Models
{
    public class Section : BaseClass
    {
        public int Id { get; set; }

        
        public string Name { get; set; }

        [Display(Name = "নাম")]
        public string NameBangla { get; set; }

        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public bool IsActive { get; set; }
        public ICollection<AppUser> AppUser { get; set; }
        public virtual Department Department { get; set; }

    }
}
