using System.Collections.Generic;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Interface.Manager;

namespace LMS_Web.Areas.Salary.Interface.Manager
{
   interface IGradeWisePayScaleManager : IBaseManager<GradeWisePayScale>
   {
       ICollection<GradeWisePayScale> GetList();
       GradeWisePayScale GetById(int id);
    }
}
