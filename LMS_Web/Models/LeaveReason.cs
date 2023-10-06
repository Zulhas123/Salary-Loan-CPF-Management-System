using System.ComponentModel.DataAnnotations;

namespace LMS_Web.Models
{
    public class LeaveReason : BaseClass
    {
        public int Id { get; set; }

        [Display(Name = "নাম")]
        public string Name { get; set; }
        public bool IsActive { get; set; }

    }
}
