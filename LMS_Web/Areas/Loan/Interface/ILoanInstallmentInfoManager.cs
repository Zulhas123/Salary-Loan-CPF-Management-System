using LMS_Web.Areas.Loan.Models;
using LMS_Web.Interface.Manager;
using System.Collections.Generic;

namespace LMS_Web.Areas.Loan.Interface
{
    interface ILoanInstallmentInfoManager : IBaseManager<LoanInstallmentInfo>
    {
        ICollection<LoanInstallmentInfo> GetList(int fyear, int fmonth, int loanHearId, string appUserId);
        LoanInstallmentInfo GetCurrentInstalment(string appUserId, int year, int month);
        LoanInstallmentInfo CurrentLoanInstalmentNo(string appUserId, int year, int month, int loanHeadId);
        ICollection<LoanInstallmentInfo> GetCurrentMonthLoan(int year, int month);
        ICollection<LoanInstallmentInfo> GetBetweenDate(int fyear, int fmonth, int tYear, int tMonth, int StationId, int? WingId, int? FromGradeId, int? ToGradeId);
        ICollection<LoanInstallmentInfo> GetCurrentMonthCpfLoan(int year, int month);

        ICollection<LoanInstallmentInfo> GetNextInstallmentFromCurrentMonth(string appUserId, int currentYear, int currentMonth, int userwiseLoanId);

        ICollection<LoanInstallmentInfo> GetLoanheadwiseInstallment(string appUserId, int userwiseLoanId);

        LoanInstallmentInfo GetLastInstallmentNo(string appUserId, int loanId);
        ICollection<LoanInstallmentInfo> GetInstallmentByUserwiseLoanId(int userwiseLoanId);
        LoanInstallmentInfo GetCurrentInstalmentByLoan(int userwiseLoanId, int year, int month);
        public LoanInstallmentInfo GetTotalInstalmentNO(string appUserId, int loanId);
    }
}
