using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LMS_Web.Models;

namespace LMS_Web.Areas.Salary.Models
{
    public class UserHouseRent:BaseClass
    {
        public int Id { get; set; }
        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }
        [ForeignKey("ResidentStatus")]
        public int ResidentStatusId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual ResidentStatus ResidentStatus { get; set; }
    }
}
