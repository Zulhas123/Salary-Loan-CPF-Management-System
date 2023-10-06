using System.Collections.Generic;
using System.Linq;
using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Models;
using LMS_Web.Repository;
using Microsoft.EntityFrameworkCore.Query;

namespace LMS_Web.Areas.Salary.Manager
{
    public class SalaryHistoryManager:BaseManager<SalaryHistory>, ISalaryHistoryManager
    {
        public SalaryHistoryManager(ApplicationDbContext db) : base(new BaseRepository<SalaryHistory>(db))
        {
        }

        public ICollection<SalaryHistory> GetListByMonthYear(int year, int month, List<AppUser> users, bool isBanglaRequired)
        {
            List<string> userIdsList = users.Select(u => u.Id).ToList();
            return Get(c => c.Year==year && c.Month==month && userIdsList.Contains(c.UserId)).OrderBy(v=>userIdsList.IndexOf(v.UserId)).ToList();
        }
    }
}
