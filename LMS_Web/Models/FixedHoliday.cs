using System.ComponentModel.DataAnnotations;

namespace LMS_Web.Models
{
    public class FixedHoliday : BaseClass
    {
        public int Id { get; set; }


        [Display(Name = "ছুটির বিষয়")]
        public string Name { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }


        [Display(Name = "রিমার্কস")]
        public string Remarks { get; set; }
    }
}