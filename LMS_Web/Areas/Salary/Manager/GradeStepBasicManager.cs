using System.Collections.Generic;
using System.Linq;
using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Repository;

namespace LMS_Web.Areas.Salary.Manager
{
    public class GradeStepBasicManager : BaseManager<GradeStepBasic>, IGradeStepBasicManager
    {
        public GradeStepBasicManager(ApplicationDbContext db) : base(new BaseRepository<GradeStepBasic>(db))
        {
        }

        public GradeStepBasic GetById(int id)
        {
            return GetFirstOrDefault(c => c.GradeId == id);
        }    

        public ICollection<GradeStepBasic> GetList(int gradeId)
        {
            return Get(c=>c.GradeId==gradeId);
        }

        public GradeStepBasic GetGradeByStep(int gardeId, decimal basic)
        {
            return GetFirstOrDefault(c => c.GradeId == gardeId && c.Amount == basic);
        }
        public GradeStepBasic GetByStep(int gardeId, int stepNo)
        {
            return GetFirstOrDefault(c => c.GradeId == gardeId && c.StepNo == stepNo);
        }

        public ICollection<GradeStepBasic> GetListAllStep()
        {
            return Get(c => true);
        }
    }
}
