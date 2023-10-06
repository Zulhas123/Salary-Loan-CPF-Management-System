using System.Collections.Generic;
using LMS_Web.Areas.Settings.Interface;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Models;
using LMS_Web.Repository;

namespace LMS_Web.Areas.Settings.Manager
{
    public class SubMenuManager:BaseManager<SubMenu>,ISubMenuManager

    {
        public SubMenuManager(ApplicationDbContext db) : base(new BaseRepository<SubMenu>(db))
        {
        }

        public ICollection<SubMenu> GetAll()
        {
            return Get(x=>true);
        }
    }
}
