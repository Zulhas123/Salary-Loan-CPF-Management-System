using LMS_Web.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_Web.Areas.CPF.Models
{
    public class UserFundInfo
    {
        public int Id { get; set; }
        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }
        public decimal WelfareFund { get; set; }
        public decimal GroupInsurance { get; set; }
        public decimal Rehabilitation { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}
