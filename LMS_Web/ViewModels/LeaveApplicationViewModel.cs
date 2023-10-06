using LMS_Web.Models;
using System.Collections.Generic;

namespace LMS_Web.ViewModels
{
    public class LeaveApplicationViewModel
    {
        public ICollection<LeaveApplication> LeaveApplications { get; set; }
        public LeaveApplication LeaveApplication { get; set; }
        public string ApplicantId { get; set; }

        //[Display(Name = "শুরু")]
        //[DataType(DataType.Date)]
        //public DateTime FromDate { get; set; }

        //[Display(Name = "শেষ")]
        //[DataType(DataType.Date)]
        //public DateTime ToDate { get; set; }
    }
}
