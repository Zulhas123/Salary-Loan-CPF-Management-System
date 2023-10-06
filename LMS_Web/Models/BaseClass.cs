using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace LMS_Web.Models
{
    public class BaseClass
    {

        [Display(Name = "সংরক্ষণ করেছেন")]
        [ForeignKey("CreatedBy")]
        public string CreatedById { get; set; }
        public AppUser CreatedBy { get; set; }



        [Display(Name = "সংরক্ষণের সময়")]
        public DateTime CreatedDateTime { get; set; }


        [Display(Name = "হালনাগাদ করেছেন")]
        [ForeignKey("UpdatedBy")]
        public string UpdatedById { get; set; }
        public AppUser UpdatedBy { get; set; }

        [Display(Name = "হালনাগাদের সময়")]
        public DateTime? UpdatedDateTime { get; set; }
        
    }
}
