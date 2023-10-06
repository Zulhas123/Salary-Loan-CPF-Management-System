using System.Collections.Generic;
using System.Linq;
using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Repository;

namespace LMS_Web.Areas.Salary.Manager
{
    public class BonusManager : BaseManager<Bonus>, IBonusManager
    {
        public BonusManager(ApplicationDbContext db) : base(new BaseRepository<Bonus>(db))
        {
        }

        public Bonus GetById(int id)
        {
            return GetFirstOrDefault(c=>c.Id == id);
        }

        public ICollection<Bonus> GetList()
        {
            return Get(c => true);
        }
    }
}
