using LMS_Web.Models;

namespace LMS_Web.Areas.Salary.Models
{
    public class GradeWisePayScale:BaseClass
    {
        public int Id { get; set; }
        public int GradeId { get; set; }
        public int PayScaleId { get; set; }
        public bool IsFixed { get; set; }
        public decimal? FixedAmount { get; set; }
        public decimal? Percentage { get; set; }
        public decimal? MinimumAmount { get; set; }
        public decimal? MaximumAmount { get; set; }

        public virtual Grade Grade { get; set; }
        public virtual PayScale PayScale { get; set; }
    }
}
