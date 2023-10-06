using System.Collections.Generic;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Areas.Settings.Interface;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Models;
using LMS_Web.Repository;

namespace LMS_Web.Areas.Settings.Manager
{
    public class TransferHistoryManager : BaseManager<TransferHistory>, ITransferHistoryManager

    {
        public TransferHistoryManager(ApplicationDbContext db) : base(new BaseRepository<TransferHistory>(db))
        {
        }

        public ICollection<TransferHistory> getNewTransferHistories(int year, int  month)
        {
            return Get(x => x.TransferDate.Year == year && x.TransferDate.Month == month,c=>c.FromStation);
        }

        public TransferHistory GetById(int id)
        {
            return GetFirstOrDefault(c => c.Id == id);
        }

        public ICollection<TransferHistory> GetList()
        {
            return Get(c => true,d=>d.AppUser);
        }
    }
}
