using System;
using System.ComponentModel.DataAnnotations.Schema;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Models;

namespace LMS_Web.Areas.Settings.Models
{
    public class TransferHistory:BaseClass
    {
        public int Id { get; set; }
        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }
        [ForeignKey("Station")]
        public int FromStationId { get; set; }
        [ForeignKey("Station")]
        public int ToStationId { get; set; }
        public DateTime TransferDate { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual Station FromStation { get; set; }
        public virtual Station ToStation { get; set; }
    }
}
