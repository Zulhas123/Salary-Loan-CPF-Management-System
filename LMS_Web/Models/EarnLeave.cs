using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LMS_Web.Models
{
    public class EarnLeave
    {
        public long Id { get; set; }
        public string AppUserId { get; set; }
        [ForeignKey("EarnLeaveType")]
        public int Type { get; set; }
        public int Obtain { get; set; }
        public int Balance { get; set; }
        public DateTime LastCalculationDate { get; set; }
        public virtual EarnLeaveType EarnLeaveType { get; set; }
    }
}
