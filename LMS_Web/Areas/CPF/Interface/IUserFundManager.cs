using LMS_Web.Areas.Loan.Models;
using LMS_Web.Interface.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS_Web.Areas.CPF.Models;
using LMS_Web.Areas.Settings.Models;

namespace LMS_Web.Areas.CPF.Interface
{
    interface IUserFundManager : IBaseManager<UserFundInfo>
    {         
        ICollection<UserFundInfo> GetList();
        UserFundInfo GetById(int id);
        UserFundInfo GetByUserAndMonth(string appuserId, int month, int year);
        ICollection<UserFundInfo> GetBetweenGrade(int year, int month);
        ICollection<UserFundInfo> GetFundDataForSpecificMonth(int StationId, int? WingId, int? FromGradeId, int? ToGradeId, int year, int month);
        ICollection<UserFundInfo> GetBetweenMonthRange(int stationId, int? wingId, int? fromGradeId, int? toGradeId, int fYear, int fMonth, int tYear, int tMonth);
    }
}
