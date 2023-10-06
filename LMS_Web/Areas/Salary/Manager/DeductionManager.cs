using System.Collections.Generic;
using System.Linq;
using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Repository;

namespace LMS_Web.Areas.Salary.Manager
{
    public class DeductionManager : BaseManager<Deduction>, IDeductionManager
    {
        public DeductionManager(ApplicationDbContext db) : base(new BaseRepository<Deduction>(db))
        {
        }

        public ICollection<Deduction> GetUserSpecific()
        {
            return Get(c => c.IsUserSpecific);
        }

        public ICollection<Deduction> GetFixedList()
        {
            return Get(c => c.IsFixed);
        }

        public ICollection<Deduction> GetGradeWiseFixedList()
        {
            return Get(c => c.IsGradeWiseFixed);
        }
    }
}
