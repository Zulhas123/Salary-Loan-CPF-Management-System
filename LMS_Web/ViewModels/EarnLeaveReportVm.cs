using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Web.ViewModels
{
    public class EarnLeaveReportVm
    {
        public string Reason { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int TotalDays { get; set; }
        public int EarnLeaveId { get; set; }
        public string EarnLeaveType { get; set; }
        public int  Balance { get; set; }
    }
}
