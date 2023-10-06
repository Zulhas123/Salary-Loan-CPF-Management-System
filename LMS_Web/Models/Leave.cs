using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_Web.Models
{
    public class Leave : BaseClass
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "ছুটির ধরন")]
        [ForeignKey("VacationType")]
        public int LeaveTypeId { get; set; }


        [Display(Name = "টাইটেল")]
        public string Name { get; set; }



        [DataType(DataType.Date)]

        [Display(Name = "শুরুর দিন")]
        public DateTime? FromDate { get; set; }



        [Display(Name = "শেষ দিন")]

        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }


        [Display(Name = "কারণ")]
        public string Reason { get; set; }


        [Display(Name = "নোট")]
        public string Detail { get; set; }


        [Display(Name = "ছুটির ধরন")]
        public LeaveType LeaveType { get; set; }



        [Display(Name = "ছুটির সংখ্যা")]
        public int LeaveQuota { get; set; }
    }
}
