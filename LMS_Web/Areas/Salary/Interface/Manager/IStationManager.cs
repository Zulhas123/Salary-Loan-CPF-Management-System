using System.Collections.Generic;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Interface.Manager;

namespace LMS_Web.Areas.Salary.Interface.Manager
{
   interface IStationManager : IBaseManager<Station>
   {
       ICollection<Station> GetList();
       Station GetById(int id);
   }
}
