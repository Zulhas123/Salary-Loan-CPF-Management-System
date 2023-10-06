
using LMS_Web.Areas.CPF.Manager;
using LMS_Web.Areas.CPF.ViewModels;
using LMS_Web.Areas.Loan.Manager;
using LMS_Web.Areas.Salary.Manager;
using LMS_Web.Areas.Settings.Manager;
using LMS_Web.Data;
using LMS_Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Microsoft.Reporting.NETCore;
using System;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using Microsoft.EntityFrameworkCore;
using LMS_Web.Areas.CPF.Interface;
using MySql.Data;
using LMS_Web.SecurityExtension;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Areas.Salary.Controllers;
using Org.BouncyCastle.Asn1.X9;
using System.Collections;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using LMS_Web.Areas.Loan.Interface;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using System.Drawing;
using LMS_Web.Areas.CPF.Models;
using MySqlX.XDevAPI.Relational;
using LMS_Web.Common;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;

namespace LMS_Web.Areas.CPF.Controllers
{
    [Area("CPF")]
    public class PRLInvestController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly UserWiseLoanManager _userWiseLoanManager;
        private readonly CpfInfoManager _cpfInfoManager;
        private readonly InvestmentInfoManager _investmentInfoManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserSpecificAllowanceManager _userSpecificAllowanceManager;
        private readonly LoanInstallmentInfoManager _loanInstallmentInfoManager;
        private readonly IUserWiseLoanManager loanManager;
        private readonly Salary.Manager.StationManager _stationManager;
        private readonly CpfPercentManager _cpfPercentManager;
        private readonly FiscalYearManager _fiscalYearManager;
        private readonly PRlApplicantInfoManager _pRlApplicantInfoManager;
        private readonly IFisYearInvestManager _fisYearInvestManager;
        InvestmentInfoController investmentInfoController;

        public PRLInvestController(ApplicationDbContext dbContext, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _userWiseLoanManager = new UserWiseLoanManager(dbContext);
            _cpfInfoManager = new CpfInfoManager(dbContext);
            _userSpecificAllowanceManager = new UserSpecificAllowanceManager(dbContext);
            _loanInstallmentInfoManager = new LoanInstallmentInfoManager(dbContext);
            _investmentInfoManager = new InvestmentInfoManager(dbContext);
            _stationManager = new Salary.Manager.StationManager(dbContext);
            _cpfPercentManager = new CpfPercentManager(dbContext);
            _fiscalYearManager = new FiscalYearManager(dbContext);
            _pRlApplicantInfoManager = new PRlApplicantInfoManager(dbContext);
            _fisYearInvestManager = new FisYearInvestManager(dbContext);
            investmentInfoController = new InvestmentInfoController(dbContext, _userManager, _webHostEnvironment);
            loanManager = new UserWiseLoanManager(dbContext);

        }
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult PRLInvestmentReport()
        {
            var users = _userManager.Users.Include(c => c.Designation).Where(c => c.IsActive).ToList().Select(s => new
            {
                Text = s.EmployeeCode + "-" + s.FullName + "_" + s.Designation.Name,
                Value = s.Id

            }).ToList();
            ViewBag.Users = new SelectList(users, "Value", "Text");
            ViewBag.Station = _stationManager.GetAll();
            ViewBag.FiscalYear = _fiscalYearManager.GetAll().ToList();
            return View();
        }

        decimal lastIntersetBalance = 0;
        decimal lastInvestmentBalance = 0;
        decimal totalBalance = 0;




        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PRLInvestmentReport(string AppUserId)
        {

            var prlApplication = _pRlApplicantInfoManager.GetListByUser(AppUserId);
            if (prlApplication == null || prlApplication.IsApproved==false)
            {
                TempData["Error"] = "Not applied yet";
                return RedirectToAction("List");
            }
            var prlDate = prlApplication.PrlDate;

            var prlFiscalYear = "";
            var applicationFiscalYear = "";


            if (prlDate.Month <= 7)
            {
                prlFiscalYear = (prlDate.Year - 1) + "-" + prlDate.Year;
            }
            else
            {
                prlFiscalYear = prlDate.Year + "-" + (prlDate.Year + 1);
            }


            if (prlApplication.ApplicationDate.Month <= 6)
            {
                applicationFiscalYear = (prlApplication.ApplicationDate.Year - 1) + "-" + prlApplication.ApplicationDate.Year;
            }
            else
            {
                applicationFiscalYear = prlApplication.ApplicationDate.Year + "-" + (prlApplication.ApplicationDate.Year + 1);
            }

            var prlFiscalYearId = _fiscalYearManager.GetByValue(prlFiscalYear);
            List<InvestmentVm> mainDs = new List<InvestmentVm>();
            DateTime fromDate = new DateTime();
            FiscalYearWiseInvestmentInfo userInvestment = new FiscalYearWiseInvestmentInfo();
            InvestmentDescriptionVm first = null;



            if (prlFiscalYear == applicationFiscalYear)
            {
                fromDate = new DateTime(Convert.ToInt32(applicationFiscalYear.Substring(0, 4)), 7, 1);
                var result = investmentInfoController.GeneralInvestmentCalculate(AppUserId, 1, fromDate, prlDate);
                mainDs = result.sources;

                foreach (var item in result.sources)
                {
                    finalTotalContribution += Convert.ToDecimal(string.Concat(item.TotalContribution.Select(c => (char)('0' + c - '\u09E6'))).Replace("", "."));
                    finaltotalInterest += Convert.ToDecimal(string.Concat(item.Interest.Select(c => (char)('0' + c - '\u09E6'))).Replace("", "."));
                }

                if (result.description.Any())
                {
                    var lastValue = result?.description[result.description.Count - 1];
                    var total = string.Concat(lastValue.Total.Select(c => (char)('0' + c - '\u09E6'))).Replace("", ".");
                    var investAmount = string.Concat(lastValue.InvestAmount.Select(c => (char)('0' + c - '\u09E6'))).Replace("", ".");
                    var interestAmount = string.Concat(lastValue.InterstAmount.Select(c => (char)('0' + c - '\u09E6'))).Replace("", ".");

                    userInvestment.AppUserId = AppUserId;
                    userInvestment.InvestmentAmount = Convert.ToDecimal(investAmount);
                    userInvestment.InterestAmount = Convert.ToDecimal(interestAmount);
                    userInvestment.Total = Convert.ToDecimal(total);
                    first = result.description[0];
                }

            }
            else
            {
                userInvestment = _fisYearInvestManager.GetByUserAndFiscalYear(prlFiscalYearId?.Id ?? 0, AppUserId);

            }

            if (userInvestment != null)
            {

                InvestmentVm insetJerAfterPrl = new InvestmentVm()
                {

                    Description = "জের",
                    InvestmentAmount = string.Concat(userInvestment.Total.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                    Interest = "০",
                    InterestRate = "০",
                    MonthNumber = "০",
                    TotalContribution = "০"

                };
                MainList.Add(insetJerAfterPrl);
            }



            if (first != null)
            {

                InvestmentDescriptionVm desVmJer = new InvestmentDescriptionVm()
                {

                    Description = "জের",
                    InterstAmount = first.InterstAmount,
                    InvestAmount = first.InvestAmount,
                    Total = first.Total
                    //InterstAmount = string.Concat(first.InterstAmount.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                    //InvestAmount = string.Concat(first.InvestAmount.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                    // Total = string.Concat(first.Total.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),

                };
                descriptionList.Add(desVmJer);





                var lastMonth = prlDate.AddDays(-(prlDate.Day));



                var firstInterst = string.Concat(first.InterstAmount.Select(c => (char)('0' + c - '\u09E6'))).Replace("", ".");
                var firstInvest = string.Concat(first.InvestAmount.Select(c => (char)('0' + c - '\u09E6'))).Replace("", ".");
                var firstTotal = string.Concat(first.Total.Select(c => (char)('0' + c - '\u09E6'))).Replace("", ".");
                InvestmentDescriptionVm newInvest = new InvestmentDescriptionVm()
                {
                    Description = NumberToWordConverter.EnglishToBanglaDate(fromDate) + " খ্রিঃ হতে " + NumberToWordConverter.EnglishToBanglaDate(lastMonth) + " খ্রিঃ পর্যন্ত",

                    InterstAmount = string.Concat((userInvestment.InterestAmount - Convert.ToDecimal(firstInterst)).ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                    InvestAmount = string.Concat((userInvestment.InvestmentAmount - Convert.ToDecimal(firstInvest)).ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                    Total = string.Concat((userInvestment.Total - Convert.ToDecimal(firstTotal)).ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")

                };
                descriptionList.Add(newInvest);

                finaltotalInvestment += (userInvestment.InvestmentAmount - Convert.ToDecimal(firstInvest));

            }



            if (userInvestment != null)
            {
                InvestmentDescriptionVm mot = new InvestmentDescriptionVm()
                {
                    Description = "মোট",
                    InterstAmount = string.Concat(userInvestment.InterestAmount.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                    InvestAmount = string.Concat(userInvestment.InvestmentAmount.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                    Total = string.Concat(userInvestment.Total.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),


                };
                descriptionList.Add(mot);

            }


            var loans = loanManager.GetBetweenDate(prlDate, prlApplication.ApplicationDate, AppUserId);
            int fyear = prlDate.Year;
            int fmonth = prlDate.Month;
            fromDate = prlDate;
            DateTime toDate = prlApplication.ApplicationDate;
            int startFrom = 0;
            List<InvestmentInfo> investInfo = new List<InvestmentInfo>();

            totalBalance = userInvestment?.Total ?? 0;
            lastIntersetBalance = userInvestment?.InterestAmount ?? 0;
            lastInvestmentBalance = userInvestment?.InvestmentAmount ?? 0;

            int maxCount = 6;

            string message = "";
            string totalloan = "";
            if (loans.Any())
            {
                foreach (var loanEach in loans)
                {
                    var totalDistance = DistanceOfMonth(loanEach.ApproveDate.Value, fromDate);
                    startFrom = totalDistance.Months;

                    //investmentInfoController.StartFromResult(loanEach.ApproveDate.Value.AddMonths(-1), fromDate);
                    PrlCalculation(totalBalance, startFrom, fmonth, fyear);

                    decimal fromMain = Convert.ToInt32(loanEach.LoanAmount * 60 / 100);
                    decimal fromInterest = loanEach.LoanAmount - fromMain;


                    lastIntersetBalance = lastIntersetBalance - fromInterest;
                    lastInvestmentBalance = lastInvestmentBalance - fromMain;
                    totalBalance = totalBalance - loanEach.LoanAmount;

                    InvestmentVm insetjerAmount = new InvestmentVm()
                    {

                        Description = "জের",
                        InvestmentAmount = string.Concat(totalBalance.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                        Interest = "০",
                        InterestRate = "০",
                        MonthNumber = "০",
                        TotalContribution = "০"

                    };
                    MainList.Add(insetjerAmount);


                    InvestmentDescriptionVm partialDeduction = new InvestmentDescriptionVm()
                    {
                        Description = "(-) আংশিক চুড়ান্ত পাওনা ",
                        InterstAmount = string.Concat(fromInterest.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                        InvestAmount = string.Concat(fromMain.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                        Total = string.Concat((loanEach.LoanAmount).ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),


                    };
                    descriptionList.Add(partialDeduction);



                    //After Loan
                    InvestmentDescriptionVm afterLoan = new InvestmentDescriptionVm()
                    {

                        Description = "মোট",
                        InterstAmount = string.Concat(lastIntersetBalance.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                        InvestAmount = string.Concat(lastInvestmentBalance.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                        Total = string.Concat(totalBalance.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),


                    };
                    descriptionList.Add(afterLoan);
                    var newInfo = loanEach.ApproveDate.Value.AddDays(-totalDistance.Days);
                    fyear = newInfo.Year;
                    fmonth = newInfo.Month;
                    fromDate = newInfo;
                    maxCount -= startFrom;
                    totalloan = string.Concat(loanEach.LoanAmount.ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    message = " তিনি গত  " + NumberToWordConverter.EnglishToBanglaDate(prlDate) + "  খ্রিঃ তারিখ হতে পি আর এল এ আছেন এবং  " + NumberToWordConverter.EnglishToBanglaDate(prlApplication.ApplicationDate) + " খ্রিঃ তারিখ আবেদন করেছেন । ";
                    message += "তিনি গত " + NumberToWordConverter.EnglishToBanglaDate(loanEach.ApplicationDate) + " খ্রি : তারিখে অফেরতযোগ্য অগ্রীম " + totalloan + " টাকা প্রদানের জন্য আবেদন করেছেন। " + NumberToWordConverter.EnglishToBanglaDate(loanEach.ApproveDate.Value) + " খ্রি :তারিখে " + totalloan + " টাকা মুঞ্জর করা হলো।";

                }


            }
            var distanceInfo = DistanceOfMonth(toDate, fromDate);
            startFrom = distanceInfo.Months; //investmentInfoController.StartFromResult(toDate, fromDate);
            if (startFrom > maxCount)
            {
                startFrom = maxCount;
            }
            PrlCalculation(totalBalance, startFrom, fmonth, fyear);


            var selftContributionAllData = _cpfInfoManager.GetListByMonthUser(prlDate.AddMonths(-1).Year, prlDate.AddMonths(-1).Month, prlDate.Year, prlDate.Month, AppUserId);
            decimal last2MonthInvest = selftContributionAllData.Sum(c => c.SelfContribution);
            //var last2MonthArrear=

            InvestmentDescriptionVm selfCont = new InvestmentDescriptionVm()
            {
                Description = "স্বীয় চাঁদা",
                InterstAmount = 0.ToString(),

                InvestAmount = string.Concat(last2MonthInvest.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                Total = string.Concat(last2MonthInvest.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),


            };
            descriptionList.Add(selfCont);

            lastInvestmentBalance += last2MonthInvest;

            InvestmentDescriptionVm finalDataDes = new InvestmentDescriptionVm()
            {
                Description = "সর্বমোট",
                InterstAmount = string.Concat(lastIntersetBalance.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                InvestAmount = string.Concat(lastInvestmentBalance.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                Total = string.Concat((lastIntersetBalance + lastInvestmentBalance).ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),

            };
            descriptionList.Add(finalDataDes);
            var getUser = _userManager.Users.Include(c => c.Designation).Include(c => c.Wing)
           .FirstOrDefault(c => c.Id == AppUserId);
            if (loans.Count == 0)
            {
                message = " তিনি গত  " + NumberToWordConverter.EnglishToBanglaDate(prlDate) + "  খ্রিঃ তারিখ হতে পি আর এল এ আছেন এবং  " + NumberToWordConverter.EnglishToBanglaDate(prlApplication.ApplicationDate) + " খ্রিঃ তারিখ আবেদন করেছেন";
            }

            var finaltotalInvest = finaltotalInvestment;
            var finaltotalInte = finaltotalInterest;
            var finalTotalContri = finalTotalContribution;
            var parameters = new[]
            {
                new ReportParameter("finaltotalInvest",
                    string.Concat(finaltotalInvest.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")),

                new ReportParameter("finalTotalContri",
                    string.Concat(finalTotalContri.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")),
                new ReportParameter("finaltotalInte",
                    string.Concat(finaltotalInte.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")),
                new ReportParameter("userName", getUser.EmployeeCodeBangla + "," + getUser.FullNameBangla + "," + getUser.Designation.Name),
                new ReportParameter("message", message),
                new ReportParameter("fMonth", GetMonthName.MonthInBangla(prlDate.Month) + "/" + string.Concat(fyear.ToString().Select(c => (char)('\u09E6' + c - '0')))),
                new ReportParameter("tMonth", GetMonthName.MonthInBangla(prlApplication.ApplicationDate.Month) + "/" + string.Concat(toDate.Year.ToString().Select(c => (char)('\u09E6' + c - '0')))),

            };



            var main = MainList;
            mainDs.AddRange(main);
            string renderFormat = "PDF";
            using var report = new LocalReport();
            report.EnableExternalImages = true;

            string rptPath = $"{this._webHostEnvironment.WebRootPath}\\Reports\\PRLInvestment.rdlc";
            report.DataSources.Add(new ReportDataSource("DsInvestment", mainDs));
            report.DataSources.Add(new ReportDataSource("InvestDes", descriptionList));

            report.ReportPath = rptPath;
            report.SetParameters(parameters);
            var pdf = report.Render(renderFormat);
            return File(pdf, "application/pdf");



        }

        private DateTimeSpan DistanceOfMonth(DateTime fromDate, DateTime toDate)
        {
            var dateSpan = DateTimeSpan.CompareDates(fromDate, toDate);
            return dateSpan;
        }

        decimal finaltotalInvestment = 0;
        decimal finaltotalInterest = 0;
        decimal finalTotalContribution = 0;

        private List<InvestmentVm> MainList = new List<InvestmentVm>();
        List<InvestmentDescriptionVm> descriptionList = new List<InvestmentDescriptionVm>();
        internal void PrlCalculation(decimal lastTotalBalance, int startFrom, int fmonth, int fyear)
        {
            if (startFrom == 0)
            {
                return;
            }

            int interestRate = 0;
            decimal balance1stPart = 0;
            decimal balance2ndPart = 0;
            decimal balance3rdPart = 0;
            decimal previousInterst = 0;
            decimal fixedAmount = 1500000;
            var bellowFifteen = _cpfPercentManager.GetByName("CPFInterestBellow15")?.Percent ?? 13;
            var bellowThirty = _cpfPercentManager.GetByName("CPFInterest15To30")?.Percent ?? 12;
            var aboveThirty = _cpfPercentManager.GetByName("CPFInterestAbove30")?.Percent ?? 11;
            if (lastTotalBalance <= 1500000)
            {
                interestRate = (int)bellowFifteen;
                balance1stPart = lastTotalBalance;
            }
            else if (lastTotalBalance > 1500000 && lastTotalBalance <= 3000000)
            {

                interestRate = (int)bellowThirty;
                balance2ndPart = lastTotalBalance - 1500000;
            }
            else
            {
                interestRate = (int)aboveThirty;
                balance3rdPart = lastTotalBalance - 3000000;
            }

            if (balance3rdPart > 0)
            {
                InvestmentVm invest1 = new InvestmentVm();
                var interest1 = Math.Round(fixedAmount * startFrom * bellowFifteen / 1200, 2);
                invest1.Description = "স্থিতির প্রথম অংশ";
                invest1.InvestmentAmount = string.Concat(fixedAmount.ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                invest1.MonthNumber = string.Concat(startFrom.ToString().Select(c => (char)('\u09E6' + c - '0')));
                invest1.InterestRate = string.Concat(bellowFifteen.ToString().Select(c => (char)('\u09E6' + c - '0')));
                invest1.TotalContribution = string.Concat((fixedAmount * startFrom).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                invest1.Interest = string.Concat((interest1).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");

                finalTotalContribution += (fixedAmount * startFrom);
                finaltotalInterest += interest1;



                InvestmentVm invest2 = new InvestmentVm();
                var interest2 = Math.Round(fixedAmount * startFrom * bellowThirty / 1200, 2);
                invest2.Description = "স্থিতির ২য় অংশ";
                invest2.InvestmentAmount = string.Concat(fixedAmount.ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                invest2.MonthNumber = string.Concat(startFrom.ToString().Select(c => (char)('\u09E6' + c - '0')));
                invest2.InterestRate = string.Concat(bellowThirty.ToString().Select(c => (char)('\u09E6' + c - '0')));
                invest2.TotalContribution = string.Concat((fixedAmount * startFrom).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                invest2.Interest = string.Concat((interest2).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");


                finalTotalContribution += (fixedAmount * startFrom);
                finaltotalInterest += interest2;


                InvestmentVm invest3 = new InvestmentVm();
                var interest3 = Math.Round(balance3rdPart * startFrom * aboveThirty / 1200, 2);
                invest3.Description = "স্থিতির অবশিষ্টাংশ";
                invest3.InvestmentAmount = string.Concat(balance3rdPart.ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                invest3.MonthNumber = string.Concat(startFrom.ToString().Select(c => (char)('\u09E6' + c - '0')));
                invest3.InterestRate = string.Concat(aboveThirty.ToString().Select(c => (char)('\u09E6' + c - '0')));
                invest3.TotalContribution = string.Concat((balance3rdPart * startFrom).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                invest3.Interest = string.Concat((interest3).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");


                finalTotalContribution += (balance3rdPart * startFrom);
                finaltotalInterest += interest3;

                previousInterst = Math.Round(interest3 + interest2 + interest1, 2);
                MainList.Add(invest1);
                MainList.Add(invest2);
                MainList.Add(invest3);
            }
            else if (balance2ndPart > 0)
            {
                InvestmentVm invest1 = new InvestmentVm();

                var interest1 = Math.Round(fixedAmount * startFrom * bellowFifteen / 1200, 2);
                invest1.Description = "স্থিতির প্রথম অংশ";
                invest1.InvestmentAmount = string.Concat(fixedAmount.ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                invest1.MonthNumber = string.Concat(startFrom.ToString().Select(c => (char)('\u09E6' + c - '0')));
                invest1.InterestRate = string.Concat(bellowFifteen.ToString().Select(c => (char)('\u09E6' + c - '0')));
                invest1.TotalContribution = string.Concat((fixedAmount * startFrom).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                invest1.Interest = string.Concat((interest1).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");


                finalTotalContribution += (fixedAmount * startFrom);
                finaltotalInterest += interest1;

                InvestmentVm invest2 = new InvestmentVm();
                var interest2 = Math.Round(balance2ndPart * startFrom * bellowThirty / 1200, 2);
                invest2.Description = "স্থিতির ২য় অংশ";
                invest2.InvestmentAmount = string.Concat(balance2ndPart.ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                invest2.MonthNumber = string.Concat(startFrom.ToString().Select(c => (char)('\u09E6' + c - '0')));
                invest2.InterestRate = string.Concat(bellowThirty.ToString().Select(c => (char)('\u09E6' + c - '0')));
                invest2.TotalContribution = string.Concat((balance2ndPart * startFrom).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                invest2.Interest = string.Concat((interest2).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");


                finalTotalContribution += (balance2ndPart * startFrom);
                finaltotalInterest += interest2;

                previousInterst = Math.Round(interest2 + interest1, 2);
                MainList.Add(invest1);
                MainList.Add(invest2);
                // balance1stPart = 15000000;


            }
            else
            {
                InvestmentVm invest1 = new InvestmentVm();
                var interest1 = Math.Round(balance1stPart * startFrom * bellowFifteen / 1200, 2);
                invest1.Description = "স্থিতির প্রথম অংশ";
                invest1.InvestmentAmount = string.Concat(balance1stPart.ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                invest1.MonthNumber = string.Concat(startFrom.ToString().Select(c => (char)('\u09E6' + c - '0')));
                invest1.InterestRate = string.Concat(bellowFifteen.ToString().Select(c => (char)('\u09E6' + c - '0')));
                invest1.TotalContribution = string.Concat((balance1stPart * startFrom).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                invest1.Interest = string.Concat((interest1).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");


                finalTotalContribution += (balance1stPart * startFrom);
                finaltotalInterest += interest1;


                previousInterst = Math.Round(interest1, 2);
                MainList.Add(invest1);
            }


            if (startFrom != 0)
            {
                InvestmentDescriptionVm interstMonth = new InvestmentDescriptionVm()
                {

                    Description = string.Concat(startFrom.ToString().Select(c => (char)('\u09E6' + c - '0'))) + " মাসের সুদ ",
                    InterstAmount = string.Concat(previousInterst.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                    InvestAmount = 0.ToString(),
                    Total = string.Concat(previousInterst.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")
                    ,
                };
                descriptionList.Add(interstMonth);

            }




            lastIntersetBalance = lastIntersetBalance + previousInterst;
            totalBalance = lastIntersetBalance + lastInvestmentBalance;

            InvestmentDescriptionVm afterSudSum = new InvestmentDescriptionVm()
            {
                Description = "মোট ",
                InterstAmount = string.Concat(lastIntersetBalance.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                InvestAmount = string.Concat(lastInvestmentBalance.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                Total = string.Concat(totalBalance.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),


            };
            descriptionList.Add(afterSudSum);
            int upTo = fmonth + startFrom;

            for (int i = fmonth; i < upTo; i++) //here change i <= upTo 
            {
                int yearValue = fyear;
                int actualMonthNo = i;

                if (i > 12)
                {
                    yearValue = fyear + 1;
                    actualMonthNo = i - 12;
                }


                InvestmentVm obj = new InvestmentVm()
                {
                    Description = GetMonthName.MonthInBangla(actualMonthNo) + "/" + string.Concat(yearValue.ToString().Select(c => (char)('\u09E6' + c - '0'))),
                    Interest = "০",
                    InvestmentAmount = "০",
                    TotalContribution = "০",
                    InterestRate = "০",
                    MonthNumber = "০"

                };



                MainList.Add(obj);

            }




        }


        public IActionResult List()
        {
            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];
            ViewBag.users = _userManager.Users.ToList();
            var list = _pRlApplicantInfoManager.GetList();
            return View(list);
        }

      
    }
}
