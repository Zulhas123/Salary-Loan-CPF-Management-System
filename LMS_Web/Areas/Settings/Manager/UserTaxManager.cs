using System.Collections.Generic;
using System.Linq;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Areas.Settings.Interface;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Models;
using LMS_Web.Repository;

namespace LMS_Web.Areas.Settings.Manager
{
    public class UserTaxManager : BaseManager<UserTax>, IUserTaxManager
    {
        public UserTaxManager(ApplicationDbContext db) : base(new BaseRepository<UserTax>(db))
        {
        }

        public UserTax GetByTaxUserId(int id)
        {
            return GetFirstOrDefault(c => c.Id == id);
        }

        public UserTax GetByUserId(string userId, int fiscalYearId)
        {
            return GetFirstOrDefault(c => c.AppUserId == userId && c.FiscalYearId==fiscalYearId);
        }

        public ICollection<UserTax> GetTaxesThisFiscalYear(int fiscalYearId)
        {
            return Get(c => c.FiscalYearId == fiscalYearId);
        }

        public ICollection<UserTax> TotalTaxAmount()
        {
            return Get(c => true, c => c.TotalAmount);
        }
    }
}
