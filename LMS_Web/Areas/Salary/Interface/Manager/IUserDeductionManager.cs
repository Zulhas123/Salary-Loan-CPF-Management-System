using System.Collections.Generic;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Interface.Manager;

namespace LMS_Web.Areas.Salary.Interface.Manager
{
   interface IUserDeductionManager:IBaseManager<UserDeduction>
   {
       ICollection<UserDeduction> GetList();
        UserDeduction GetById(int id);
        UserDeduction GetDeductionByInfo(string userId, int deductionId, bool isSameEveryMonth, int month, int year);
        ICollection<UserDeduction> GetListByDateRange(int fromYear, int fromMonth, int toYear, int toMonth);
        ICollection<UserDeduction> GetSameDataInEveryMonth();
    }
}
