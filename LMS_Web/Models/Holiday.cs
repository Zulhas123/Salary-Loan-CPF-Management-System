using System;
using System.ComponentModel.DataAnnotations;

namespace LMS_Web.Models
{
    public class Holiday
    {
        public int Id { get; set; }


        [Display(Name = "ছুটির বিষয়")]
        [Required(ErrorMessage = "আবশ্যিক")]
        public string Name { get; set; }


        [DataType(DataType.Date)]

        [Display(Name = "শুরুর দিন")]
        [Required(ErrorMessage = "আবশ্যিক")]
        public DateTime FromDate { get; set; }



        [Display(Name = "শেষ দিন")]
        [Required(ErrorMessage = "আবশ্যিক")]
        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }



        [Display(Name = "রিমার্কস")]
        public string Remarks { get; set; }
    }
}
