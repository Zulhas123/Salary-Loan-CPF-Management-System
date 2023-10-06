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
    public class StationManager : BaseManager<Station>, IStationManager

    {
        public StationManager(ApplicationDbContext db) : base(new BaseRepository<Station>(db))
        {
        }

       
        public ICollection<Station> GetAll()
        {
            return Get(x => true);
        }
    }
}
