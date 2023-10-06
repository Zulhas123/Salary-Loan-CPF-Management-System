using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LMS_Web.Models;

namespace LMS_Web.Areas.Salary.Models
{
    public class UserSpecificAllowance:BaseClass
    {
        public int Id { get; set; }
        [ForeignKey("AppUser")]
        public string  AppUserId { get; set; }
        [ForeignKey("PayScale")]
        public int PayScaleId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Remarks { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual PayScale PayScale { get; set; }


    }
}
