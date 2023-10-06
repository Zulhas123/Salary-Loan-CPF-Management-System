using LMS_Web.Areas.Salary.Enum;

namespace LMS_Web.Areas.Salary.Models
{
    public class HouseRentRules
    {
        public int Id { get; set; }
        public StationType StationType { get; set; }
        public decimal BasicFrom { get; set; }
        public decimal BasicTo { get; set; }
        public decimal Percentage { get; set; }
        public decimal MinimumAmount { get; set; }
    }
}
