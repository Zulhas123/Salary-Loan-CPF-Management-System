using LMS_Web.Areas.CPF.Interface;
using LMS_Web.Areas.CPF.Manager;
using LMS_Web.Areas.CPF.Models;
using LMS_Web.Areas.CPF.ViewModels;
using LMS_Web.Areas.Loan.Interface;
using LMS_Web.Areas.Loan.Manager;
using LMS_Web.Areas.Loan.Models;
using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Manager;
using LMS_Web.Areas.Salary.ViewModels;
using LMS_Web.Areas.Settings.Interface;
using LMS_Web.Areas.Settings.Manager;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Common;
using LMS_Web.Data;
using LMS_Web.Interface.Manager;
using LMS_Web.Manager;
using LMS_Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySql.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Reporting.NETCore;
using Microsoft.AspNetCore.Routing;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using Microsoft.ReportingServices.Interfaces;
using Org.BouncyCastle.Bcpg.OpenPgp;
using LMS_Web.SecurityExtension;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Runtime.ConstrainedExecution;
using Google.Protobuf.WellKnownTypes;
using System.Net.NetworkInformation;

namespace LMS_Web.Areas.CPF.Controllers
{
    [Area("CPF")]
    public class InvestmentInfoController : Controller
    {


        private readonly UserManager<AppUser> _userManager;
        private readonly UserWiseLoanManager _userWiseLoanManager;
        private readonly IFisYearInvestManager _fisYearInvestManager;
        private readonly InvestmentInfoManager _investmentInfoManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserSpecificAllowanceManager _userSpecificAllowanceManager;
        private readonly LoanInstallmentInfoManager _loanInstallmentInfoManager;
        private readonly Salary.Manager.StationManager _stationManager;
        private readonly CpfPercentManager _cpfPercentManager;
        private readonly FiscalYearManager _fiscalYearManager;
        private readonly PRlApplicantInfoManager _pRlApplicantInfoManager;

        public List<InvestmentDescriptionVm> investmentDescriptionVmsList;
        private readonly WingsManager _wingsManager;
        private readonly UserStationPermissionManager _userStationPermissionManager;
        private readonly GradeManager _gradeManager;



        static decimal lastInvestmentBalance = 0;
        static decimal lastIntersetBalance = 0;
        static bool isPrl = false;
        static int gotInterest = 0;

        public InvestmentInfoController(ApplicationDbContext dbContext, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _userWiseLoanManager = new UserWiseLoanManager(dbContext);
            //_cpfInfoManager = new CpfInfoManager(dbContext);
            _userSpecificAllowanceManager = new UserSpecificAllowanceManager(dbContext);
            _loanInstallmentInfoManager = new LoanInstallmentInfoManager(dbContext);
            _investmentInfoManager = new InvestmentInfoManager(dbContext);
            _stationManager = new Salary.Manager.StationManager(dbContext);
            _cpfPercentManager = new CpfPercentManager(dbContext);
            _fiscalYearManager = new FiscalYearManager(dbContext);
            // _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            _fisYearInvestManager = new FisYearInvestManager(dbContext);
            _pRlApplicantInfoManager = new PRlApplicantInfoManager(dbContext);
            investmentDescriptionVmsList = new List<InvestmentDescriptionVm>();
            _wingsManager = new WingsManager(dbContext);
            _userStationPermissionManager = new UserStationPermissionManager(dbContext);
            _gradeManager = new GradeManager(dbContext);
        }

        // Start ********** YearlyInvestmentStatement***************

        [HttpGet]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult YearlyInvestmentStatement()
        {
            //ViewBag.users = _userManager.Users.ToList();
            ViewBag.Wings = _wingsManager.GetWings();
            ViewBag.Stations = _userStationPermissionManager.UserWiseLoadStation(_userManager.GetUserId(User));
            ViewBag.Grade = _gradeManager.GetList();
            ViewBag.FiscalYear = _fiscalYearManager.GetAll();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult YearlyInvestmentStatement(int fiscalYear, int StationId, int? WingId, int? FromGradeId, int? ToGradeId)
        {
            try
            {


                List<FiscalYearWiseInvestmentInfo> sources = new List<FiscalYearWiseInvestmentInfo>();
                string currentFiscalYear = "";

                var currentDate = DateTime.Now;
                var currentYear = currentDate.Year;
                if (currentDate.Month < 7)
                {
                    currentFiscalYear = (currentYear - 1) + "-" + currentYear;
                }
                else
                {
                    currentFiscalYear = currentYear + "-" + (currentYear + 1);
                }

                var fiscalYears = _fiscalYearManager.GetById(fiscalYear).Value;
                int fmonth = 7;
                int tmonth = 6;
                int fyear = 0;
                int tyear = 0;




                if (fiscalYears == currentFiscalYear)
                {
                    var splitResult = currentFiscalYear.Split("-");
                    fyear = Convert.ToInt32(splitResult[0]);
                    tyear = Convert.ToInt32(splitResult[1]);
                    var lessThanYear = currentDate.AddYears(-1);
                    var users = _userManager.Users.Where(c => c.JoiningDate <= lessThanYear && c.IsActive && c.StationId == StationId && (WingId == null || c.WingId == WingId) && (FromGradeId == null || c.GradeId >= FromGradeId) && (ToGradeId == null || c.GradeId <= ToGradeId)).Include(c => c.Designation).ToList();
                    tmonth = DateTime.Now.Month;

                    foreach (var user in users)
                    {

                        var dddd = new DateTime(fyear, 7, 1);
                        var data = GeneralInvestmentCalculate(user.Id, 1, dddd, currentDate);
                        var lastValue = data.description[data.description.Count - 1];


                        var total = string.Concat(lastValue.Total.Select(c => (char)('0' + c - '\u09E6'))).Replace("", ".");
                        var investAmount = string.Concat(lastValue.InvestAmount.Select(c => (char)('0' + c - '\u09E6'))).Replace("", ".");
                        var interestAmount = string.Concat(lastValue.InterstAmount.Select(c => (char)('0' + c - '\u09E6'))).Replace("", ".");
                        FiscalYearWiseInvestmentInfo fInfo = new FiscalYearWiseInvestmentInfo();

                        fInfo.AppUserId = user.EmployeeCodeBangla + "-" + user.FullNameBangla + "," + user.Designation.Name;
                        fInfo.InvestmentAmount = Convert.ToDecimal(investAmount == "" ? "0" : investAmount);
                        fInfo.Total = Convert.ToDecimal(total);
                        fInfo.InterestAmount = Convert.ToDecimal(interestAmount == "" ? "0" : interestAmount);



                        sources.Add(fInfo);

                    }




                }
                else
                {

                    var splitResult = fiscalYears.Split("-");
                    fyear = Convert.ToInt32(splitResult[0]);
                    tyear = Convert.ToInt32(splitResult[1]);
                    var Users = _fisYearInvestManager.GetListByFiscalYear(fiscalYear);
                    //sources.AddRange(Users);

                    foreach (var user in Users)
                    {
                        //string.Concat(user.InvestmentAmount.ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                        FiscalYearWiseInvestmentInfo fiscalyearinfo = new FiscalYearWiseInvestmentInfo();
                        fiscalyearinfo.InvestmentAmount = user.InvestmentAmount;
                        fiscalyearinfo.InterestAmount = user.InterestAmount;
                        fiscalyearinfo.Total = user.Total;
                        fiscalyearinfo.AppUserId = user.AppUser.FullNameBangla;

                        sources.Add(fiscalyearinfo);
                    }


                }
                var getwing = _wingsManager.GetWingById(WingId);
                var grade = "";
                if (FromGradeId != null)
                {
                    grade += "গ্রেড-" + string.Concat(FromGradeId.ToString().Select(c => (char)('\u09E6' + c - '0')));
                }
                if (ToGradeId != null)
                {
                    grade += " থেকে গ্রেড-" + string.Concat(ToGradeId.ToString().Select(c => (char)('\u09E6' + c - '0'))) + " পর্যন্ত";
                }
                if (FromGradeId == null && ToGradeId == null)
                {
                    grade = "সকল গ্রেডের";
                }
                var getStation = _stationManager.GetById(StationId);
                string currentMonthNameInBangla = GetMonthName.MonthInBangla(currentDate.Month - 1);
                var parameters = new[]
                {
                 new ReportParameter("wing", getwing != null ? getwing.Name.ToString() : " সকল"),
                new ReportParameter("Station",getStation?.NameBangla.ToString()),
                 new ReportParameter("grade",grade.ToString()),
                new ReportParameter("fMonth", GetMonthName.MonthInBangla(fmonth) + "/" + string.Concat(fyear.ToString().Select(c => (char)('\u09E6' + c - '0')))),
                //new ReportParameter("tMonth",  GetMonthName.MonthInBangla(tmonth) + "/" + string.Concat(tyear.ToString().Select(c => (char)('\u09E6' + c - '0')))),
                 new ReportParameter("tMonth", currentMonthNameInBangla  + "/" + string.Concat(tyear.ToString().Select(c => (char)('\u09E6' + c - '0')))),
            };
                string renderFormat = "PDF";
                string mimtype = "application/pdf";
                using var report = new LocalReport();
                report.EnableExternalImages = true;
                string rptPath = $"{this._webHostEnvironment.WebRootPath}\\Reports\\YearlyInvestStatement.rdlc";
                report.DataSources.Add(new ReportDataSource("YearlyInvestStatement", sources));
                report.ReportPath = rptPath;
                report.SetParameters(parameters);
                var pdf = report.Render(renderFormat);
                return File(pdf, mimtype);
            }
            catch (Exception e)
            {

                return BadRequest("Something went wrong");
            }
        }

        // END ********** YearlyInvestmentStatement***************


        // Common Method created for **  YearlyInvestmentStatement & *****
        public InvestmentStatementVM GeneralInvestmentCalculate(string AppUserId, int fiscalYearId, DateTime? fromDate, DateTime? toDate)
        {
            finalTotalContribution = 0;
            bool isRemove1 = false;

            InvestmentStatementVM vm = new InvestmentStatementVM();
            List<InvestmentVm> source = new List<InvestmentVm>();

            var getUser = _userManager.Users.Include(c => c.Designation).Include(c => c.Wing)
            .FirstOrDefault(c => c.Id == AppUserId);
            DateTime prlDate = getUser.BirthDate.AddYears(getUser.PlrAge);
            var cpfApplicationUser = _pRlApplicantInfoManager.GetListByUser(AppUserId);

            try
            {
                var fiscalYear = _fiscalYearManager.GetById(fiscalYearId).Value;

                var splitResult = fiscalYear.Split("-");
                int fyear = Convert.ToInt32(splitResult[0]);
                if (fromDate != null)
                {
                    isRemove1 = true;
                    fyear = fromDate.Value.Year;
                }


                int tyear = Convert.ToInt32(splitResult[1]);
                if (toDate != null)
                {
                    tyear = toDate.Value.Year;
                }

                int fYearForParam = fyear;
                int tYearForParam = tyear;

                int fmonth = 7;
                int tmonth = 6;
                if (toDate != null)
                {
                    tmonth = toDate.Value.Month;
                }


                var investInfo = _investmentInfoManager.GetListByMonthUser(fyear, fmonth, tyear, tmonth, AppUserId);

                int lastMonth = fmonth - 1;
                int lastYear = fyear;
                if (lastMonth == 0)
                {
                    lastMonth = 12;
                    lastYear = fyear - 1;
                }
                int fPart = (fyear - 1);
                var lastFiscalYear = _fiscalYearManager.GetByValue(fPart + "-" + (fPart + 1));


                var lastFiscalYearInfo = _fisYearInvestManager.GetByUserAndFiscalYear(lastFiscalYear?.Id ?? 0, AppUserId);
                //var lastYearTotalBalance = lastFiscalYearInfo?.Total ?? 0;
                lastInvestmentBalance = lastFiscalYearInfo?.InvestmentAmount ?? 0;
                lastIntersetBalance = lastFiscalYearInfo?.InterestAmount ?? 0;



                int cMonth = fmonth;
                int cYear = fyear;

                if (fromDate == null)
                {
                    fromDate = new DateTime(fyear, fmonth, 1);

                }
                if (toDate == null)
                {
                    toDate = new DateTime(tyear, tmonth, DateTime.DaysInMonth(tyear, tmonth));

                }

                int startFrom = 0;
                int months = 0;
                //decimal fixedAmount = 1500000;

                //******************loan check********************************************


                // decimal approvedLoan = 0;             
                var loan = _userWiseLoanManager.GetBetweenDate(fromDate ?? DateTime.MinValue, toDate ?? DateTime.MinValue, AppUserId);
                string message = "";
                string totalloan = "";
                DateTime loanApproveDate = DateTime.Now;


                if (loan.Any())
                {
                    foreach (var loanEach in loan)
                    {
                        //if (isRemove1)
                        //{
                        months = ((loanEach.ApproveDate.Value.Year - fyear) * 12) + loanEach.ApproveDate.Value.Month - fmonth;

                        //}
                        //else
                        //{
                        //    months = ((loanEach.ApproveDate.Value.Year - fyear) * 12) + loanEach.ApproveDate.Value.Month - fmonth + 1;
                        //}

                        startFrom = StartFromResult(loanEach.ApproveDate.Value.AddMonths(-1), fromDate ?? DateTime.MinValue);
                        var data = JerCalculate(startFrom, months, investInfo, fmonth, fyear, fromDate ?? DateTime.MinValue, loanEach.ApproveDate.Value.AddMonths(-1), false, prlDate, AppUserId);
                        source.AddRange(data);




                        lastIntersetBalance = lastIntersetBalance - loanEach.FromInterest ?? 0;
                        lastInvestmentBalance = lastInvestmentBalance - loanEach.FromMain ?? 0;



                        InvestmentDescriptionVm desVm4 = new InvestmentDescriptionVm()
                        {

                            Description = "সিপিএফ অফেরৎযোগ্য ঋণ বাবদ কর্তন (-)",
                            InvestAmount = string.Concat(loanEach.FromMain.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                            InterstAmount = string.Concat(loanEach.FromInterest.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                            Total = string.Concat((loanEach.LoanAmount).ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),

                        };
                        investmentDescriptionVmsList.Add(desVm4);
                        fyear = loanEach.ApproveDate.Value.Year;
                        fmonth = loanEach.ApproveDate.Value.Month;
                        fromDate = loanEach.ApproveDate.Value;
                        totalloan = string.Concat(loanEach.LoanAmount.ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                        loanApproveDate = loanEach.ApproveDate.Value;
                        //message += "তিনি গত " + NumberToWordConverter.EnglishToBanglaDate(loanEach.ApplicationDate) + " খ্রি: তারিখে "+NonRefundableName(loanEach.NonRefundableLoanNo??1)+" অফেরতযোগ্য অগ্রীম " + totalloan + " টাকা প্রদানের জন্য আবেদন করেছেন। " + NumberToWordConverter.EnglishToBanglaDate(loanApproveDate) + " খ্রি :তারিখে " + totalloan + " টাকা মুঞ্জর করা হলো।";
                        message += "তিনি গত " + NumberToWordConverter.EnglishToBanglaDate(loanEach.ApplicationDate) + " খ্রি: তারিখে " + NonRefundableName(loanEach.NonRefundableLoanNo ?? 1) + " অফেরতযোগ্য অগ্রীম  80 %  প্রদানের জন্য আবেদন করেছেন। " + NumberToWordConverter.EnglishToBanglaDate(loanApproveDate) + " খ্রি :তারিখে 80% টাকা মুঞ্জর করা হলো।";
                        //message += "তিনি গত " + string.Concat(fromDate.Value.ToString().Select(c => (char)('\u09E6' + c - '0'))) + "খ্রি তারিখে অফেরতযোগ্য অগ্রীম " + totalloan + " টাকা প্রদানের জন্য আবেদন করেছেন। " + loanApproveDate.ToString("dd/MM/yyyy") + "খ্রি :তারিখে " + totalloan + " টাকা মুঞ্জর করা হলো।";


                    }

                }
                if (isRemove1)
                {
                    months = ((tyear - fyear) * 12) + tmonth - fmonth;
                }
                else
                {
                    months = ((tyear - fyear) * 12) + tmonth - fmonth + 1;
                }
                //Problem aste pare
                startFrom = StartFromResult(toDate ?? DateTime.MinValue, fromDate ?? DateTime.MinValue);
                var restOfData = JerCalculate(startFrom, months, investInfo, fmonth, fyear, fromDate ?? DateTime.MinValue, toDate ?? DateTime.MinValue, true, prlDate, AppUserId);
                source.AddRange(restOfData);

                var finaltotalInvest = finaltotalInvestment;
                var finaltotalInte = finaltotalInterest;
                var finalTotalContri = finalTotalContribution;
                //DateTime currentDate = DateTime.Now;
                //string currentMonth = string.Concat(currentDate.Month.ToString().Select(c => (char)(c + 2486)));
                DateTime currentDate = DateTime.Now;
                string currentMonthNameInBangla = GetMonthName.MonthInBangla(currentDate.Month - 1);

                var parameters = new[]
                {
                    new ReportParameter("finalTotalContri",
                        string.Concat(finalTotalContri.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")),
                    new ReportParameter("finaltotalInvest",
                        string.Concat(finaltotalInvest.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")),
                    new ReportParameter("finaltotalInte",
                        string.Concat(finaltotalInte.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")),
                    new ReportParameter("userName", getUser.EmployeeCodeBangla + "," + getUser.FullNameBangla + "," + getUser.Designation.Name),
                    //new ReportParameter("finaltotalInvest", finaltotalInvest.ToString()),
                    //new ReportParameter("finaltotalInte", finaltotalInte.ToString()),
                    new ReportParameter("comments", message),
                    new ReportParameter("fMonth", GetMonthName.MonthInBangla(7) + "/" + string.Concat(fYearForParam.ToString().Select(c => (char)('\u09E6' + c - '0')))),
                    new ReportParameter("tMonth", currentMonthNameInBangla  + "/" + string.Concat(tYearForParam.ToString().Select(c => (char)('\u09E6' + c - '0')))),

                };

                vm.sources = source;
                vm.ReportParameters = parameters;
                vm.description = investmentDescriptionVmsList;
                return vm;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        private string NonRefundableName(int no)
        {
            switch (no)
            {
                case 1:
                    return "১ম";
                case 2:
                    return "২য়";
                case 3:
                    return "৩য়";
                case 4:
                    return "৪র্থ";
                case 5:
                    return "৫ম";
                case 6:
                    return "৬ষ্ঠ";
                case 7:
                    return "৭ম";
                case 8:
                    return "৮ম";
                case 9:
                    return "৯ম";
                case 10:
                    return "১০ম";
                case 11:
                    return "১১তম";
                case 12:
                    return "১২তম";
                default:
                    return "১২ এর বেশি তম";
                   
            }
        }

        decimal finaltotalInvestment = 0;
        decimal finaltotalInterest = 0;
        decimal finalTotalContribution = 0;


        internal List<InvestmentVm> JerCalculate(int startFrom, int months, ICollection<InvestmentInfo> investInfo, int cMonth, int cYear, DateTime fromDate, DateTime toDate, bool isEnd, DateTime prlDate, string appUserId)
        {

            List<InvestmentVm> source = new List<InvestmentVm>();
            if (startFrom != 0)
            {
                decimal lastTotalBalance = lastInvestmentBalance + lastIntersetBalance;
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

                InvestmentVm prevJer = new InvestmentVm();
                prevJer.AppUserId = appUserId;
                prevJer.Description = "জের";
                prevJer.InvestmentAmount = string.Concat(lastTotalBalance.ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                prevJer.MonthNumber = string.Concat(0.ToString().Select(c => (char)('\u09E6' + c - '0')));
                prevJer.InterestRate = string.Concat(0.ToString().Select(c => (char)('\u09E6' + c - '0')));
                prevJer.TotalContribution = string.Concat((0 * startFrom).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                prevJer.Interest = string.Concat((0).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                source.Add(prevJer);


                if (balance3rdPart > 0)
                {
                    InvestmentVm invest1 = new InvestmentVm();
                    var interest1 = Math.Round(fixedAmount * startFrom * bellowFifteen / 1200, 2);
                    invest1.AppUserId = appUserId;
                    invest1.Description = "স্থিতির প্রথম অংশ";
                    invest1.InvestmentAmount = string.Concat(fixedAmount.ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    invest1.MonthNumber = string.Concat(startFrom.ToString().Select(c => (char)('\u09E6' + c - '0')));
                    invest1.InterestRate = string.Concat(bellowFifteen.ToString().Select(c => (char)('\u09E6' + c - '0')));
                    invest1.TotalContribution = string.Concat((fixedAmount * startFrom).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    invest1.Interest = string.Concat((interest1).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    finalTotalContribution += (fixedAmount * startFrom);



                    InvestmentVm invest2 = new InvestmentVm();
                    var interest2 = Math.Round(fixedAmount * startFrom * bellowThirty / 1200, 2);
                    invest2.Description = "স্থিতির ২য় অংশ";
                    invest2.AppUserId = appUserId;
                    invest2.InvestmentAmount = string.Concat(fixedAmount.ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    invest2.MonthNumber = string.Concat(startFrom.ToString().Select(c => (char)('\u09E6' + c - '0')));
                    invest2.InterestRate = string.Concat(bellowThirty.ToString().Select(c => (char)('\u09E6' + c - '0')));
                    invest2.TotalContribution = string.Concat((fixedAmount * startFrom).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    invest2.Interest = string.Concat((interest2).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");

                    finalTotalContribution += (fixedAmount * startFrom);

                    InvestmentVm invest3 = new InvestmentVm();
                    var interest3 = Math.Round(balance3rdPart * startFrom * aboveThirty / 1200, 2);
                    invest3.Description = "স্থিতির অবশিষ্টাংশ";
                    invest3.AppUserId = appUserId;
                    invest3.InvestmentAmount = string.Concat(balance3rdPart.ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    invest3.MonthNumber = string.Concat(startFrom.ToString().Select(c => (char)('\u09E6' + c - '0')));
                    invest3.InterestRate = string.Concat(aboveThirty.ToString().Select(c => (char)('\u09E6' + c - '0')));
                    invest3.TotalContribution = string.Concat((balance3rdPart * startFrom).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    invest3.Interest = string.Concat((interest3).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");

                    finalTotalContribution += (balance3rdPart * startFrom);
                    previousInterst = Math.Round(interest3 + interest2 + interest1, 2);
                    source.Add(invest1);
                    source.Add(invest2);
                    source.Add(invest3);
                }
                else if (balance2ndPart > 0)
                {
                    InvestmentVm invest1 = new InvestmentVm();

                    var interest1 = Math.Round(fixedAmount * startFrom * bellowFifteen / 1200, 2);
                    invest1.Description = "স্থিতির প্রথম অংশ";
                    invest1.AppUserId = appUserId;
                    invest1.InvestmentAmount = string.Concat(fixedAmount.ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    invest1.MonthNumber = string.Concat(startFrom.ToString().Select(c => (char)('\u09E6' + c - '0')));
                    invest1.InterestRate = string.Concat(bellowFifteen.ToString().Select(c => (char)('\u09E6' + c - '0')));
                    invest1.TotalContribution = string.Concat((fixedAmount * startFrom).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    invest1.Interest = string.Concat((interest1).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");

                    finalTotalContribution += (fixedAmount * startFrom);

                    InvestmentVm invest2 = new InvestmentVm();
                    var interest2 = Math.Round(balance2ndPart * startFrom * bellowThirty / 1200, 2);
                    invest2.Description = "স্থিতির ২য় অংশ";
                    invest2.AppUserId = appUserId;
                    invest2.InvestmentAmount = string.Concat(balance2ndPart.ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    invest2.MonthNumber = string.Concat(startFrom.ToString().Select(c => (char)('\u09E6' + c - '0')));
                    invest2.InterestRate = string.Concat(bellowThirty.ToString().Select(c => (char)('\u09E6' + c - '0')));
                    invest2.TotalContribution = string.Concat((balance2ndPart * startFrom).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    invest2.Interest = string.Concat((interest2).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");

                    finalTotalContribution += (balance2ndPart * startFrom);
                    previousInterst = Math.Round(interest2 + interest1, 2);
                    source.Add(invest1);
                    source.Add(invest2);
                    //balance1stPart = 15000000;
                }
                else
                {
                    InvestmentVm invest1 = new InvestmentVm();
                    var interest1 = Math.Round(balance1stPart * startFrom * bellowFifteen / 1200, 2);
                    invest1.Description = "স্থিতির প্রথম অংশ";
                    invest1.AppUserId = appUserId;
                    invest1.InvestmentAmount = string.Concat(balance1stPart.ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    invest1.MonthNumber = string.Concat(startFrom.ToString().Select(c => (char)('\u09E6' + c - '0')));
                    invest1.InterestRate = string.Concat(bellowFifteen.ToString().Select(c => (char)('\u09E6' + c - '0')));
                    invest1.TotalContribution = string.Concat((balance1stPart * startFrom).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    invest1.Interest = string.Concat((interest1).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");

                    finalTotalContribution += (balance1stPart * startFrom);

                    previousInterst = Math.Round(interest1, 2);
                    source.Add(invest1);
                }

                decimal ctotalinvest = 0;
                decimal cTotalinterest = previousInterst;




                for (int i = 0; i < months; i++)
                {
                    if (cMonth == prlDate.Month && cYear == prlDate.Year)
                    {
                        break;
                    }


                    if (startFrom == 0)
                    {
                        interestRate = 0;
                    }

                    

                    var obj = investInfo.FirstOrDefault(x => x.Month == cMonth && x.Year == cYear);

                    decimal investmentAmount = obj?.InvestmentAmount ?? 0;
                    var totInterest = Math.Round(investmentAmount * startFrom * interestRate / 1200, 2);
                    InvestmentVm invest = new InvestmentVm()
                    {
                        AppUserId=appUserId,
                        Description = GetMonthName.MonthInBangla(cMonth) + "/" + string.Concat(cYear.ToString().Select(c => (char)('\u09E6' + c - '0'))),
                        InvestmentAmount = string.Concat(investmentAmount.ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                        MonthNumber = string.Concat(startFrom.ToString().Select(c => (char)('\u09E6' + c - '0'))),
                        InterestRate = string.Concat(interestRate.ToString().Select(c => (char)('\u09E6' + c - '0'))),
                        TotalContribution = string.Concat((investmentAmount * startFrom).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                        Interest = string.Concat((Math.Round(investmentAmount * startFrom * interestRate / 1200, 2)).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                    };
                    finalTotalContribution += (investmentAmount * startFrom);

                    //source.Add(invest);
                    cMonth++;
                    if (cMonth > 12)
                    {
                        cMonth = 1;
                        cYear++;
                    }
                    if (startFrom != 0)
                    {
                        startFrom--;

                    }

                    ctotalinvest += obj?.InvestmentAmount ?? 0;
                    cTotalinterest += totInterest;

                    source.Add(invest);
                }
                string des = "জের";
                if (investmentDescriptionVmsList.Any())
                {
                    des = "সিপিএফ ঋণ কর্তন এর পর মোটঃ";
                    //des = "জের";
                }
                InvestmentDescriptionVm desVm1 = new InvestmentDescriptionVm()
                {
                    AppUserId=appUserId,
                    Description = des,
                    InvestAmount = string.Concat(lastInvestmentBalance.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                    InterstAmount = string.Concat(lastIntersetBalance.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                    Total = string.Concat((lastInvestmentBalance + lastIntersetBalance).ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")

                };
                investmentDescriptionVmsList.Add(desVm1);
                DateTime currentDate = DateTime.Now;
                string currentMonthNameInBangla = GetMonthName.MonthInBangla(currentDate.Month - 1);



                fromDate = new DateTime(fromDate.Year, fromDate.Month, 1);
                toDate = new DateTime(toDate.Year, toDate.Month, 1).AddMonths(1).AddDays(-1);
                

                string toDateText = "ইং হতে " + string.Concat(toDate.Day.ToString().Select(c => (char)('\u09E6' + c - '0')));

                InvestmentDescriptionVm desVm2 = new InvestmentDescriptionVm()
                {
                    AppUserId=appUserId,
                    Description = "১ লা " + GetMonthName.MonthInBangla(fromDate.Month) + "/" + string.Concat(fromDate.Year.ToString().Select(c => (char)('\u09E6' + c - '0'))) + toDateText+" " + GetMonthName.MonthInBangla(toDate.Month)+ "/" + string.Concat(toDate.Year.ToString().Select(c => (char)('\u09E6' + c - '0'))) + "(+)",
                    //Description = fromDate.ToString("dd/MM/yyyy") + " ইং হতে " + toDate.ToString("dd/MM/yyyy") + " ইং পর্যন্ত (+)",
                    InvestAmount = string.Concat(ctotalinvest.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                    InterstAmount = string.Concat(cTotalinterest.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                    Total = string.Concat((ctotalinvest + cTotalinterest).ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")

                };
                finaltotalInvestment = ctotalinvest;


                investmentDescriptionVmsList.Add(desVm2);


                lastInvestmentBalance += ctotalinvest;
                lastIntersetBalance += cTotalinterest;
                finaltotalInterest = cTotalinterest;

                string des1 = "সিপিএফ ঋণ বাবদ কর্তনের পূর্বে মোটঃ";
               // string des1 = "জের";
                if (isEnd)
                {
                    des1 = "সর্বমোট";
                }

                InvestmentDescriptionVm desVm3 = new InvestmentDescriptionVm()
                {

                    AppUserId=appUserId,
                    Description = des1,
                    InvestAmount = string.Concat(lastInvestmentBalance.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                    InterstAmount = string.Concat(lastIntersetBalance.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                    Total = string.Concat((lastInvestmentBalance + lastIntersetBalance).ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")

                };
                investmentDescriptionVmsList.Add(desVm3);


                //Previous jer end
            }
            return source;
        }



        [HttpGet]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult SelfInvestmentReport()
        {
            var AppUserId = _userManager.GetUserId(User);
            ViewBag.users = _userManager.Users.ToList();
            ViewBag.FiscalYear = _fiscalYearManager.GetAll().ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelfInvestmentReport(int fiscalYear)
        {
            var AppUserId = _userManager.GetUserId(User);
            var obj = GeneralInvestmentCalculate(AppUserId, fiscalYear, null, null);
            var result = obj.description[obj.description.Count - 1];
            string renderFormat = "PDF";
            string extension = "pdf";
            string mimtype = "application/pdf";
            using var report = new LocalReport();
            report.EnableExternalImages = true;

            string rptPath = $"{this._webHostEnvironment.WebRootPath}\\Reports\\Investment.rdlc";
            report.DataSources.Add(new ReportDataSource("DsInvestment", obj.sources));
            report.DataSources.Add(new ReportDataSource("InvestDes", investmentDescriptionVmsList));

            report.ReportPath = rptPath;
            report.SetParameters(obj.ReportParameters);
            var pdf = report.Render(renderFormat);
            return File(pdf, "application/pdf");
        }



        internal int StartFromResult(DateTime toDate, DateTime fromDate)
        {
            //int fmonth = fromDate.Month;
            int startFrom = ((toDate.Year - fromDate.Year) * 12) + toDate.Month - fromDate.Month+1;

            var today = DateTime.Today;

            if (toDate >= today)
            {

                int remainingMonth = ((toDate.Year - today.Year) * 12) + toDate.Month - today.Month;
                startFrom = startFrom - remainingMonth;


            }
            return startFrom;
        }
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult InvestmentReport()
        {

            ViewBag.Wings = _wingsManager.GetWings();
            ViewBag.Stations = _userStationPermissionManager.UserWiseLoadStation(_userManager.GetUserId(User));
            ViewBag.Grade = _gradeManager.GetList();
            var users = _userManager.Users.Include(c => c.Designation).Where(c => c.IsActive).ToList().Select(s => new
            {
                Text = s.EmployeeCode + "-" + s.FullName + "_" + s.Designation.Name,
                Value = s.Id

            }).ToList();
            ViewBag.Users = new SelectList(users, "Value", "Text");
            //ViewBag.FiscalYear = _fiscalYearManager.GetAll().ToList();
            ViewBag.FiscalYear = _fiscalYearManager.GetList();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InvestmentReport(string AppUserId, int fiscalYear)
        {
            //int StationId, int? WingId, int? FromGradeId, int? ToGradeId,
            //var users = _userManager.Users.Where(c => c.IsActive && (AppUserId == null && (c.StationId == StationId && (WingId == null || c.WingId == WingId) && (FromGradeId == null || c.GradeId >= FromGradeId) && (ToGradeId == null || c.GradeId <= ToGradeId))) || c.Id == AppUserId).ToList();

            //foreach (var item in users)
            //{
            //    //var obj = GeneralInvestmentCalculate(item.Id, fiscalYear, null, null);

            //}


            var obj = GeneralInvestmentCalculate(AppUserId, fiscalYear, null, null);

            var prlUser = _pRlApplicantInfoManager.GetListByUser(AppUserId);
            if (prlUser != null)
            {
                TempData["Success"] = "This User Is PRL User! So Please Generate Report From PRL Investment Report Page!";
                return RedirectToAction("List");


            }
            string renderFormat = "PDF";
            using var report = new LocalReport();
            report.EnableExternalImages = true;

            string rptPath = $"{this._webHostEnvironment.WebRootPath}\\Reports\\Investment.rdlc";
            report.DataSources.Add(new ReportDataSource("DsInvestment", obj.sources));
            report.DataSources.Add(new ReportDataSource("InvestDes", investmentDescriptionVmsList));

            report.ReportPath = rptPath;
            report.SetParameters(obj.ReportParameters);
            var pdf = report.Render(renderFormat);

            return File(pdf, "application/pdf");

        }

        [HttpGet]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public async Task<IActionResult> InvestmentInfoCalculate()
        {
            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InvestmentInfoCalculate(int year, int month)
        {
            string endpoint = "/api/Utility/Investment?year=" + year + "&&month=" + month;

            var baseUri = $"{Request.Scheme}://{Request.Host}";
            string apiUrl = baseUri + endpoint;

            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(""), Encoding.UTF8, "application/json");

                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    //var data = await response.Content.ReadAsStringAsync();
                    //var table = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Data.DataTable>(data);
                    TempData["Success"] = "Investment Calculate Successful";
                }
                else
                {
                    TempData["Error"] = "Investment Calculate Failed";
                }
            }
            return RedirectToAction("InvestmentInfoCalculate");
        }
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult PrevInvestmentInfo()
        {
            ViewBag.users = _userManager.Users.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PrevInvestmentInfo(InvestmentInfo investmentInfo)
        {
            _investmentInfoManager.Add(investmentInfo);
            return RedirectToAction("List");
        }

        public IActionResult List()
        {
            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];
            var list = _investmentInfoManager.GetList();
            //var list = _pRlApplicantInfoManager.GetList();
            // ViewBag.TotalInvestment = finaltotalInvestment;
            return View(list);
        }
    }
}
