using System.Collections.Generic;
using LMS_Web.Areas.Settings.Interface;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Repository;

namespace LMS_Web.Areas.Settings.Manager
{
    public class MainMenuManager:BaseManager<MainMenu>,IMainMenuManager
    {
        public MainMenuManager(ApplicationDbContext db) : base(new BaseRepository<MainMenu>(db))
        {
        }

        public ICollection<MainMenu> GetAll()
        {
            return Get(x => true);
        }
    }
}
