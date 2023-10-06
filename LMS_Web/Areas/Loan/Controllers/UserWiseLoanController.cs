using LMS_Web.Areas.Loan.Manager;
using LMS_Web.Areas.Loan.Models;
using LMS_Web.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS_Web.Areas.Salary.Enum;
using Microsoft.AspNetCore.Identity;
using LMS_Web.Models;
using System.Drawing;
using LMS_Web.Areas.Loan.Interface;
using LMS_Web.Areas.Salary.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;


using LMS_Web.Common;
using Microsoft.AspNetCore.Hosting;
using LMS_Web.Areas.CPF.ViewModels;
using LMS_Web.Areas.Loan.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.NETCore;
using LMS_Web.SecurityExtension;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using LMS_Web.Areas.CPF.Manager;
using LMS_Web.Areas.CPF.Models;
using LMS_Web.Areas.CPF.Interface;
using LMS_Web.Areas.Loan.DataSet;
using UserWiseLoan = LMS_Web.Areas.Loan.Models.UserWiseLoan;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace LMS_Web.Areas.Loan.Controllers
{
    [Area("Loan")]
    public class UserWiseLoanController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private UserWiseLoanManager userWiseLoanManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private LoanHeadManager loanHeadManager;
        private readonly InterestSlabManager interestSlabManager;
        private LoanInstallmentInfoManager _userInstallmentInfoManager;
        private readonly UserWiseLoanManager _userWiseLoanManager;
        private readonly LoanInstallmentInfoManager _loanInstallmentInfo;
        private readonly LoanPaymentHistoryManager _loanPaymentHistory;

        public UserWiseLoanController(ApplicationDbContext dbContext, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            userWiseLoanManager = new UserWiseLoanManager(dbContext);
            loanHeadManager = new LoanHeadManager(dbContext);
            interestSlabManager = new InterestSlabManager(dbContext);
            _userInstallmentInfoManager = new LoanInstallmentInfoManager(dbContext);
            _userWiseLoanManager = new UserWiseLoanManager(dbContext);
            _loanInstallmentInfo = new LoanInstallmentInfoManager(dbContext);
            _loanPaymentHistory = new LoanPaymentHistoryManager(dbContext);
        }

        [HttpGet]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult Add(int? id)
        {
            UserWiseLoan userWiseLoan = new UserWiseLoan();
            if (id != null)
            {
                userWiseLoan = userWiseLoanManager.GetById((int)id);
            }
            // var users = _userManager.Users.Where(c =>c.IsActive).ToList();
            var users = _userManager.Users.Include(c => c.Designation).Where(c => c.IsActive).ToList().Select(s => new
            {
                Text = s.EmployeeCode + "-" + s.FullName + "_" + s.Designation.Name,
                Value = s.Id

            }).ToList();
            ViewBag.Users = new SelectList(users, "Value", "Text");

            //ViewBag.users = (from e in _userManager.Users
            //                 select e.EmployeeCode +"-" + e.FullName).ToList();

            ViewBag.loanHead = loanHeadManager.GetList();
            return View(userWiseLoan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(UserWiseLoan userWiseLoan)
        {
            var notApproved = userWiseLoanManager.GetUserWiseLoanNotApprove(userWiseLoan.AppUserId, userWiseLoan.LoanHeadId);
            if (notApproved != null)
            {
                TempData["Error"] = "Already has an un approved laon. Please approve or reject this loan first";
                return RedirectToAction("List");
            }
            var AlredySameLoan = userWiseLoanManager.GetUserWiseLoanApprove(userWiseLoan.AppUserId, userWiseLoan.LoanHeadId, userWiseLoan.MemorandumNo);
            if (AlredySameLoan != null)
            {
                TempData["Error"] = "Loan already added";
                return RedirectToAction("List");
            }
            var currentMonthSameLoan = _userInstallmentInfoManager.CurrentLoanInstalmentNo(userWiseLoan.AppUserId, DateTime.Now.Year, DateTime.Now.Month, userWiseLoan.LoanHeadId);
            if (currentMonthSameLoan!=null)
            {
                TempData["Error"] = "Already Same Loan Running";
                return RedirectToAction("List");
            }
            decimal totalAmount = userWiseLoan.LoanAmount;

            if (!userWiseLoan.IsRefundable)
            {
                userWiseLoan.CreatedById = _userManager.GetUserId(User);
                userWiseLoan.CreatedDateTime = DateTime.Now;
                var result = userWiseLoanManager.Add(userWiseLoan);
                TempData["Success"] = "Successfully Added";
                return RedirectToAction("List");
            }

            // decimal interest = 0;
            var loanHead = loanHeadManager.GetById(userWiseLoan.LoanHeadId);

            if (loanHead != null)
            {
                decimal interest = 0;
                if (loanHead.LoanHeadType == LoanHeadType.CPF)
                {
                    interest = ((userWiseLoan.LoanAmount * userWiseLoan.NoOfInstallment + userWiseLoan.LoanAmount) * 13) / 2400;
                    totalAmount += interest;
                    userWiseLoan.NoOfInstallmentForInterest = 0;
                    userWiseLoan.InterestDeductionAmount = 0;
                    userWiseLoan.CapitalDeductionAmount = totalAmount / userWiseLoan.NoOfInstallment;

                }
                else
                {
                    var monthlyDeductionAmount = totalAmount / userWiseLoan.NoOfInstallment;
                    userWiseLoan.CapitalDeductionAmount = monthlyDeductionAmount;

                    interest = ((userWiseLoan.LoanAmount * userWiseLoan.NoOfInstallment + userWiseLoan.LoanAmount) * 10) / 2400;

                    var noOfInstallmentForInterest = Convert.ToInt32(Math.Floor(interest / monthlyDeductionAmount));
                    if (noOfInstallmentForInterest <= 0)
                    {
                        noOfInstallmentForInterest = 1;
                    }
                    userWiseLoan.InterestDeductionAmount = interest / noOfInstallmentForInterest;

                    userWiseLoan.NoOfInstallmentForInterest = noOfInstallmentForInterest;

                }

                userWiseLoan.CreatedById = _userManager.GetUserId(User);
                userWiseLoan.CreatedDateTime = DateTime.Now;
                var result = userWiseLoanManager.Add(userWiseLoan);
                if (result)
                {
                    TempData["Success"] = "Successfully Added";
                }
                else
                {
                    TempData["Error"] = "Failed to save";
                }

                return RedirectToAction("List");
            }
            TempData["Error"] = "Failed to save";
            return RedirectToAction("List");
        }
        //[MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult Stop(int loanId, int fmonth, int tmonth, int fyear, int tyear)
        {


            var userWiseLoan = userWiseLoanManager.GetById(loanId);
            var curentDate = DateTime.Now;
            var currentMonth = curentDate.Month;
            var currentYear = curentDate.Year;
            var AllLoan = userWiseLoanManager.GetList();

            //var instamentInfo = _userInstallmentInfoManager.GetListByUser(user.AppUserId);

            //if (userWiseLoan.IsPaid == false && userWiseLoan.IsStop == false)
            //{
            //    userWiseLoan.IsStop = true;
            //    userWiseLoan.StopUntilMonth =tmonth ;
            //    userWiseLoan.StopUntilYear = tyear;

            //   var result = userWiseLoanManager.Update(userWiseLoan);

            /// New code for stop loan Calculation--start----------------

            int distance = (((tyear - fyear) * 12) + tmonth - fmonth) + 1;
            var list = _userInstallmentInfoManager.GetList(fyear, fmonth, userWiseLoan.LoanHeadId, userWiseLoan.AppUserId);

            List<LoanInstallmentInfo> allList = new List<LoanInstallmentInfo>();

            foreach (var item in list)
            {
                int month = item.Month + distance;
                int year = item.Year;
                if (month > 12)
                {
                    month = month - 12;
                    year++;
                }
                item.Month = month;
                item.Year = year;
                allList.Add(item);
            }


            if (allList.Any())
            {
                bool result = _userInstallmentInfoManager.Update(allList);
                if (result)
                {
                    _userInstallmentInfoManager.Update(allList);
                    TempData["Success"] = "Loan Stoped successfully";
                }
                else
                {
                    TempData["Error"] = "Loan Stop Failed";
                }
            }


            else
            {
                TempData["Error"] = "Nothing updated";
            }
            return RedirectToAction("List");
        }
        //[MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        [HttpPost]
        public IActionResult LoanPay(int loanId, DateTime paymentDate, string payNo, decimal paidAmount)
        {

            //decimal payableAmount = 0;
            var curentDate = DateTime.Now;
            var currentMonth = curentDate.Month;
            var currentYear = curentDate.Year;

            var userWiseLoan = userWiseLoanManager.GetById(loanId);
            if (userWiseLoan != null)
            {
                int cInstallmentNo = 0;
                var currentInstallment = _userInstallmentInfoManager.GetCurrentInstalmentByLoan(userWiseLoan.Id, currentYear, currentMonth);
                if (currentInstallment != null)
                {
                    if (currentInstallment.IsCapital)
                    {
                        cInstallmentNo = currentInstallment.CapitalInstallmentNo - 1;
                    }
                    else
                    {
                        cInstallmentNo = userWiseLoan.NoOfInstallment + (currentInstallment.InterestInstallmentNo - 1);
                    }
                }
                var nextInstallment = _userInstallmentInfoManager.GetNextInstallmentFromCurrentMonth(userWiseLoan.AppUserId, currentYear, currentMonth, userWiseLoan.Id);

                userWiseLoan.IsPaid = true;
                _userWiseLoanManager.Update(userWiseLoan);
                if (nextInstallment.Any())
                {
                    _userInstallmentInfoManager.Delete(nextInstallment);
                }
                //var uniqueFileName = "";
                //if (document != null)
                //{
                //    string uplaodsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "loanfile");
                //    uniqueFileName = Guid.NewGuid().ToString() + "_" + document.FileName;
                //    var filePath = Path.Combine(uplaodsFolder, uniqueFileName);


                //    using var stream = new FileStream(filePath, FileMode.Create);
                //    document.CopyTo(stream);
                //}

                LoanPaymentHistory history = new LoanPaymentHistory()
                {
                    UserWiseLoanId = userWiseLoan.Id,
                    PaymentDate =paymentDate,
                    CreatedBy = _userManager.GetUserId(User),
                    BankNo = payNo,
                    InstallmentNoBeforePaid = cInstallmentNo,
                    PaidAmount = paidAmount,
                    AppUserId = userWiseLoan.AppUserId

                };
                _loanPaymentHistory.Add(history);
            }
            //var loanInstallmentInfo = _userInstallmentInfoManager.GetCurrentInstalment(userWiseLoan.AppUserId, currentYear, currentMonth);

            //if (loanInstallmentInfo != null)
            //{
            //    var currentCapitalInstallmentNo = loanInstallmentInfo.CapitalInstallmentNo;
            //    if (currentCapitalInstallmentNo == 0)
            //    {
            //        var currentInterestInstallment = loanInstallmentInfo.InterestInstallmentNo;
            //        payableAmount = (userWiseLoan.NoOfInstallmentForInterest - currentInterestInstallment + 1) * userWiseLoan.InterestDeductionAmount ?? 0;
            //    }
            //    else
            //    {
            //        var interestSlab = interestSlabManager.GetInterest(userWiseLoan.LoanAmount);
            //        var interest = (userWiseLoan.LoanAmount * currentCapitalInstallmentNo + userWiseLoan.LoanAmount * interestSlab.InterestRate) / 2400;
            //        var totalPayable = userWiseLoan.LoanAmount + interest;
            //        var alreadyPaid = userWiseLoan.CapitalDeductionAmount * (currentCapitalInstallmentNo - 1);
            //        payableAmount = totalPayable - alreadyPaid;
            //        userWiseLoan.IsPaid = true;


            //    }
            //    _userWiseLoanManager.Update(userWiseLoan);
            //    var getNextInstallment = _userInstallmentInfoManager.GetNextInstallmentFromCurrentMonth(userWiseLoan.AppUserId, currentYear, currentMonth, userWiseLoan.Id);
            //    _userInstallmentInfoManager.Delete(getNextInstallment);
            //    TempData["Success"] = "Loan Paid Successfully";
            //}
            //else
            //{


            //    var installmentData = _userInstallmentInfoManager.GetLoanheadwiseInstallment(userWiseLoan.AppUserId, userWiseLoan.Id);
            //    var lastYearMonth = _userInstallmentInfoManager.GetLastInstallmentNo(userWiseLoan.AppUserId, userWiseLoan.Id);
            //    ViewBag.YearMonth = lastYearMonth;
            //    if (currentYear < lastYearMonth.Year || (currentYear == lastYearMonth.Year && currentMonth < lastYearMonth.Month))
            //    {
            //        userWiseLoan.IsPaid = true;
            //        _userWiseLoanManager.Update(userWiseLoan);
            //        _userInstallmentInfoManager.Delete(installmentData);
            //        TempData["Success"] = "Loan Paid Successfully";
            //    }
            //    else
            //    {
            //        TempData["Error"] = "Loan already Paid";

            //    }

            //}
            return RedirectToAction("List");

        }
        //[MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult Approved(int loanId, int month, int year, DateTime approveDate, decimal? fromInvest, decimal? fromInterest)
        {
            var userWiseLoan = userWiseLoanManager.GetById(loanId);

            if (!userWiseLoan.IsRefundable)
            {
                userWiseLoan.IsApprove = true;
                userWiseLoan.ApproveDate = approveDate;
                userWiseLoan.FromInterest = fromInterest;
                userWiseLoan.FromMain = fromInvest;
                var result = userWiseLoanManager.Update(userWiseLoan);
                return RedirectToAction("List");

            }



            if (userWiseLoan.IsApprove == false)
            {
                userWiseLoan.IsApprove = true;
                userWiseLoan.ApproveDate = approveDate;
                // userWiseLoan.ApplicationDate = u.ApplicationDate;
                var result = userWiseLoanManager.Update(userWiseLoan);

                List<LoanInstallmentInfo> allList = new List<LoanInstallmentInfo>();
                for (int i = 0; i < userWiseLoan.NoOfInstallment; i++)
                {

                    LoanInstallmentInfo userInstallmentInfo = new LoanInstallmentInfo();
                    userInstallmentInfo.AppUserId = userWiseLoan.AppUserId;
                    userInstallmentInfo.LoanHeadId = userWiseLoan.LoanHeadId;
                    userInstallmentInfo.CapitalInstallmentNo = i + 1;
                    userInstallmentInfo.IsCapital = true;
                    userInstallmentInfo.UserWiseLoanId = userWiseLoan.Id;


                    //month= month;                                                  

                    userInstallmentInfo.Month = (int)month;
                    userInstallmentInfo.Year = (int)year;
                    month += 1;
                    if (month > 12)
                    {
                        month = 1;
                        year = year + 1;

                    }
                    allList.Add(userInstallmentInfo);


                }
                for (int i = 0; i < userWiseLoan.NoOfInstallmentForInterest; i++)
                {
                    LoanInstallmentInfo userInstallmentInfo = new LoanInstallmentInfo();
                    userInstallmentInfo.AppUserId = userWiseLoan.AppUserId;
                    userInstallmentInfo.LoanHeadId = userWiseLoan.LoanHeadId;
                    userInstallmentInfo.InterestInstallmentNo = i + 1;
                    userInstallmentInfo.IsCapital = false;
                    userInstallmentInfo.Month = (int)month;
                    userInstallmentInfo.Year = (int)year;
                    userInstallmentInfo.UserWiseLoanId = userWiseLoan.Id;
                    month += 1;
                    if (month > 12)
                    {
                        month = 1;
                        year = year + 1;

                    }
                    allList.Add(userInstallmentInfo);

                }
                if (result)
                {
                    _userInstallmentInfoManager.Add(allList);
                    TempData["Success"] = "Successfully Added";
                }
                else
                {
                    TempData["Error"] = "Failed Add";
                }

            }
            return RedirectToAction("List");
        }
        [HttpGet]
        //[MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult IndividualLoan()
        {
            var users = _userManager.Users.Include(c => c.Designation).Where(c => c.IsActive).ToList().Select(s => new
            {
                Text = s.EmployeeCode + "-" + s.FullName + "_" + s.Designation.Name,
                Value = s.Id

            }).ToList();
            ViewBag.Users = new SelectList(users, "Value", "Text");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IndividualLoan(string AppUserId)
        {
            List<UserLoanVM> source = new List<UserLoanVM>();
            var getUser = _userManager.Users.Include(c => c.Designation).Include(c => c.Wing)
                    .FirstOrDefault(c => c.Id == AppUserId);
            var CurrentDate = DateTime.Now;
            var currentMonth = CurrentDate.Month;
            var currentYear = CurrentDate.Year;
            var Activeloans = _userWiseLoanManager.GetIndividualActiveLoans(AppUserId);
            foreach (var loan in Activeloans)
            {
                var installmentinfo = _loanInstallmentInfo.CurrentLoanInstalmentNo(loan.AppUserId, currentYear, currentMonth, loan.LoanHeadId);
                var loanHead = loanHeadManager.GetById(loan.LoanHeadId);
                UserLoanVM lm = new UserLoanVM();
                lm.CapitalAmount = loan?.LoanAmount ?? 0;
                if (!loan.IsRefundable)
                {
                    lm.LoanName = "Advance Non Refundable";
                }
                else
                {
                    lm.LoanName = loan.IsRefundable ? loanHead.DisplayName.ToString() : loanHead.DisplayName;
                }
                lm.TotalCapitalInstallment = loan?.NoOfInstallment ?? 0;
                lm.TotalInterestInstallment = loan?.NoOfInstallmentForInterest ?? 0;
                lm.CapitalDeduction = loan?.CapitalDeductionAmount ?? 0;
                lm.loanType = loan.IsRefundable ? "Refundable" : "Non Refundable";
                lm.ApproveDate = loan.ApproveDate?.ToString("dd/MM/yyyy");
                lm.CurrentCapitalInstallment = installmentinfo?.CapitalInstallmentNo ?? 0;
                lm.CurrentInterestInstallment = installmentinfo?.InterestInstallmentNo ?? 0;

                lm.MonthlyInterestAmount = loan?.InterestDeductionAmount ?? 0;
                source.Add(lm);
            }


            string renderFormat = "PDF";
            string extension = "pdf";
            string mimtype = "application/pdf";
            using var report = new LocalReport();
            report.EnableExternalImages = true;
            var code = string.Concat(getUser.EmployeeCode.ToString().Select(c => (char)('\u09E6' + c - '0')));
            var parameters = new[] {
                    new ReportParameter("name",code+"," +getUser.FullNameBangla +"," + getUser.Designation.Name)

            };
            string rptPath = $"{this._webHostEnvironment.WebRootPath}\\Reports\\UserWiseLoan.rdlc";
            report.DataSources.Add(new ReportDataSource("UserWiseLoan", source));

            report.ReportPath = rptPath;
            report.SetParameters(parameters);
            var pdf = report.Render(renderFormat);
            return File(pdf, mimtype);

        }
        public IActionResult List()
        {
            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];
            var list = userWiseLoanManager.GetList().OrderByDescending(c => c.CreatedDateTime);
            return View(list);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ViewBag.users = _userManager.Users.ToList();

            ViewBag.loanHead = loanHeadManager.GetList();
            UserWiseLoan loan = new UserWiseLoan();
            if (id != null)
            {
                loan = userWiseLoanManager.GetById((int)id);
            }

            return View(loan);
        }
        [HttpPost]

        public IActionResult Edit(UserWiseLoan c)
        {

            var getLoan = userWiseLoanManager.GetById(c.Id);

            if (getLoan != null)
            {

                //getLoan.InterestDeductionAmount = c.InterestDeductionAmount;
                //getLoan.NoOfInstallmentForInterest= c.NoOfInstallmentForInterest;
                getLoan.ApplicationDate=c.ApplicationDate;
                getLoan.ApproveDate=c.ApproveDate;


                var result = userWiseLoanManager.Update(getLoan);
                if (result)
                {
                    TempData["Success"] = "Successfully Update";
                }
                else
                {
                    TempData["Error"] = "Failed Update";
                }

            }

            return RedirectToAction("List");
        }
        [HttpPost]
        public IActionResult DateEdit(int loanId, DateTime applicationDate, DateTime approveDate,string memorandumNo,decimal? fromMain,decimal? fromInterest)
        {
          
            var userWiseLoan = userWiseLoanManager.GetById(loanId);
            if (userWiseLoan != null)
            {

                userWiseLoan.ApplicationDate = applicationDate;
                userWiseLoan.ApproveDate = approveDate;
                userWiseLoan.MemorandumNo = memorandumNo;
                if (!userWiseLoan.IsRefundable)
                {
                    userWiseLoan.FromMain=fromMain;
                    userWiseLoan.FromInterest= fromInterest;
                }
                if (userWiseLoanManager.Update(userWiseLoan))
                {
                    TempData["Success"] = "Updated";

                }
                else
                {
                    TempData["Error"] = "Failed";

                }
                return RedirectToAction("List");

            }
            TempData["Error"] = "Failed";
            return RedirectToAction("List");

        }
        public UnpaidVm CalculateRemainingLoan(int id)
        {
            UnpaidVm res = new UnpaidVm();
            decimal alreadyPaid = 0;
            decimal unpaidAmount = 0;
            var currMonth = DateTime.Now.Month;
            var currYear = DateTime.Now.Year;
            var userWiseLoan = userWiseLoanManager.GetById(id);
            if (userWiseLoan != null)
            {
                var nextInstallment = _userInstallmentInfoManager.GetNextInstallmentFromCurrentMonth(userWiseLoan.AppUserId, currYear, currMonth, userWiseLoan.Id);


                // var loanInstallmentInfo = _userInstallmentInfoManager.GetInstallmentByUserwiseLoanId(userWiseLoan.Id);

                var currentInstallmentNo = _userInstallmentInfoManager.GetCurrentInstalmentByLoan(userWiseLoan.Id, currYear, currMonth);
                if (currentInstallmentNo != null)
                {
                    if (currentInstallmentNo.IsCapital)
                    {
                        decimal newInterest = 0;
                        alreadyPaid = userWiseLoan.CapitalDeductionAmount * (currentInstallmentNo.CapitalInstallmentNo - 1);
                        if (userWiseLoan.LoanHeadId == 1 || userWiseLoan.LoanHeadId == 2)
                        {
                            newInterest = ((userWiseLoan.LoanAmount * currentInstallmentNo.CapitalInstallmentNo + userWiseLoan.LoanAmount) * 13) / 2400;

                        }
                        else
                        {
                            newInterest = ((userWiseLoan.LoanAmount * currentInstallmentNo.CapitalInstallmentNo + userWiseLoan.LoanAmount) * 10) / 2400;

                        }

                        unpaidAmount = Decimal.Round((userWiseLoan.LoanAmount - alreadyPaid) + newInterest, 2);
                    }
                    else
                    {
                        var interestPaidAmount = Decimal.Round((decimal)userWiseLoan.InterestDeductionAmount * (currentInstallmentNo.CapitalInstallmentNo - 1), 2);
                        alreadyPaid = userWiseLoan.LoanAmount + interestPaidAmount;
                        unpaidAmount = Decimal.Round((decimal)(userWiseLoan.InterestDeductionAmount * userWiseLoan.NoOfInstallmentForInterest) - interestPaidAmount, 2);
                    }


                }


            }
            res.PaidAmount = alreadyPaid;
            res.RemainingAmount = unpaidAmount;

            return res;
        }
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult PaymentHistory()
        {
            var users = _userManager.Users.Include(c => c.Designation).Where(c => c.IsActive).ToList().Select(s => new
            {
                Text = s.EmployeeCode + "-" + s.FullName + "-" + s.Designation.Name,
                Value = s.Id

            }).ToList();
            ViewBag.Users = new SelectList(users, "Value", "Text");
            return View();
        }
        public IActionResult LoadPaymentHistory(string userId)
        {
            var data = _loanPaymentHistory.GetLoanPaymentHistory(userId);
            return PartialView("_LoadPaymentHistory", data);
        }

        public IActionResult DeletePaymentHistory(int id)
        {

            var Data = _loanPaymentHistory.GetById(id);

            if (Data != null)
            {
                var result = _loanPaymentHistory.Delete(Data);
                if (result)
                {
                    TempData["Success"] = "Successfully Deleted";
                }
                else
                {
                    TempData["Error"] = "Failed Deleted";
                }

            }
            return PartialView("_LoadPaymentHistory");
        }

        public IActionResult DeleteLoan(int id)
        {

            var Data = userWiseLoanManager.GetById(id);

            if (Data != null)
            {
                var result = userWiseLoanManager.Delete(Data);
                if (result)
                {
                    TempData["Success"] = "Successfully Deleted";
                }
                else
                {
                    TempData["Error"] = "Failed to delete";
                }
            }
            return RedirectToAction("List");
        }
    }
}
