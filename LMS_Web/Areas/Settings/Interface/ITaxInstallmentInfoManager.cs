using LMS_Web.Areas.Loan.Models;
using LMS_Web.Areas.Settings.Manager;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Interface.Manager;
using System.Collections.Generic;

namespace LMS_Web.Areas.Settings.Interface
{
    interface ITaxInstallmentInfoManager: IBaseManager<TaxInstallmentInfo>
    {
        ICollection<TaxInstallmentInfo> GetListById(int userTaxId);
        TaxInstallmentInfo GetById(int id);
        ICollection<TaxInstallmentInfo> GetUserTaxInstallmentInfo(int userTaxId);
        ICollection<TaxInstallmentInfo> GetByMonthYear(int month,int year); 
        ICollection<TaxInstallmentInfo> GetFilteredData(int? FromGradeId, int? ToGradeId, int year, int month, int stationId, int?wingId);

        TaxInstallmentInfo GetByMonthYearAndTaxId(int year, int month, int taxId);
        TaxInstallmentInfo GetUserSpecificInstallment(string userId, int installmentNo);
        TaxInstallmentInfo GetCurrentInstalment(string appUserId, int year, int month);
    }
}
