using System.Collections.Generic;
using System.Linq;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Areas.Settings.Interface;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Models;
using LMS_Web.Repository;

namespace LMS_Web.Areas.Settings.Manager
{
    public class UserStationPermissionManager : BaseManager<UserStationPermission>, IUserStationPermissionManager

    {
        public UserStationPermissionManager(ApplicationDbContext db) : base(new BaseRepository<UserStationPermission>(db))
        {
        }

        public ICollection<UserStationPermission> GetByUserId(string userId)
        {
            return Get(x =>x.AppUserId==userId,c=>c.Station);
        }

        public ICollection<Station> UserWiseLoadStation(string userId)
        {
            
            List<Station> list = new List<Station>();
            var StationExist = Get(c => c.AppUserId == userId,c=>c.Station);
            foreach (var s in StationExist)
            {
                Station sStation = new Station();
                sStation.Id = s.StationId;
                sStation.Name = s?.Station.Name ?? "";
                sStation.Address = s.Station.Address;
                sStation.StationType = s.Station.StationType;
                list.Add(sStation);
                //list.Add(s.Station);
            }
            return list;
        }

        
    }
}
