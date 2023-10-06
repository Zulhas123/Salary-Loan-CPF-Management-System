using System.Collections.Generic;
using System.Linq;
using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Repository;

namespace LMS_Web.Areas.Salary.Manager
{
    public class HouseRentRuleManager : BaseManager<HouseRentRules>, IHouseRentRuleManager
    {
        public HouseRentRuleManager(ApplicationDbContext db) : base(new BaseRepository<HouseRentRules>(db))
        {
        }

       

        public ICollection<HouseRentRules> GetList()
        {
            return Get(c => true);
        }

        
    }
}
