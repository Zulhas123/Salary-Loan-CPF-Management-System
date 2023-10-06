using System;
using System.Collections.Generic;
using System.Linq;
using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Areas.Settings.ViewModels;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Models;
using LMS_Web.Repository;

namespace LMS_Web.Areas.Salary.Manager
{
    public class UserSpecificAllowanceManager : BaseManager<UserSpecificAllowance>, IUserSpecificAllowanceManager
    {
        public UserSpecificAllowanceManager(ApplicationDbContext db) : base(new BaseRepository<UserSpecificAllowance>(db))
        {
        }
        
        public ICollection<UserSpecificAllowance> GetAllByMonthYear(int month, int year)
        {
            return Get(c => c.Year == year && c.Month == month);
        }

        public ICollection<UserSpecificAllowance> GetAllByMonthYearPayScale(int month, int year, int userid, int payscale)
        {
            return Get(c=>c.Year==year && c.Month==month && c.PayScaleId==payscale);
        }

        public ICollection<UserSpecificAllowance> ArrearBasicMonthYearPayScale(int month, int year,string payscale)
        {
            return Get(c => c.Year == year && c.Month == month && c.PayScale.Name == payscale);
        }


        public UserSpecificAllowance GetById(int id)
        {
            return GetFirstOrDefault(c => c.Id == id);
        }

        public ICollection<UserSpecificAllowance> GetList()
        {
            return Get(c => true, c => c.AppUser, c => c.PayScale);
        }

        public ICollection<UserSpecificAllowance> GetListByDateRange(int fromYear, int fromMonth, int toYear, int toMonth)
        {
            return Get(d =>
            (d.Year > fromYear || (d.Year == fromYear && d.Month >= fromMonth)) &&          
             (d.Year < toYear || (d.Year == toYear && d.Month <= toMonth)), c=>c.AppUser, c=>c.PayScale);
        }
    }
}
