using System.Collections.Generic;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Interface.Manager;

namespace LMS_Web.Areas.Salary.Interface.Manager
{
   interface IResidentStatusManager : IBaseManager<ResidentStatus>
   {
        ICollection<ResidentStatus> GetList();
        ResidentStatus GetById(int id);
    }
}
