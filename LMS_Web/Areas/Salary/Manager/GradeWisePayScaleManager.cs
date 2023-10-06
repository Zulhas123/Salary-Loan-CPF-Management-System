using System.Collections.Generic;
using System.Linq;
using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Repository;

namespace LMS_Web.Areas.Salary.Manager
{
    public class GradeWisePayScaleManager : BaseManager<GradeWisePayScale>, IGradeWisePayScaleManager
    {
        public GradeWisePayScaleManager(ApplicationDbContext db) : base(new BaseRepository<GradeWisePayScale>(db))
        {
        }

        public ICollection<GradeWisePayScale> GetList()
        {
            var list = Get(c => true, c => c.PayScale, c => c.Grade);
            return list;
        }     
        public GradeWisePayScale GetById(int id)
        {
            return GetFirstOrDefault(x => x.Id == id);
        }
    }
}
