using System.ComponentModel.DataAnnotations;

namespace LMS_Web.Areas.Salary.Models
{
    public class DueDuringSuspension
    {
        public int Id { get; set; }
        public string AppuserId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        [Required]
        public decimal Amount { get; set; }
    }
}
