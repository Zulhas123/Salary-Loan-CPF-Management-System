using System.Collections.Generic;
using System.Linq;
using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Areas.Salary.ViewModels;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Repository;

namespace LMS_Web.Areas.Salary.Manager
{
    public class SalaryManager : BaseManager<SalaryVM>, ISalaryManager
    {
        public SalaryManager(ApplicationDbContext db) : base(new BaseRepository<SalaryVM>(db))
        {
        }

        public SalaryVM GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<SalaryVM> GetList()
        {
            throw new System.NotImplementedException();
        }
    }
}
