using System;
using System.Collections.Generic;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Interface.Manager;

namespace LMS_Web.Areas.Salary.Interface.Manager
{
   interface IUserSpecificAllowanceManager : IBaseManager<UserSpecificAllowance>
   {
      ICollection<UserSpecificAllowance> GetAllByMonthYear(int month, int year);
        ICollection<UserSpecificAllowance> GetAllByMonthYearPayScale(int month, int year,int userid,int payscale);
        ICollection<UserSpecificAllowance> GetList();
        UserSpecificAllowance GetById(int id);
        ICollection<UserSpecificAllowance> ArrearBasicMonthYearPayScale(int month, int year, string payscale);
        ICollection<UserSpecificAllowance> GetListByDateRange(int fromYear, int fromMonth, int toYear , int toMonth);

    }
}
