using LMS_Web.Areas.Loan.Models;
using LMS_Web.Areas.Settings.Interface;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Models;
using LMS_Web.Repository;
using System.Collections.Generic;
using System.Linq;

namespace LMS_Web.Areas.Settings.Manager
{
    public class TaxInstallmentInfoManager : BaseManager<TaxInstallmentInfo>, ITaxInstallmentInfoManager
    {
        public TaxInstallmentInfoManager(ApplicationDbContext db) : base(new BaseRepository<TaxInstallmentInfo>(db))
        {

        }
        public ICollection<TaxInstallmentInfo> GetListById(int userTaxId)
        {
            return Get(e => e.UserTaxId == userTaxId);
        }

        public ICollection<TaxInstallmentInfo> GetByMonthYear(int month, int year)
        {
            return Get(c => c.Month == month && c.Year == year);
        }

        public TaxInstallmentInfo GetByMonthYearAndTaxId(int year, int month,int taxId)
        {
            return GetFirstOrDefault(e => e.Year == year && e.Month==month && e.UserTaxId==taxId);
        }

        public ICollection<TaxInstallmentInfo> GetFilteredData(int? FromGradeId, int? ToGradeId, int year, int month, int stationId, int? wingId)
        {
            return Get(c => c.Month == month && c.Year == year && (FromGradeId == null || c.AppUser.GradeId >= FromGradeId) && (ToGradeId == null || c.AppUser.GradeId <= ToGradeId) && c.AppUser.StationId == stationId && (wingId==null || c.AppUser.WingId==wingId), c => c.AppUser, c => c.AppUser.Designation, c => c.UserTax);

        }

        public ICollection<TaxInstallmentInfo> GetUserTaxInstallmentInfo(int userTaxId)
        {
            return Get(c=>c.UserTaxId== userTaxId);
        }

        public TaxInstallmentInfo GetUserSpecificInstallment(string userId, int installmentNo)
        {
            return GetFirstOrDefault(c => c.AppUserId== userId && c.InstallmentNo==installmentNo);
        }

        public TaxInstallmentInfo GetById(int id)
        {
            return GetFirstOrDefault(c => c.Id == id);
        }

        public TaxInstallmentInfo GetCurrentInstalment(string appUserId, int year, int month)
        {
            return GetFirstOrDefault(e => e.AppUserId == appUserId && e.Year == year && e.Month == month);
        }
    }
}