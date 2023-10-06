using LMS_Web.Areas.Loan.Manager;
using LMS_Web.Areas.Loan.ViewModels;
using LMS_Web.Areas.Salary.Controllers;
using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Manager;
using LMS_Web.Areas.Settings.Interface;
using LMS_Web.Areas.Settings.Manager;
using LMS_Web.Common;
using LMS_Web.Data;
using LMS_Web.Interface.Manager;
using LMS_Web.Manager;
using LMS_Web.Models;
using LMS_Web.SecurityExtension;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Reporting.NETCore;
using Microsoft.ReportingServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;


namespace LMS_Web.Areas.Loan.Controllers
{
    [Area("Loan")]
    public class LoanController : Controller
    {
        private readonly IWingsManager _wingsManager;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserStationPermissionManager _userStationPermissionManager;
        private readonly IGradeManager _gradeManager;
        private readonly SalaryController _salaryController;
        private readonly UserWiseLoanManager _userWiseLoanManager;
        private readonly LoanInstallmentInfoManager _loanInstallmentInfoManager;
        private readonly LoanHeadManager _loanHeadManager;
        private readonly Salary.Manager.StationManager _stationManager;
        public LoanController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, UserManager<AppUser> userManager)
        {
            _webHostEnvironment = webHostEnvironment;
            _wingsManager = new WingsManager(db);
            _userStationPermissionManager = new UserStationPermissionManager(db);
            _gradeManager = new GradeManager(db);
            _userManager = userManager;
            _salaryController = new SalaryController(db, userManager, webHostEnvironment);
            _userWiseLoanManager = new UserWiseLoanManager(db);
            _loanHeadManager = new LoanHeadManager(db);
            _stationManager = new Salary.Manager.StationManager(db);
            _loanInstallmentInfoManager = new LoanInstallmentInfoManager(db);
        }
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult OthersLoanReport()
        {
            ViewBag.Stations = _userStationPermissionManager.UserWiseLoadStation(_userManager.GetUserId(User));
            ViewBag.Wings = _wingsManager.GetWings();
            ViewBag.Grade = _gradeManager.GetList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult OthersLoanReport(int? FromGradeId, int? ToGradeId, int? WingId, int year, int month, int stationId)
        {

            IIncludableQueryable<AppUser, Designation> users = _userManager.Users.Where(c => c.IsActive && (c.StationId == stationId) && (WingId == null || c.WingId == WingId) && (FromGradeId == null || c.GradeId >= FromGradeId) && (ToGradeId == null || c.GradeId <= ToGradeId)).Include(c => c.Wing).Include(c => c.Station).Include(v => v.Department).Include(v => v.Designation);
            var data = _salaryController.CalculateSalary(year, month, users, true);
            int slNo = 1;
            var getwing = _wingsManager.GetWingById(WingId);
            var getStation = _stationManager.GetById(stationId);

            List<OthersLoanVm> list = new List<OthersLoanVm>();
            foreach (DataRow dtRow in data.Rows)
            {
                bool isAdd = false;
                var empCode = dtRow["EmployeeCode"].ToString();
                var empNameBn = dtRow["EmployeeNameBangla"].ToString();
                var empDes = dtRow["Designation"].ToString();


                var mCycleFirstCaptial = dtRow["MotorCycleFirstCapital"].ToString();
                var mCycleFirstCapitalInstallment = dtRow["MotorCycleFirstCapitalInstallment"].ToString();
                var mCycleFirstInterest = dtRow["MotorCycleFirstInterest"].ToString();
                var mCycleFirstInterestInstallment = dtRow["MotorCycleFirstInterestInstallment"].ToString();

                var fKistiForMotor1 = mCycleFirstCapitalInstallment;
                var fAmountForMotor1 = mCycleFirstCaptial;

                if (Convert.ToDecimal(mCycleFirstCaptial) == 0)
                {
                    fAmountForMotor1 = mCycleFirstInterest;
                    fKistiForMotor1 = mCycleFirstInterestInstallment;
                    if (Convert.ToDecimal(fAmountForMotor1) != 0)
                    {
                        isAdd = true;
                    }
                }
                else
                {
                    isAdd = true;
                }

                var motorCycleSecondCapital = dtRow["MotorCycleSecondCapital"].ToString();
                var motorCycleSecondCapitalInstallment = dtRow["MotorCycleSecondCapitalInstallment"].ToString();
                var motorCycleSecondInterest = dtRow["MotorCycleSecondInterest"].ToString();
                var motorCycleSecondInterestInstallment = dtRow["MotorCycleSecondInterestInstallment"].ToString();

                var fKistiForMotor2 = motorCycleSecondCapitalInstallment;
                var fAmountForMotor2 = motorCycleSecondCapital;
                if (Convert.ToDecimal(motorCycleSecondCapital) == 0)
                {
                    fAmountForMotor2 = motorCycleSecondInterest;
                    fKistiForMotor2 = motorCycleSecondInterestInstallment;
                    if (Convert.ToDecimal(fAmountForMotor2) != 0)
                    {
                        isAdd = true;
                    }
                }
                else
                {
                    isAdd = true;
                }


                var carFirstCapital = dtRow["CarFirstCapital"].ToString();
                var carFirstInstallment = dtRow["CarFirstCapitalInstallment"].ToString();
                var carFirstInterest = dtRow["CarFirstInterest"].ToString();
                var carFirstInterestInstallment = dtRow["CarFirstInterestInstallment"].ToString();

                var fKistiForCar1 = carFirstInstallment;
                var fAmountForCar1 = carFirstCapital;

                if (Convert.ToDecimal(carFirstCapital) == 0)
                {
                    fAmountForCar1 = carFirstInterest;
                    fKistiForCar1 = carFirstInterestInstallment;
                    if (Convert.ToDecimal(fAmountForCar1) != 0)
                    {
                        isAdd = true;
                    }
                }
                else
                {
                    isAdd = true;
                }

                var carSecondCapital = dtRow["CarSecondCapital"].ToString();
                var carSecondCapitalInstallment = dtRow["CarSecondCapitalInstallment"].ToString();
                var carSecondInterest = dtRow["CarSecondInterest"].ToString();
                var carSecondInterestInstallment = dtRow["CarSecondInterestInstallment"].ToString();

                var fKistiForCar2 = carSecondCapitalInstallment;
                var fAmountForCar2 = carSecondCapital;

                if (Convert.ToDecimal(carSecondCapital) == 0)
                {
                    fAmountForCar2 = carSecondInterest;
                    fKistiForCar2 = carSecondInterestInstallment;
                    if (Convert.ToDecimal(fAmountForCar2) != 0)
                    {
                        isAdd = true;
                    }
                }
                else
                {
                    isAdd = true;
                }


                var houseBuildingFirstCapital = dtRow["HouseBuildingFirstCapital"].ToString();
                var houseBuildingFirstCapitalInstallment = dtRow["HouseBuildingFirstCapitalInstallment"].ToString();
                var houseBuildingFirstInterest = dtRow["HouseBuildingFirstInterest"].ToString();
                var houseBuildingFirstInterestInstallment = dtRow["HouseBuildingFirstInterestInstallment"].ToString();

                var fKistiForhb1 = houseBuildingFirstCapitalInstallment;
                var fAmountForhb1 = houseBuildingFirstCapital;

                if (Convert.ToDecimal(houseBuildingFirstCapital) == 0)
                {
                    fAmountForhb1 = houseBuildingFirstInterest;
                    fKistiForhb1 = houseBuildingFirstInterestInstallment;
                    if (Convert.ToDecimal(fAmountForhb1) != 0)
                    {
                        isAdd = true;
                    }
                }
                else
                {
                    isAdd = true;
                }

                var houseBuildingSecondCapital = dtRow["HouseBuildingSecondCapital"].ToString();
                var houseBuildingSecondCapitalInstallment = dtRow["HouseBuildingSecondCapitalInstallment"].ToString();
                var houseBuildingSecondInterest = dtRow["HouseBuildingSecondInterest"].ToString();
                var houseBuildingSecondInterestInstallment = dtRow["HouseBuildingSecondInterestInstallment"].ToString();

                var fKistiForhb2 = houseBuildingSecondCapitalInstallment;
                var fAmountForhb2 = houseBuildingSecondCapital;

                if (Convert.ToDecimal(houseBuildingSecondCapital) == 0)
                {
                    fAmountForhb2 = houseBuildingSecondInterest;
                    fKistiForhb2 = houseBuildingSecondInterestInstallment;
                    if (Convert.ToDecimal(fAmountForhb2) != 0)
                    {
                        isAdd = true;
                    }
                }
                else
                {
                    isAdd = true;
                }


                var houseRepairingFirstCapital = dtRow["HouseRepairingFirstCapital"].ToString();
                var houseRepairingFirstCapitalInstallment = dtRow["HouseRepairingFirstCapitalInstallment"].ToString();
                var houseRepairingFirstInterest = dtRow["HouseRepairingFirstInterest"].ToString();
                var houseRepairingFirstInterestInstallment = dtRow["HouseRepairingFirstInterestInstallment"].ToString();

                var fKistiForhr1 = houseRepairingFirstCapitalInstallment;
                var fAmountForhr1 = houseRepairingFirstCapital;

                if (Convert.ToDecimal(houseRepairingFirstCapital) == 0)
                {
                    fAmountForhr1 = houseRepairingFirstInterest;
                    fKistiForhr1 = houseRepairingFirstInterestInstallment;
                    if (Convert.ToDecimal(fAmountForhr1) != 0)
                    {
                        isAdd = true;
                    }
                }
                else
                {
                    isAdd = true;
                }

                var houseRepairingSecondCapital = dtRow["HouseRepairingSecondCapital"].ToString();
                var houseRepairingSecondCapitalInstallment = dtRow["HouseRepairingSecondCapitalInstallment"].ToString();
                var houseRepairingSecondInterest = dtRow["HouseRepairingSecondInterest"].ToString();
                var houseRepairingSecondInterestInstallment = dtRow["HouseRepairingSecondInterestInstallment"].ToString();

                var fKistiForhr2 = houseRepairingSecondCapitalInstallment;
                var fAmountForhr2 = houseRepairingSecondCapital;

                if (Convert.ToDecimal(houseRepairingSecondCapital) == 0)
                {
                    fAmountForhr2 = houseRepairingSecondInterest;
                    fKistiForhr2 = houseRepairingSecondInterestInstallment;
                    if (Convert.ToDecimal(fAmountForhr2) != 0)
                    {
                        isAdd = true;
                    }
                }
                else
                {
                    isAdd = true;
                }



                var houseRepairingThirdCapital = dtRow["HouseRepairingThirdCapital"].ToString();
                var houseRepairingThirdCapitalInstallment = dtRow["HouseRepairingThirdCapitalInstallment"].ToString();
                var houseRepairingThirdInterest = dtRow["HouseRepairingThirdInterest"].ToString();
                var houseRepairingThirdInterestInstallment = dtRow["HouseRepairingThirdInterestInstallment"].ToString();

                var fKistiForhr3 = houseRepairingThirdCapitalInstallment==""?"0":"0";
                var fAmountForhr3 = houseRepairingThirdCapital == "" ? "0" : "0";

                if (Convert.ToDecimal(houseRepairingThirdCapital==""?"0":"0") == 0)
                {
                    fAmountForhr3 = houseRepairingThirdInterest == "" ? "0" : "0";
                    fKistiForhr3 = houseRepairingThirdInterestInstallment == "" ? "0" : "0";
                    if (Convert.ToDecimal(fAmountForhr3 == "" ? "0" : "0") != 0)
                    {
                        isAdd = true;
                    }
                }
                else
                {
                    isAdd = true;
                }


                var othersAdvanceCapital = dtRow["OthersAdvanceCapital"].ToString();
                var othersAdvanceCapitalInstallment = dtRow["OthersAdvanceCapitalInstallment"].ToString();
                var othersAdvanceInterest = dtRow["OthersAdvanceInterest"].ToString();
                var othersAdvanceInterestInstallment = dtRow["OthersAdvanceInterestInstallment"].ToString();


                var fKistiForOthers = othersAdvanceCapitalInstallment;
                var fAmountForOthers = othersAdvanceCapital;

                if (Convert.ToDecimal(othersAdvanceCapital) == 0)
                {
                    fAmountForOthers = othersAdvanceInterest;
                    fKistiForOthers = othersAdvanceInterestInstallment;
                    if (Convert.ToDecimal(fAmountForOthers) != 0)
                    {
                        isAdd = true;
                    }

                }
                else
                {
                    isAdd = true;
                }
                if (!isAdd)
                {
                    continue;

                }

                OthersLoanVm obj = new OthersLoanVm()
                {
                    Sl = slNo,
                    SlNo = Converter.EnglishToBanglaNumberConvert(slNo),
                    Name = Converter.EnglishToBanglaNumberConvert(empCode) + "-" + empNameBn,
                    Designation = empDes,

                    MotorCycleFirstInstallment = Converter.EnglishToBanglaNumberConvert(fKistiForMotor1),
                    MotorCycleSecondInstallment = Converter.EnglishToBanglaNumberConvert(fKistiForMotor2),
                    MotorCycleFirstAmount = Converter.EnglishToBanglaNumberConvert(fAmountForMotor1),
                    MotorCycleSecondAmount = Converter.EnglishToBanglaNumberConvert(fAmountForMotor2),


                    MotorCarFirstAmount = Converter.EnglishToBanglaNumberConvert(fAmountForCar1),
                    MotorCarFirstInstallment = Converter.EnglishToBanglaNumberConvert(fKistiForCar1),
                    MotorCarSecondInstallment = Converter.EnglishToBanglaNumberConvert(fKistiForCar2),
                    MotorCarSecondAmount = Converter.EnglishToBanglaNumberConvert(fAmountForCar2),


                    HouseBuildingFirstAmount = Converter.EnglishToBanglaNumberConvert(fAmountForhb1),
                    HouseBuildingFirstInstallment = Converter.EnglishToBanglaNumberConvert(fKistiForhb1),
                    HouseBuildingSecondAmount = Converter.EnglishToBanglaNumberConvert(fAmountForhb2),
                    HouseBuildingSecondInstallment = Converter.EnglishToBanglaNumberConvert(fKistiForhb2),

                    HouseRepairingFirstAmount = Converter.EnglishToBanglaNumberConvert(fAmountForhr1),
                    HouseRepairingFirstInstallment = Converter.EnglishToBanglaNumberConvert(fKistiForhr1),
                    HouseRepairingSecondAmount = Converter.EnglishToBanglaNumberConvert(fAmountForhr2),
                    HouseRepairingSecondInstallment = Converter.EnglishToBanglaNumberConvert(fKistiForhr2),
                    HouseRepairingThirdInstallment = Converter.EnglishToBanglaNumberConvert(fKistiForhr3),
                    HouseRepairingThirdAmount = Converter.EnglishToBanglaNumberConvert(fAmountForhr3),





                    OthersAdvancedAmount = Converter.EnglishToBanglaNumberConvert(fAmountForOthers),
                    OthersAdvancedInstallment = Converter.EnglishToBanglaNumberConvert(fKistiForOthers),
                };
                list.Add(obj);
                slNo++;


            }


            string renderFormat = "PDF";
            string mimtype = "application/pdf";
            using var report = new LocalReport();
            report.EnableExternalImages = true;
            string rptPath = $"{_webHostEnvironment.WebRootPath}\\Reports\\OthersLoanRpt.rdlc";
            var monthYear = GetMonthName.MonthInBangla(month) + "/" + Converter.EnglishToBanglaNumberConvert(year);
            var parameters = new[]
           {
                new ReportParameter("monthYear", monthYear),
                new ReportParameter("wing", getwing != null ? getwing.Name.ToString() : " সকল "),
                new ReportParameter("station",getStation?.NameBangla.ToString()),

            };

            report.DataSources.Add(new ReportDataSource("OthersLoan", list));

            report.ReportPath = rptPath;
            report.SetParameters(parameters);
            var pdf = report.Render(renderFormat);
            return File(pdf, mimtype);
        }

        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult LoanReport()
        {
            ViewBag.Stations = _userStationPermissionManager.UserWiseLoadStation(_userManager.GetUserId(User));
            ViewBag.Wings = _wingsManager.GetWings();
            ViewBag.Grade = _gradeManager.GetList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult LoanReport(int? FromGradeId, int? ToGradeId, int? WingId, int fYear, int fMonth, int tYear, int tMonth, int stationId, string type)
        {
            var fromDate = new DateTime(fYear, fMonth, 1);
            var toDate = new DateTime(tYear, tMonth, DateTime.DaysInMonth(tYear, tMonth));
            var fromMonth = fMonth;
            var ToMonths = tMonth;
            var fromYear = fYear;
            var toyears = tYear;

            var loans = _userWiseLoanManager.GetStationWiseLoans(stationId, WingId, FromGradeId, ToGradeId, fromDate, toDate, type);
            List<DynamicLoanVm> groupByLoanData = new List<DynamicLoanVm>();
            if (type == "NonRefundable")
            {
                groupByLoanData = loans.GroupBy(c => new { c.NonRefundableLoanNo, c.ApproveDate.Value.Month, c.ApproveDate.Value.Year }).Select(v => new
            DynamicLoanVm
                {
                    LoanNo = v.Key.NonRefundableLoanNo,
                    Month = v.Key.Month,
                    Year = v.Key.Year,
                    Sum = v.Sum(f => f.LoanAmount)
                }).ToList();

            }
            else
            {
                groupByLoanData = loans.GroupBy(c => new { c.LoanHeadId, c.ApproveDate.Value.Month, c.ApproveDate.Value.Year }).Select(v => new
                DynamicLoanVm
                {
                    LoanId = v.Key.LoanHeadId,
                    Month = v.Key.Month,
                    Year = v.Key.Year,
                    Sum = v.Sum(f => f.LoanAmount)
                }).ToList();


            }



            var loanHeads = _loanHeadManager.GetAll();
            var getWing = _wingsManager.GetWingById(WingId ?? 0);
            var getStation = _stationManager.GetById(stationId);
            List<AllLoansVm> list = new List<AllLoansVm>();
            List<NonRefundableLoanVm> nonRefundableList = new List<NonRefundableLoanVm>();

            var difference = (DateTimeSpan.CompareDates(fromDate, toDate).Years) * 12 + DateTimeSpan.CompareDates(fromDate, toDate).Months + 1;
            decimal totalAmount = 0;
            while (difference > 0)
            {

                var loanData = groupByLoanData.Where(c => c.Month == fMonth && c.Year == fYear).ToList();

                if (type == "NonRefundable")
                {

                    NonRefundableLoanVm obj = new NonRefundableLoanVm();
                    obj.MonthYear = GetMonthName.GetFullName(fMonth) + "/" + fYear;
                    foreach (var item in loanData)
                    {
                        var loanAmount = item.Sum;
                        totalAmount += Convert.ToDecimal(loanAmount);
                        switch (item.LoanNo)
                        {
                            case 1:
                                obj.CPF1 = loanAmount;
                                break;
                            case 2:
                                obj.CPF2 = loanAmount;
                                break;
                            case 3:
                                obj.CPF3 = loanAmount;
                                break;
                            case 4:
                                obj.CPF4 = loanAmount;
                                break;
                            case 5:
                                obj.CPF5 = loanAmount;
                                break;
                            case 6:
                                obj.CPF6 = loanAmount;
                                break;
                            case 7:
                                obj.CPF7 = loanAmount;
                                break;
                            case 8:
                                obj.CPF8 = loanAmount;
                                break;
                            case 9:
                                obj.CPF9 = loanAmount;
                                break;
                            case 10:
                                obj.CPF10 = loanAmount;
                                break;
                            case 11:
                                obj.CPF11 = loanAmount;
                                break;
                            case 12:
                                obj.CPF12 = loanAmount;
                                break;
                            default:
                                obj.Others = loanAmount;
                                break;
                        }
                    }
                    nonRefundableList.Add(obj);
                }
                else
                {


                    AllLoansVm obj = new AllLoansVm();
                    obj.MonthYear = GetMonthName.GetFullName(fMonth) + "/" + fYear;


                    foreach (var item in loanData)
                    {
                        var laonName = loanHeads.FirstOrDefault(c => c.Id == item.LoanId).Name;
                        var loanAmount = item.Sum;
                        totalAmount += Convert.ToDecimal(loanAmount);
                        switch (laonName)
                        {
                            case "CPFFirstLoan":
                                obj.CPF1 = loanAmount;
                                break;
                            case "CPFSecondLoan":
                                obj.CPF2 = loanAmount;
                                break;
                            case "MotorCycleFirst":
                                obj.Bike1 = loanAmount;
                                break;
                            case "MotorCycleSecond":
                                obj.Bike2 = loanAmount;
                                break;
                            case "CarFirst":
                                obj.Car1 = loanAmount;
                                break;
                            case "CarSecond":
                                obj.Car2 = loanAmount;
                                break;
                            case "HouseBuildingFirst":
                                obj.Hb1 = loanAmount;
                                break;
                            case "HouseBuildingSecond":
                                obj.Hb2 = loanAmount;
                                break;
                            case "HouseRepairingFirst":
                                obj.Hr1 = loanAmount;
                                break;
                            case "HouseRepairingSecond":
                                obj.Hr2 = loanAmount;
                                break;
                            case "HouseRepairingThird":
                                obj.Hr3 = loanAmount;
                                break;
                            case "OthersAdvance":
                                obj.OtherAdvance = loanAmount;
                                break;
                            case "Others":
                                obj.Others = loanAmount;
                                break;
                        }
                    }
                    list.Add(obj);
                }

                fMonth++;
                if (fMonth == 13)
                {
                    fMonth = 1;
                    fYear++;
                }
                difference--;


            }
            var cpfType = "";
            string rptPath = "";

            if (type == "Refundable")
            {
                cpfType = "ফেরতযোগ্য অগ্রিম";
                rptPath = $"{_webHostEnvironment.WebRootPath}\\Reports\\CPFLoanReport.rdlc";

            }
            else if (type == "NonRefundable")
            {
                cpfType = "অ-ফেরতযোগ্য ঋণ";
                rptPath = $"{_webHostEnvironment.WebRootPath}\\Reports\\CPFNonRefundableReport.rdlc";
            }
            else
            {
                cpfType = "অন্যান্য";
                rptPath = $"{_webHostEnvironment.WebRootPath}\\Reports\\LoanReport.rdlc";
            }
            //Report code
            string renderFormat = "PDF";
            string mimtype = "application/pdf";
            using var report = new LocalReport();
            report.EnableExternalImages = true;

            var fromMonthYear = GetMonthName.GetFullName(fromMonth) + "/" + fromYear;
            var ToMonthYear = GetMonthName.GetFullName(ToMonths) + "/" + toyears;

            //var monthYear = GetMonthName.MonthInBangla(month) + "/" + Converter.EnglishToBanglaNumberConvert(year);
            var parameters = new[]
            {
                new ReportParameter("totalAmount",totalAmount.ToString()),
                new ReportParameter("fromMonthYear", fromMonthYear.ToString()),
                new ReportParameter("ToMonthYear", ToMonthYear.ToString()),
                new ReportParameter("type", cpfType.ToString()),
                new ReportParameter("wing",getWing?.Name.ToString()),
                new ReportParameter("station",getStation?.NameBangla.ToString())

            };
            if (type == "NonRefundable")
            {
                report.DataSources.Add(new ReportDataSource("CPFNonRefundableReportDs", nonRefundableList));
            }
            else
            {

                report.DataSources.Add(new ReportDataSource("CPFLoanReport", list));
            }

            report.ReportPath = rptPath;
            report.SetParameters(parameters);
            var pdf = report.Render(renderFormat);
            return File(pdf, mimtype);
        }

        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult SelfLoanHistory()
        {
            var loanData = _userWiseLoanManager.GetLoansByUser(_userManager.GetUserId(User));
            return View(loanData);

        }
        public IActionResult LoanDetails(int id)
        {
            var details = _loanInstallmentInfoManager.GetInstallmentByUserwiseLoanId(id);
            return PartialView("_LoanDetails", details);

        }
       // [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult MonthWiseReport()
        {
            ViewBag.Stations = _userStationPermissionManager.UserWiseLoadStation(_userManager.GetUserId(User));
            ViewBag.Wings = _wingsManager.GetWings();
            ViewBag.Grade = _gradeManager.GetList();
            return View();

        }
        [HttpPost]
        //[MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult MonthWiseReport(int fYear, int fMonth, int tYear, int tMonth, int stationId, int? WingId, int? FromGradeId, int? ToGradeId, string type)
        {
            var fromDate = new DateTime(fYear, fMonth, 1);
            var toDate = new DateTime(tYear, tMonth, DateTime.DaysInMonth(tYear, tMonth));
            var fromMonth = fMonth;
            var ToMonths = tMonth;
            var fromYear = fYear;
            var toyears = tYear;

            var loans = _userWiseLoanManager.GetStationWiseIndividualLoans(stationId, WingId, FromGradeId, ToGradeId, fromDate, toDate, type);
            List<IndividualLoansVm> list = new List<IndividualLoansVm>();
            foreach (var item in loans)
            {
                IndividualLoansVm obj = new IndividualLoansVm();
                obj.EmployeeCode = item.AppUsers?.EmployeeCode;
                obj.EmployeeName = item.AppUsers?.FullName;
                obj.Designation = item.AppUsers?.Designation?.Name;
                if (!item.IsRefundable)
                {
                    obj.LoanName = item.LoanHeads.DisplayName + "(" + item.NonRefundableLoanNo + ")";
                    
                }
                else
                {
                    obj.LoanName = item.LoanHeads.DisplayName;
                }

                obj.LoanType = item.IsRefundable ? "Refundable" : "Non Refundable";
                obj.ApproveDate = item.ApproveDate;
                obj.LoanAmount = item.LoanAmount;
                list.Add(obj);
            }



            //Report code
            string renderFormat = "PDF";
            string mimtype = "application/pdf";
            using var report = new LocalReport();
            var rptPath = $"{_webHostEnvironment.WebRootPath}\\Reports\\MonthWiseReport.rdlc";
            report.EnableExternalImages = true;

            var fromMonthYear = GetMonthName.GetFullName(fromMonth) + "/" + fromYear;
            var ToMonthYear = GetMonthName.GetFullName(ToMonths) + "/" + toyears;
            string wing = "সকল";
            var getWing = _wingsManager.GetWingById(WingId ?? 0);
            if (getWing != null)
            {
                wing = getWing.Name;
            }
            var getStation = _stationManager.GetById(stationId);
            //var grade = "";
            //if (FromGradeId != null)
            //{
            //    grade += "গ্রেড-" + FromGradeId.ToString().Select(c => (char)('\u09E6' + c - '0'));
            //}
            //if (ToGradeId != null)
            //{
            //    grade += "থেকে গ্রেড-" + FromGradeId.ToString().Select(c => (char)('\u09E6' + c - '0'));
            //}
            //if (FromGradeId == null && ToGradeId == null)
            //{
            //    grade = "সকল গ্রেড";
            //}

            string grade = "";
            var fgrade = string.Concat(FromGradeId.ToString().Select(c => (char)('\u09E6' + c - '0')));
            var tgrade = string.Concat(ToGradeId.ToString().Select(c => (char)('\u09E6' + c - '0')));
            if (FromGradeId != null && ToGradeId != null)
            {
                grade = " গ্রেড  " + fgrade + " থেকে " + " গ্রেড " + tgrade;
            }
            else
            {
                grade = " সকল গ্রেড ";
            }



            var parameters = new[]
            {

                new ReportParameter("fromMonthYear", fromMonthYear.ToString()),
                new ReportParameter("toMonthYear", ToMonthYear.ToString()),
                 new ReportParameter("wing",wing),
                 new ReportParameter("grade",grade.ToString()),
                 new ReportParameter("station",getStation?.NameBangla.ToString())
            };

            report.DataSources.Add(new ReportDataSource("MonthWiseReportDs", list));

            report.ReportPath = rptPath;
            report.SetParameters(parameters);
            var pdf = report.Render(renderFormat);
            return File(pdf, mimtype);
        }
    }
}
