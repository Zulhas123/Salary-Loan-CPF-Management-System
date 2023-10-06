using System.Collections.Generic;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Interface.Manager;

namespace LMS_Web.Areas.Settings.Interface
{
    interface IRoleSubMenuManager:IBaseManager<RoleSubMenu>
    {
        ICollection<RoleSubMenu> GetByRoleId(string roleId);
        ICollection<RoleSubMenu> GetAll();
        
    }
}
