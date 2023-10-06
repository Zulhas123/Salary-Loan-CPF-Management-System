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
    public class ChildrenInfoManager : BaseManager<ChildrenInfo>, IChildrenInfoManager

    {
        public ChildrenInfoManager(ApplicationDbContext db) : base(new BaseRepository<ChildrenInfo>(db))
        {
        }

        public ChildrenInfo GetById(int id)
        {
            return GetFirstOrDefault(c => c.Id == id);
        }

        public ICollection<ChildrenInfo> GetList()
        {
            return Get(c => true,d=>d.AppUser);
        }

        public ICollection<ChildrenInfo> GetUserChildrenList(string userId)
        {
            return Get(c => c.AppUserId == userId, c=>c.AppUser);
        }
    }
}
