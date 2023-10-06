using LMS_Web.Areas.CPF.Interface;
using LMS_Web.Areas.CPF.Models;
using LMS_Web.Areas.Loan.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Repository;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LMS_Web.Areas.CPF.Manager
{
    public class InvestmentInfoManager : BaseManager<InvestmentInfo>, IInvestmentInfoManager
    {
        public InvestmentInfoManager(ApplicationDbContext db) : base(new BaseRepository<InvestmentInfo>(db))
        {

        }

        public InvestmentInfo GetByMonthAndUserId(int year, int month, string userId)
        {
            return GetFirstOrDefault(c => c.AppUserId == userId && c.Month == month && c.Year == year);
        }

        public ICollection<InvestmentInfo> GetList()
        {
            return Get(c => true, c => c.AppUser);


        }

        public ICollection<InvestmentInfo> GetListByMonth(int year, int month)
        {
            return Get(e => e.Year == year && e.Month == month);
        }


        //public ICollection<InvestmentInfo> GetListForCurrentFiscal(int fyear, int fmonth, int tyear, int tmonth)
        //{
        //    return Get(c => (c.Year >= fyear || c.Year <= tyear) && (c.Month >= fmonth || c.Month <= tmonth));
        //}


        public ICollection<InvestmentInfo> GetListByMonthUser(int fromYear, int fromMonth, int toYear, int toMonth, string appUserId)
        {
            return Get(d => d.AppUserId==appUserId &&
       (d.Year > fromYear || (d.Year == fromYear && d.Month >= fromMonth)) &&
       (d.Year < toYear || (d.Year == toYear && d.Month <= toMonth)));

            //  return Get(c => (c.Year >= fyear || c.Year <=tyear) && (c.Month>=fmonth || c.Month<=tmonth) && c.AppUserId == appUserId);

        }
    }
}
