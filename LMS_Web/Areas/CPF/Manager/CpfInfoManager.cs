using LMS_Web.Areas.CPF.Interface;
using LMS_Web.Areas.CPF.Models;
using LMS_Web.Areas.Loan.Interface;
using LMS_Web.Areas.Loan.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Models;
using LMS_Web.Repository;
using System.Collections.Generic;
using System.Linq;

namespace LMS_Web.Areas.CPF.Manager
{
    public class CpfInfoManager : BaseManager<CpfInfo>, ICpfInfoManager
    {
        public CpfInfoManager(ApplicationDbContext db) : base(new BaseRepository<CpfInfo>(db))
        {

        }


        public CpfInfo GetById(int id)
        {
            return GetFirstOrDefault(x => x.Id == id);
        }

        public ICollection<CpfInfo> GetList()
        {
            return Get(c => true, c => c.AppUser);
        }

        public ICollection<CpfInfo> GetListByMonth(int year, int month)
        {
            return Get(e => e.Year == year && e.Month == month);
        }

        public CpfInfo GetListByMonthUser(int year, int month, string appUserId)
        {
            return GetFirstOrDefault(e => e.Year == year && e.Month == month && e.AppUserId == appUserId);
        }

        public ICollection<CpfInfo> GetListByMonthUser(int fyear, int fmonth, int tyear, int tmonth, string appUserId)
        {
            return Get(d =>
      (d.Year > fyear || (d.Year == fyear && d.Month >= fmonth)) &&
      (d.Year < tyear || (d.Year == tyear && d.Month <= tmonth)) && d.AppUserId == appUserId, c=>c.AppUser);
        }
        public ICollection<CpfInfo> GetListByMonth(int fromYear, int fromMonth, int toYear, int toMonth)
        {
            return Get(d =>
       (d.Year > fromYear || (d.Year == fromYear && d.Month >= fromMonth)) &&
       (d.Year < toYear || (d.Year == toYear && d.Month <= toMonth)));

        }


        public ICollection<CpfInfo> GetListByMonthStation(int fromYear, int fromMonth, int toYear, int toMonth, int? StationId, int? WingId, int? FromGradeId, int? ToGradeId)
        {
            return Get(d =>
            (d.Year > fromYear || (d.Year == fromYear && d.Month >= fromMonth)) && (StationId == null || d.AppUser.StationId == StationId) &&
            (WingId == null || d.AppUser.WingId == WingId) && (FromGradeId == null || d.AppUser.GradeId >= FromGradeId)
            && (ToGradeId == null || d.AppUser.GradeId <= ToGradeId) &&
            (d.Year < toYear || (d.Year == toYear && d.Month <= toMonth)));

        }

    }
}
