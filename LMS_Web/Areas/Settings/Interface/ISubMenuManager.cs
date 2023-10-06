using System.Collections.Generic;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Interface.Manager;
using LMS_Web.Models;

namespace LMS_Web.Areas.Settings.Interface
{
    interface ISubMenuManager:IBaseManager<SubMenu>
    {
        ICollection<SubMenu> GetAll();
    }
}
