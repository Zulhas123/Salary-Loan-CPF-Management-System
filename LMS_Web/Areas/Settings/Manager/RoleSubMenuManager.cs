using System.Collections.Generic;
using LMS_Web.Areas.Settings.Interface;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Repository;

namespace LMS_Web.Areas.Settings.Manager
{
    public class RoleSubMenuManager:BaseManager<RoleSubMenu>,IRoleSubMenuManager
    {
        public RoleSubMenuManager(ApplicationDbContext db) : base(new BaseRepository<RoleSubMenu>(db))
        {
        }

        public ICollection<RoleSubMenu> GetByRoleId(string roleId)
        {
            return Get(x => x.RoleId == roleId);
        }

        public ICollection<RoleSubMenu> GetAll()
        {
            return Get(x => true);
        }
    }
}
