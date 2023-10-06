using LMS_Web.Areas.Loan.Interface;
using LMS_Web.Areas.Loan.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Models;
using LMS_Web.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LMS_Web.Areas.Loan.Manager
{
    public class LoanInstallmentInfoManager : BaseManager<LoanInstallmentInfo>, ILoanInstallmentInfoManager
    {
        public LoanInstallmentInfoManager(ApplicationDbContext db) : base(new BaseRepository<LoanInstallmentInfo>(db))
        {

        }

        public LoanInstallmentInfo GetCurrentInstalment(string appUserId, int year, int month)
        {
            return GetFirstOrDefault(e => e.AppUserId == appUserId && e.Year == year && e.Month == month,c=>c.UserWiseLoan);
        }

        public LoanInstallmentInfo CurrentLoanInstalmentNo(string appUserId, int year, int month, int loanHeadId)
        {
            return GetFirstOrDefault(e => e.AppUserId == appUserId && e.Year == year && e.Month == month && e.LoanHeadId == loanHeadId);
        }

        public ICollection<LoanInstallmentInfo> GetCurrentMonthLoan(int year, int month)
        {
            return Get(c => c.Year == year && c.Month == month);
        }
        public ICollection<LoanInstallmentInfo> GetCurrentMonthCpfLoan(int year, int month)
        {
            return Get(c => c.Year == year && c.Month == month && c.UserWiseLoan.LoanHeads.LoanHeadType == LoanHeadType.CPF, c => c.UserWiseLoan,c=>c.UserWiseLoan.AppUsers);
        }


        public ICollection<LoanInstallmentInfo> GetList(int fyear, int fmonth, int loanHearId, string appUserId)
        {
            var data = Get(c => (c.Year > fyear || (c.Month >= fmonth && c.Year >= fyear)) && c.LoanHeadId == loanHearId && c.AppUserId == appUserId);
            return data;
        }

        public ICollection<LoanInstallmentInfo> GetNextInstallmentFromCurrentMonth(string appUserId, int currentYear, int currentMonth, int userwiseLoanId)
        {
            var data = Get(c => c.AppUserId == appUserId && c.UserWiseLoanId == userwiseLoanId && (c.Year > currentYear || (c.Month >= currentMonth && c.Year >= currentYear)));
            return data;
        }

        public ICollection<LoanInstallmentInfo> GetLoanheadwiseInstallment(string appUserId, int userwiseLoanId)
        {
            var data = Get(c => c.AppUserId == appUserId && c.UserWiseLoanId == userwiseLoanId);
            return data;
        }


        public LoanInstallmentInfo GetLastInstallmentNo(string appUserId, int loanId)
        {
            var lastInstallment = Get(e => e.AppUserId == appUserId && e.UserWiseLoanId == loanId)
                .OrderByDescending(e => e.Id)
                .FirstOrDefault();
            return lastInstallment;
        }

        public ICollection<LoanInstallmentInfo> GetBetweenDate(int fyear, int fmonth, int tYear, int tMonth, int StationId, int? WingId, int? FromGradeId, int? ToGradeId)
        {
            return Get(d => d.UserWiseLoan.LoanHeads.LoanHeadType==LoanHeadType.CPF && d.UserWiseLoan.AppUsers.IsActive && d.UserWiseLoan.AppUsers.StationId == StationId && (WingId == null || d.UserWiseLoan.AppUsers.WingId==WingId) && (FromGradeId == null || d.UserWiseLoan.AppUsers.GradeId >= FromGradeId ) && (ToGradeId == null || d.UserWiseLoan.AppUsers.GradeId <= ToGradeId) &&
            (d.Year > fyear || (d.Year == fyear && d.Month >= fmonth)) &&
      (d.Year < tYear || (d.Year == tYear && d.Month <= tMonth)),d=>d.UserWiseLoan);
        }

        public ICollection<LoanInstallmentInfo> GetInstallmentByUserwiseLoanId(int userwiseLoanId)
        {
            return Get(c => c.UserWiseLoanId == userwiseLoanId);
        }

        public LoanInstallmentInfo GetCurrentInstalmentByLoan(int userwiseLoanId, int year, int month)
        {
            return GetFirstOrDefault(e => e.UserWiseLoanId == userwiseLoanId && e.Year == year && e.Month == month);
        }

        public LoanInstallmentInfo GetTotalInstalmentNO(string appUserId, int loanId)
        {
            var capitalInstNo = Get(e => e.AppUserId == appUserId && e.UserWiseLoanId == loanId).FirstOrDefault();
           return capitalInstNo;

        }
    }
}
