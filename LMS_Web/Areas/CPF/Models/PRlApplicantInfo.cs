using LMS_Web.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_Web.Areas.CPF.Models
{
    public class PRlApplicantInfo
    {
        public int Id { get; set; }
        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime PrlDate { get; set; }
        public bool IsApproved { get; set; }
        public DateTime? ApproveDate { get; set; }

    }
}
