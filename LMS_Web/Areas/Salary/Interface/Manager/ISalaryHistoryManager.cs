using System.Collections.Generic;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Interface.Manager;
using LMS_Web.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace LMS_Web.Areas.Salary.Interface.Manager
{
   interface ISalaryHistoryManager:IBaseManager<SalaryHistory>
   {
       ICollection<SalaryHistory> GetListByMonthYear(int year, int month, List<AppUser> users, bool isBanglaRequired);
   }
}
