using System.ComponentModel.DataAnnotations.Schema;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Models;

namespace LMS_Web.Areas.Settings.Models
{
    public class UserStationPermission:BaseClass
    {
        public int Id { get; set; }
        [ForeignKey("AppUserId")]
        public string AppUserId { get; set; }
        [ForeignKey("StationId")]
        public int StationId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual Station Station { get; set; }
    }
}
