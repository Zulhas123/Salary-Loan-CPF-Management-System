using System.Collections.Generic;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Interface.Manager;

namespace LMS_Web.Areas.Salary.Interface.Manager
{
   interface IProcessSalaryManager : IBaseManager<ProcessSalary>
   {
      
        ProcessSalary GetByMonthYear(int month, int year);
   }
}
