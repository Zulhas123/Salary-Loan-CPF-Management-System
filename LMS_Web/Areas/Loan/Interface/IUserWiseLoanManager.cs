using LMS_Web.Areas.Loan.Models;
using LMS_Web.Interface.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Web.Areas.Loan.Interface
{
    interface IUserWiseLoanManager: IBaseManager<UserWiseLoan>
    {
        ICollection<UserWiseLoan> GetList();
        ICollection<UserWiseLoan> GetLoansByUser(string userId);
        ICollection<UserWiseLoan> GetActiveLoans();
        ICollection<UserWiseLoan> GetPaidLoans();
        ICollection<UserWiseLoan> GetCpfLoans();
        UserWiseLoan GetById(int id);
        UserWiseLoan GetUserWiseLoanNotApprove (string AppuserId, int LoanheadId);
        UserWiseLoan GetUserWiseLoanApprove (string AppuserId, int LoanheadId, string memorandumNo);
        ICollection<UserWiseLoan> GetBetweenDate(DateTime fromDate, DateTime toDate, string userId);
        ICollection<UserWiseLoan> GetIndividualActiveLoans(string AppuserId);
        ICollection<UserWiseLoan> GetStationWiseLoans(int StationId, int? WingId, int? FromGradeId, int? ToGradeId, DateTime fromDate, DateTime toDate, string type);
        ICollection<UserWiseLoan> GetStationWiseIndividualLoans(int StationId, int? WingId, int? FromGradeId, int? ToGradeId, DateTime fromDate, DateTime toDate,string type);
      
    }
}
