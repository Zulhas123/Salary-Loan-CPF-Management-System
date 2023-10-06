using System;
using System.ComponentModel.DataAnnotations;

namespace LMS_Web.Models
{

    public class EmployeeLeave : BaseClass
    {
        public int Id { get; set; }


        [Display(Name = "কর্মী")]
        public string EmployeeId { get; set; }
        public AppUser Employee { get; set; }


        [Display(Name = "ছুটির ধরণ")]
        [Required]
        public int LeaveTypeId { get; set; }
        public LeaveType Leave { get; set; }



        [Display(Name = "বরাদ্দকৃত ছুটি")]

        [Required]
        public int TotalLeave { get; set; }


        [Display(Name = "ব্যবহৃত ছুটি")]
        [Required]
        public int LeaveTaken { get; set; }

        [Display(Name = "অবশিষ্ট ছটি")]
        [Required]
        public int LeaveBalance { get; set; }


        [Display(Name = "বছর")]

        [DataType(DataType.Date)]
        public DateTime Year { get; set; }
    }
}
