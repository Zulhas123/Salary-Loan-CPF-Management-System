using System.Collections.Generic;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Areas.Settings.Interface;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Models;
using LMS_Web.Repository;

namespace LMS_Web.Areas.Settings.Manager
{
    public class GradeWiseFixedDeductionManager : BaseManager<GradeWiseFixedDeduction>, IGradeWiseFixedDeductionManager

    {
        public GradeWiseFixedDeductionManager(ApplicationDbContext db) : base(new BaseRepository<GradeWiseFixedDeduction>(db))
        {
        }

       
        public ICollection<GradeWiseFixedDeduction> GetAll()
        {
            return Get(x => true);
        }

        public ICollection<GradeWiseFixedDeduction> GetAllIncluded()
        {
            return Get(x => true, c => c.FromGrade, c => c.ToGrade, c => c.Deduction);
        }

        public GradeWiseFixedDeduction GetById(int id)
        {
            return GetFirstOrDefault(c => c.Id == id);
        }

        public GradeWiseFixedDeduction CheckUnique(int deductionId, int fromGrade, int toGrade)
        {
            return GetFirstOrDefault(c =>
                c.DeductionId == deductionId && c.FromGradeId == fromGrade && c.ToGradeId == toGrade);
        }
    }
}
