using System.Collections.Generic;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Interface.Manager;

namespace LMS_Web.Areas.Salary.Interface.Manager
{
   interface IFixedDeductionManager : IBaseManager<FixedDeduction>
   {
       ICollection<FixedDeduction> GetList();
      FixedDeduction GetById(int id);
      FixedDeduction GetByDeductionId(int deductionId);
      
   }
}
