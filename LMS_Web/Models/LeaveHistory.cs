using System;

namespace LMS_Web.Models
{
    public class LeaveHistory
    {
        public long Id { get; set; }
        public long? LeaveId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
