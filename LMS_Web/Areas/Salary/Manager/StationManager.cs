using System.Collections.Generic;
using System.Linq;
using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Areas.Settings.Manager;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Areas.Settings.ViewModels;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Web.Areas.Salary.Manager
{
    public class StationManager : BaseManager<Station>, IStationManager
    {
        public StationManager(ApplicationDbContext db) : base(new BaseRepository<Station>(db))
        {
            
        }

        public Station GetById(int id)
        {
            return GetFirstOrDefault(c => c.Id == id);
        }

        public ICollection<Station> GetList()
        {
            return Get(c => true);
        }


        



    }
}
