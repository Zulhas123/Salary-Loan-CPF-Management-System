using System.Collections.Generic;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Interface.Manager;
using LMS_Web.Models;

namespace LMS_Web.Areas.Settings.Interface
{
    interface ITransferHistoryManager : IBaseManager<TransferHistory>
    {
        ICollection<TransferHistory> GetList();
        ICollection<TransferHistory> getNewTransferHistories(int year, int month);
        TransferHistory GetById(int id);
       
    }
}
