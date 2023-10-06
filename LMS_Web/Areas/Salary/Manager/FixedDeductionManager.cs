using System.Collections.Generic;
using System.Linq;
using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Repository;

namespace LMS_Web.Areas.Salary.Manager
{
    public class FixedDeductionManager : BaseManager<FixedDeduction>, IFixedDeductionManager
    {
        public FixedDeductionManager(ApplicationDbContext db) : base(new BaseRepository<FixedDeduction>(db))
        {
        }

        public ICollection<FixedDeduction> GetList()
        {
            return Get(c => true, c=>c.Deduction);
        }

        public FixedDeduction GetById(int id)
        {
            var data = GetFirstOrDefault(c => c.Id == id);
            return data;
        }

        public FixedDeduction GetByDeductionId(int deductionId)
        {
            var data = GetFirstOrDefault(c => c.DeductionId == deductionId);
            return data;
        }
    }
}
