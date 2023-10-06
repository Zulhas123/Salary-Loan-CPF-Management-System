using System.Collections.Generic;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Interface.Manager;

namespace LMS_Web.Areas.Salary.Interface.Manager
{
   interface IUserHouseRentManager : IBaseManager<UserHouseRent>
   {
        ICollection<UserHouseRent> GetList();
        UserHouseRent GetById(int id);
        UserHouseRent GetByEmployeeId(string id);
    }
}
