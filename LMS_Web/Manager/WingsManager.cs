using System.Collections.Generic;
using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Data;
using LMS_Web.Interface.Manager;
using LMS_Web.Models;
using LMS_Web.Repository;

namespace LMS_Web.Manager
{
    public class WingsManager : BaseManager<Wing>, IWingsManager
    {
        public WingsManager(ApplicationDbContext db) : base(new BaseRepository<Wing>(db))
        {
        }

        public Wing GetWingById(int? id)
        {
            return GetFirstOrDefault(x => x.Id==id);
        }

        public ICollection<Wing> GetWings()
        {
            return Get(x => x.IsActive);
        }
    }
}
