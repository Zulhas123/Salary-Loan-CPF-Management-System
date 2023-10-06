using System.Collections.Generic;
using System.Linq;
using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Repository;

namespace LMS_Web.Areas.Salary.Manager
{
    public class UserHouseRentManager : BaseManager<UserHouseRent>,IUserHouseRentManager
    {
        public UserHouseRentManager(ApplicationDbContext db) : base(new BaseRepository<UserHouseRent>(db))
        {
        }

        public UserHouseRent GetByEmployeeId(string id)
        {
            return GetFirstOrDefault(c => c.AppUserId == id);
        }

        public UserHouseRent GetById(int id)
        {
            return GetFirstOrDefault(c => c.Id == id);
        }

        public ICollection<UserHouseRent> GetList()
        {
            return Get(c => true,c=>c.AppUser, c=>c.ResidentStatus);
        }

        


    }
}
