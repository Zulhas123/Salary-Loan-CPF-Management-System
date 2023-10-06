using System.Collections.Generic;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Interface.Manager;

namespace LMS_Web.Areas.Salary.Interface.Manager
{
   interface IPayScaleManager:IBaseManager<PayScale>
   {
       ICollection<PayScale> GetList();
       ICollection<PayScale> GetUserSpecific();
   }
}
