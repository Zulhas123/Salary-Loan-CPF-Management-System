using System.ComponentModel.DataAnnotations;

namespace LMS_Web.Models
{
    public class LeaveType : BaseClass
    {
        public int Id { get; set; }


        [Display(Name = "টাইটেল")]
        public string Name { get; set; }


        //[Display(Name = "ছুটির সংখ্যা")]
        //public int Quota { get; set; }

    }
}