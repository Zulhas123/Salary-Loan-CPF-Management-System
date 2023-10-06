using System.Collections.Generic;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Interface.Manager;
using LMS_Web.Models;

namespace LMS_Web.Areas.Settings.Interface
{
    interface IUserTaxManager:IBaseManager<UserTax>
    {
        UserTax GetByUserId(string userId, int fiscalYearId);
        UserTax GetByTaxUserId(int id);
        ICollection<UserTax> GetTaxesThisFiscalYear(int fiscalYearId);
        ICollection<UserTax> TotalTaxAmount();

    }
}
