using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_Web.Models
{
    public class EmployeeLeaveBalance
    {
        public int Id { get; set; }


        [ForeignKey("Employee")]
        public string EmployeeId { get; set; }
        public AppUser Employee { get; set; }
        public int LeaveTaken { get; set; }
        public int Leave { get; set; }


        [DataType(DataType.Date)]
        public DateTime Year { get; set; }
    }
}
