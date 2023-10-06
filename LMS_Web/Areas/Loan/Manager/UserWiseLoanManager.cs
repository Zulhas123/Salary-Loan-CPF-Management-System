using LMS_Web.Areas.Loan.Interface;
using LMS_Web.Areas.Loan.Models;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Repository;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Web.Areas.Loan.Manager
{
    public class UserWiseLoanManager : BaseManager<UserWiseLoan>, IUserWiseLoanManager
    {
        public UserWiseLoanManager(ApplicationDbContext db) : base(new BaseRepository<UserWiseLoan>(db))
        {

        }

        public ICollection<UserWiseLoan> GetList()
        {
            return Get(c => !c.IsPaid, c => c.AppUsers, c => c.LoanHeads, c => c.AppUsers.Designation);
        }

        public ICollection<UserWiseLoan> GetActiveLoans()
        {
            return Get(c => !c.IsPaid,c=>c.LoanHeads);
        }
        public ICollection<UserWiseLoan> GetPaidLoans()
        {
            return Get(c => c.IsPaid, c => c.AppUsers, c => c.LoanHeads);
        }

        public ICollection<UserWiseLoan> GetCpfLoans()
        {
            return Get(c => !c.IsPaid && c.LoanHeadId == 1 || c.LoanHeadId == 2, c => c.AppUsers, c => c.LoanHeads);
        }

        public UserWiseLoan GetById(int id)
        {
            return GetFirstOrDefault(x => x.Id == id);
        }

        public UserWiseLoan GetUserWiseLoanApprove(string AppuserId, int LoanheadId, string memorandumNo)
        {
            return GetFirstOrDefault(x => x.AppUserId == AppuserId && x.LoanHeadId == LoanheadId && x.MemorandumNo == memorandumNo);
        }

        public UserWiseLoan GetUserWiseLoanNotApprove(string AppuserId, int LoanheadId)
        {
            return GetFirstOrDefault(x => !x.IsApprove && x.AppUserId == AppuserId && x.LoanHeadId == LoanheadId);
        }



        public ICollection<UserWiseLoan> GetBetweenDate(DateTime fromDate, DateTime toDate, string userId)
        {
            return Get(c => !c.IsRefundable && c.IsApprove && c.ApplicationDate >= fromDate && c.ApplicationDate <= toDate && c.AppUserId == userId).OrderBy(v => v.ApproveDate).ToList();
        }
        public ICollection<UserWiseLoan> GetIndividualActiveLoans(string AppuserId)
        {
            return Get(c => !c.IsPaid && c.AppUserId == AppuserId);
        }

        public ICollection<UserWiseLoan> GetStationWiseLoans(int StationId, int? WingId, int? FromGradeId, int? ToGradeId, DateTime fromDate, DateTime toDate, string type)
        {
            return Get(d => d.ApproveDate!=null && (d.ApproveDate>= fromDate && d.ApproveDate<= toDate) && d.AppUsers.StationId == StationId
            && (WingId == null || d.AppUsers.WingId == WingId) && (FromGradeId == null || d.AppUsers.GradeId >= FromGradeId)
            && (ToGradeId == null || d.AppUsers.GradeId <= ToGradeId)
            && ((type == "Refundable" && d.IsRefundable && d.LoanHeads.LoanHeadType==LoanHeadType.CPF)
            || (type == "NonRefundable" && !d.IsRefundable)
            || (type == "Others" && (d.LoanHeads.LoanHeadType != LoanHeadType.CPF && d.IsRefundable))));

        }

        public ICollection<UserWiseLoan> GetLoansByUser(string userId)
        {
            return Get(c => c.AppUserId == userId, c => c.LoanHeads);
        }

        public ICollection<UserWiseLoan> GetStationWiseIndividualLoans(int StationId, int? WingId, int? FromGradeId, int? ToGradeId, DateTime fromDate, DateTime toDate, string type)
        {
            return Get(d => d.ApproveDate != null&&( d.ApproveDate >= fromDate && d.ApproveDate <= toDate) && d.AppUsers.StationId == StationId
         && (WingId == null || d.AppUsers.WingId == WingId) && (FromGradeId == null || d.AppUsers.GradeId >= FromGradeId)
         && (ToGradeId == null || d.AppUsers.GradeId <= ToGradeId) && (type == "All" || (type == "Refundable" && d.IsRefundable && d.LoanHeads.LoanHeadType == LoanHeadType.CPF) || (type == "Others" && d.LoanHeads.LoanHeadType != LoanHeadType.CPF && d.IsRefundable) || (type == "Non Refundable" && !d.IsRefundable)), c => c.AppUsers, c => c.AppUsers.Designation, c => c.LoanHeads);

        }

        
    }
}
