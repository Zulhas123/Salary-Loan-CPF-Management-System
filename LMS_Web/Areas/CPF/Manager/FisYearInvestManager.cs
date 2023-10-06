using LMS_Web.Areas.CPF.Interface;
using LMS_Web.Areas.CPF.Models;
using LMS_Web.Areas.Loan.Interface;
using LMS_Web.Areas.Loan.Models;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Models;
using LMS_Web.Repository;
using System.Collections.Generic;
using System.Linq;

namespace LMS_Web.Areas.CPF.Manager
{
    public class FisYearInvestManager: BaseManager<FiscalYearWiseInvestmentInfo>, IFisYearInvestManager
    {
        public FisYearInvestManager(ApplicationDbContext db) : base(new BaseRepository<FiscalYearWiseInvestmentInfo>(db))
        {
            
        }

        public FiscalYearWiseInvestmentInfo GetByUserAndFiscalYear(int fiscalYear, string userId)
        {
           return GetFirstOrDefault(c=>c.FiscalYearId==fiscalYear && c.AppUserId==userId);
        }

        public ICollection<FiscalYearWiseInvestmentInfo> GetListByFiscalYear(int fiscalYear)
        {
            return Get(e => e.FiscalYearId == fiscalYear,e=> e.AppUser);
        }

        public ICollection<FiscalYearWiseInvestmentInfo> GetList()
        {
            return Get(c => true, c => c.AppUser,c=>c.FiscalYear);
        }

    }
}
