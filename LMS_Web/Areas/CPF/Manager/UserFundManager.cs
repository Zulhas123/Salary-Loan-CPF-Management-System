using LMS_Web.Areas.CPF.Interface;
using LMS_Web.Areas.CPF.Models;
using LMS_Web.Areas.Loan.Interface;
using LMS_Web.Areas.Loan.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Models;
using LMS_Web.Repository;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Web.Areas.CPF.Manager
{
    public class UserFundManager : BaseManager<UserFundInfo>, IUserFundManager
    {
        public UserFundManager(ApplicationDbContext db):base(new BaseRepository<UserFundInfo>(db))
        {

        }
        public UserFundInfo GetById(int id)
        {
            return GetFirstOrDefault(x => x.Id == id);
        }
        public ICollection<UserFundInfo> GetBetweenGrade( int year, int month)
        {
            return Get(c => c.Year == year && c.Month == month);
        }
      

        public ICollection<UserFundInfo> GetList()
        {
            return Get(c => true);
        }

        public UserFundInfo GetByUserAndMonth(string appuserId, int month, int year)
        {
            return GetFirstOrDefault(c => c.AppUserId==appuserId && c.Year == year && c.Month == month);
        }
        public ICollection<UserFundInfo> GetFundDataForSpecificMonth(int StationId, int? WingId, int? FromGradeId, int? ToGradeId, int year, int month)
        {
            return Get(c => c.Year == year && c.Month == month && c.AppUser.StationId == StationId && (WingId == null || c.AppUser.WingId == WingId) && (FromGradeId == null || c.AppUser.GradeId >= FromGradeId) && (ToGradeId == null || c.AppUser.GradeId <= ToGradeId), C => C.AppUser, C => C.AppUser.Designation);

        }

        public ICollection<UserFundInfo> GetBetweenMonthRange(int stationId, int? wingId, int? fromGradeId, int? toGradeId, int fYear, int fMonth, int tYear, int tMonth)
        {
            return Get(d =>
            (d.Year > fYear || (d.Year == fYear && d.Month >= fMonth)) &&  d.AppUser.StationId == stationId &&
            (wingId == null || d.AppUser.WingId == wingId) && (fromGradeId == null || d.AppUser.GradeId >= fromGradeId)
            && (toGradeId == null || d.AppUser.GradeId <= toGradeId) &&
            (d.Year < tYear || (d.Year == tYear && d.Month <= tMonth)));
        }
    }
}
