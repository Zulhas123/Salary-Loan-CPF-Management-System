using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_Web.Models
{
    public class LeaveApplication : BaseClass
    {
        public long Id { get; set; }


        [Display(Name = "আবেদনকারী")]
        [ForeignKey("Applicant")]
        public string ApplicantId { get; set; }

        [Display(Name = "ছুটির ধরণ")]
        [Required(ErrorMessage = "আবশ্যিক")]
        public int LeaveTypeId { get; set; }

        [Display(Name = "শুরু")]
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }

        [Display(Name = "শেষ")]
        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }


        [Display(Name = "কারণ")]
        [Required(ErrorMessage = "আবশ্যিক")]
        public string Reason { get; set; }

        [Display(Name = "বিস্তারিত")]
        public string Notes { get; set; }

        [ForeignKey("NextApprovedPerson")]
        [Required(ErrorMessage = "আবশ্যিক")]
        public string NextApprovedPersonId { get; set; }
        public bool IsRejected { get; set; }

        public bool IsApproved { get; set; }
        public string RejectedById { get; set; }
        public DateTime? RejectedDate { get; set; }

        public int EarnLeaveType { get; set; }
        public bool IsHalfToFull { get; set; }
        public int TotalDays { get; set; }
        public string CancellationRemarks { get; set; }

        public bool IsRead { get; set; }

        [Display(Name = "অন্যান্য কারণ")]
        public string OtherReason { get; set; }
        public virtual AppUser RejectedBy { get; set; }
        public virtual AppUser Applicant { get; set; }
        public virtual AppUser NextApprovedPerson { get; set; }
        public virtual LeaveType LeaveType { get; set; }
        public List<AttachedFile> AttachedFiles { get; set; }


        [NotMapped]
        public int TempApprovedPersonId { get; set; }

    }
}
