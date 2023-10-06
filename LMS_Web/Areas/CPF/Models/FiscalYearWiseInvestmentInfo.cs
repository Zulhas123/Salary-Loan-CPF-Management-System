using LMS_Web.Areas.Settings.Models;
using LMS_Web.Models;

namespace LMS_Web.Areas.CPF.Models
{
    public class FiscalYearWiseInvestmentInfo
    {
        public int Id { get; set; }
        public int FiscalYearId { get; set; }
        public string AppUserId { get; set; }
        public decimal InvestmentAmount { get; set; }
        public decimal InterestAmount { get; set; }
        public decimal Total { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public AppUser AppUser { get; set; }

    }
}
