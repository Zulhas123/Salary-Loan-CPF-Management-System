using System.Collections.Generic;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Models;

namespace LMS_Web.Interface.Manager
{
    interface IWingsManager : IBaseManager<Wing>
    {
        ICollection<Wing> GetWings();
        Wing GetWingById(int? id);
    }
}
