using LMS_Web.Areas.CPF.Interface;
using LMS_Web.Areas.CPF.Models;
using LMS_Web.Areas.Loan.Interface;
using LMS_Web.Areas.Loan.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Web.Areas.CPF.Manager
{
    public class CpfPercentManager : BaseManager<CpfPercent>, ICpfPercentManager
    {
        public CpfPercentManager(ApplicationDbContext db):base(new BaseRepository<CpfPercent>(db))
        {

        }

        public CpfPercent GetById(int id)
        {
            return GetFirstOrDefault(x => x.Id == id);
        }

        public ICollection<CpfPercent> GetList()
        {
            return Get(c => true);
        }

        public CpfPercent GetByName(string name)
        {
            return GetFirstOrDefault(c => c.Name == name);
        }
    }
}
