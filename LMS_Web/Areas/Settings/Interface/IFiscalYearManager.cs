using System.Collections.Generic;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Interface.Manager;
using LMS_Web.Models;

namespace LMS_Web.Areas.Settings.Interface
{
    interface IFiscalYearManager:IBaseManager<FiscalYear>
    {
        ICollection<FiscalYear> GetList();
        FiscalYear GetByValue(string value);
        FiscalYear GetById(int  id);
    }
}
