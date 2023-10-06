using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Repository;

namespace LMS_Web.Areas.Salary.Manager
{
    public class SalaryStepInfoManager: BaseManager<SalaryIncrement>, ISalaryStepInfoManager
    {
        public SalaryStepInfoManager(ApplicationDbContext db) : base(new BaseRepository<SalaryIncrement>(db))
        {

        }

        public SalaryIncrement GetByMonthYear(int month, int year)
        {
            
           var data = GetFirstOrDefault(c => c.Month == month && c.Year == year);
           return data;
        }
    }
}
