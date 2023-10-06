using System.Collections.Generic;
using System.Linq;
using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Repository;

namespace LMS_Web.Areas.Salary.Manager
{
    public class PayScaleManager:BaseManager<PayScale>, IPayScaleManager
    {
        public PayScaleManager(ApplicationDbContext db) : base(new BaseRepository<PayScale>(db))
        {
        }

        public ICollection<PayScale> GetList()
        {
            return Get(c => true);
        }public ICollection<PayScale> GetUserSpecific()
        {
            return Get(c => c.IsUserSpecific);
        }
    }
}
