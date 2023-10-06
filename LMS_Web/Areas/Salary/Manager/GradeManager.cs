using System.Collections.Generic;
using System.Linq;
using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Repository;

namespace LMS_Web.Areas.Salary.Manager
{
    public class GradeManager:BaseManager<Grade>,IGradeManager
    {
        public GradeManager(ApplicationDbContext db) : base(new BaseRepository<Grade>(db))
        {
        }
        public ICollection<Grade> GetList()
        {
            return Get(c => true);
        }
    }
}
