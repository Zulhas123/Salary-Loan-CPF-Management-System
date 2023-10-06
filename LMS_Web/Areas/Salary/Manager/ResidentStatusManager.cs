using System.Collections.Generic;
using System.Linq;
using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Repository;

namespace LMS_Web.Areas.Salary.Manager
{
    public class ResidentStatusManager : BaseManager<ResidentStatus>,IResidentStatusManager
    {
        public ResidentStatusManager(ApplicationDbContext db) : base(new BaseRepository<ResidentStatus>(db))
        {
        }

        public ResidentStatus GetById(int id)
        {
            return GetFirstOrDefault(c => c.Id == id);
        }

        public ICollection<ResidentStatus> GetList()
        {
            return Get(c =>c.IsUserSpecific);
        }
    }
}
