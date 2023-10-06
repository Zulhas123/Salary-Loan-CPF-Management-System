using System.Collections.Generic;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Areas.Salary.ViewModels;
using LMS_Web.Interface.Manager;

namespace LMS_Web.Areas.Salary.Interface.Manager
{
   interface ISalaryManager : IBaseManager<SalaryVM>
   {
       ICollection<SalaryVM> GetList();
       SalaryVM GetById(int id);
    }
}
