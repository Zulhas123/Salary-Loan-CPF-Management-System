using System.Collections.Generic;
using System.Linq;
using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Repository;

namespace LMS_Web.Areas.Salary.Manager
{
    public class LienUserManager : BaseManager<LienUser>, ILienUserManager
    {
        public LienUserManager(ApplicationDbContext db) : base(new BaseRepository<LienUser>(db))
        {
            
        }

        public LienUser GetByUserId(string userId)
        {
            return GetFirstOrDefault(c => c.AppUserId==userId);
        }

        //public ICollection<LienUser> GetList()
        //{
        //    return Get(c => true);
        //}
    }
}
