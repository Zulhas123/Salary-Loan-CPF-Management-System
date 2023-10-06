using LMS_Web.Areas.CPF.Models;
using System.Collections.Generic;
using LMS_Web.Interface.Manager;

namespace LMS_Web.Areas.CPF.Interface
{
    interface IInvestmentInfoManager:IBaseManager<InvestmentInfo>
    {
        ICollection<InvestmentInfo> GetListByMonth(int year, int month);
        InvestmentInfo GetByMonthAndUserId(int year, int month, string userId);
        ICollection<InvestmentInfo> GetList();
        ICollection<InvestmentInfo> GetListByMonthUser(int fyear, int fmonth, int tyear, int tmonth, string appUserId);
      //  ICollection<InvestmentInfo> GetListForCurrentFiscal(int fyear, int fmonth, int tyear, int tmonth);
    }
}
