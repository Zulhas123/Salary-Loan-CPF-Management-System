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
    public class FiscalYearManager : BaseManager<FiscalYear>, IFiscalYearManager

    {
        public FiscalYearManager(ApplicationDbContext db) : base(new BaseRepository<FiscalYear>(db))
        {
        }

        public FiscalYear GetById(int id)
        {
           return  GetFirstOrDefault(c => c.Id == id);
        }

        public FiscalYear GetByValue(string value)
        {
            return GetFirstOrDefault(c => c.Value == value);
        }

        public ICollection<FiscalYear> GetList()
        {
            return Get(c => true);
        }
    }
}
