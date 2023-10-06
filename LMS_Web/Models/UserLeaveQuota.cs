using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_Web.Models
{
    public class UserLeaveQuota
    {
        public int Id { get; set; }

        [ForeignKey("Employee")]
        public string EmployeeId { get; set; }

        [ForeignKey("LeaveType")]
        public int LeaveTypeId { get; set; }

        [Display(Name = "অবশিষ্ট ছুটি")]
        public int Balance { get; set; }

        [Display(Name = "ছুটি নিয়েছেন")]
        public int LeaveObtain { get; set; }


        [DataType(DataType.Date)]
        public DateTime Year { get; set; }



        public AppUser Employee { get; set; }
        public LeaveType LeaveType { get; set; }

    }
}
