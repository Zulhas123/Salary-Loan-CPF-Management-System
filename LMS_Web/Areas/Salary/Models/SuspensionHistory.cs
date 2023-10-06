using System;
using LMS_Web.Models;

namespace LMS_Web.Areas.Salary.Models
{
    public class SuspensionHistory:BaseClass
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public AppUser AppUser { get; set; }    
    }
}
