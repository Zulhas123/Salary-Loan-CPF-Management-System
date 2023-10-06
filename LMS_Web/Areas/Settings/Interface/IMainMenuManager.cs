using System.Collections.Generic;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Interface.Manager;

namespace LMS_Web.Areas.Settings.Interface
{
    interface IMainMenuManager:IBaseManager<MainMenu>
    {
        ICollection<MainMenu> GetAll();
    }
}
