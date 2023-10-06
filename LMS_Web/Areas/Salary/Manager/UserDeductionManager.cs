using System.Collections.Generic;
using System.Linq;
using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Repository;

namespace LMS_Web.Areas.Salary.Manager
{
    public class UserDeductionManager:BaseManager<UserDeduction>,IUserDeductionManager
    {
        public UserDeductionManager(ApplicationDbContext db) : base(new BaseRepository<UserDeduction>(db))
        {
        }

        public UserDeduction GetById(int id)
        {
            return GetFirstOrDefault(c => c.Id == id);
        }

        public UserDeduction GetDeductionByInfo(string userId, int deductionId, bool isSameEveryMonth, int month, int year)
        {
            return GetFirstOrDefault(c => c.AppUserId == userId && c.DeductionId == deductionId && (isSameEveryMonth || (c.Month == month && c.Year == year)));
        }

        public ICollection<UserDeduction> GetList()
        {
            return Get(c =>true,c=>c.Deduction, c=>c.AppUser);           
        }

        public ICollection<UserDeduction> GetListByDateRange(int fromYear, int fromMonth, int toYear, int toMonth)
        {
            return Get(d =>
            (d.Year > fromYear || (d.Year == fromYear && d.Month >= fromMonth)) &&
             (d.Year < toYear || (d.Year == toYear && d.Month <= toMonth)), c=> c.AppUser,c=>c.Deduction);
        }

        public ICollection<UserDeduction> GetSameDataInEveryMonth()
        {
            return Get(c => c.IsSameEveryMonth, c => c.AppUser, c => c.Deduction);
        }
    }
}
