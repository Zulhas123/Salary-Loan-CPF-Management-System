using System.Collections.Generic;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Interface.Manager;
using LMS_Web.Models;

namespace LMS_Web.Areas.Settings.Interface
{
    interface IGradeWiseFixedDeductionManager : IBaseManager<GradeWiseFixedDeduction>
    {
        ICollection<GradeWiseFixedDeduction> GetAll();
        ICollection<GradeWiseFixedDeduction> GetAllIncluded();
        GradeWiseFixedDeduction GetById(int id);
        GradeWiseFixedDeduction CheckUnique(int deductionId, int fromGrade, int toGrade);
    }
}
