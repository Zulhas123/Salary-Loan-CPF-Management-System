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
    public class LoanPaymentHistoryManager : BaseManager<LoanPaymentHistory>, ILoanPaymentHistoryHeadManager
    {
        public LoanPaymentHistoryManager(ApplicationDbContext db):base(new BaseRepository<LoanPaymentHistory>(db))
        {

        }

        public ICollection<LoanPaymentHistory> GetLoanPaymentHistory(string appuserId)
        {
            return Get(c =>c.AppUserId==appuserId, c=>c.UserWiseLoan,c=>c.UserWiseLoan.LoanHeads);
        }

        public LoanPaymentHistory GetById(int id)
        {
            return GetFirstOrDefault(x => x.Id == id);
        }
    }
}
