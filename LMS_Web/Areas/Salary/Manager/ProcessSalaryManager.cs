using System.Collections.Generic;
using System.Linq;
using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Repository;

namespace LMS_Web.Areas.Salary.Manager
{
    public class ProcessSalaryManager : BaseManager<ProcessSalary>, IProcessSalaryManager
    {
        public ProcessSalaryManager(ApplicationDbContext db) : base(new BaseRepository<ProcessSalary>(db))
        {
        }

        public ProcessSalary GetByMonthYear(int month, int year)
        { 
            var data= GetFirstOrDefault(c => c.Month == month && c.Year == year && c.IsFinal);
            if(data != null)
            {
                return data;
            }
           data= GetFirstOrDefault(c => c.Month == month && c.Year == year);
            return data;
        }
    }
}
