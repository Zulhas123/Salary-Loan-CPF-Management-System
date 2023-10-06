using LMS_Web.Areas.Loan.Interface;
using LMS_Web.Areas.Loan.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Web.Areas.Loan.Manager
{
    public class InterestSlabManager : BaseManager<InterestSlab>, IInterestSlabManager
    {
        public InterestSlabManager(ApplicationDbContext db):base(new BaseRepository<InterestSlab>(db))
        {

        }

        public InterestSlab GetById(int id)
        {
            return GetFirstOrDefault(c => c.Id == id);
        }

        public InterestSlab GetInterest(decimal amount)
        {
            return GetFirstOrDefault(f=>amount >= f.FromAmount && amount <= f.ToAmount);
        }
    }
}
