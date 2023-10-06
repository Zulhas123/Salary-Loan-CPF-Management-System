using LMS_Web.Areas.CPF.Models;
using LMS_Web.Areas.Loan.Models;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Interface.Manager;
using System.Collections.Generic;

namespace LMS_Web.Areas.CPF.Interface
{
    interface IFisYearInvestManager: IBaseManager<FiscalYearWiseInvestmentInfo>
    {
        FiscalYearWiseInvestmentInfo GetByUserAndFiscalYear(int fiscalYear, string userId);
        ICollection<FiscalYearWiseInvestmentInfo> GetListByFiscalYear(int fiscalYear);
        ICollection<FiscalYearWiseInvestmentInfo> GetList();

    }
}
