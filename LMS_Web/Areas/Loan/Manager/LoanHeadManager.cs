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
    public class LoanHeadManager : BaseManager<LoanHead>, ILoanHeadManager
    {
        public LoanHeadManager(ApplicationDbContext db):base(new BaseRepository<LoanHead>(db))
        {

        }

        public LoanHead GetById(int id)
        {
            return GetFirstOrDefault(c => c.Id == id);
        }  
        public ICollection<LoanHead> GetList()
        {
            return Get(c => true).OrderBy(c=>c.OrderNo).ToList();
        }
    }
}
