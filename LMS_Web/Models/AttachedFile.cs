using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_Web.Models
{
    public class AttachedFile
    {
        public long Id { get; set; }

        [ForeignKey("LeaveApplication")]
        public long LeaveId { get; set; }
        public LeaveApplication LeaveApplication { get; set; }

        public string DocumentPath { get; set; }

        [NotMapped]
        public IFormFile Document { get; set; }
    }
}
