
using LMS_Web.Areas.CPF.Interface;
using LMS_Web.Areas.CPF.Manager;
using LMS_Web.Areas.CPF.Models;
using LMS_Web.Areas.CPF.ViewModels;
using LMS_Web.Areas.Salary.Controllers;
using LMS_Web.Areas.Salary.Enum;
using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Manager;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Areas.Salary.ViewModels;
using LMS_Web.Areas.Settings.Interface;
using LMS_Web.Areas.Settings.Manager;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Areas.Settings.ViewModels;
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
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;
using StationManager = LMS_Web.Areas.Salary.Manager.StationManager;

namespace LMS_Web.Areas.CPF.Controllers
{
    [Area("CPF")]
    public class UserFundController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly UserFundManager _userFundManager;
        private readonly StationManager _stationManager;
        private readonly WingsManager _wingsManager;
        private readonly GradeManager _gradeManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly DeductionManager _deductionManager;
        private readonly UserDeductionManager _userDeductionManager;
        private readonly GradeWiseFixedDeductionManager _gradeWiseFixedDeductionManager;
        private readonly FixedDeductionManager _fixedDeductionManager;
        private readonly IUserStationPermissionManager _userStationPermissionManager;
        SalaryController salaryController;
        public UserFundController(ApplicationDbContext db, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _userFundManager = new UserFundManager(db);
            _wingsManager = new WingsManager(db);
            _stationManager = new StationManager(db);
            _gradeManager = new GradeManager(db);

            _webHostEnvironment = webHostEnvironment;
            _deductionManager = new DeductionManager(db);
            _userDeductionManager = new UserDeductionManager(db);
            _gradeWiseFixedDeductionManager = new GradeWiseFixedDeductionManager(db);
            _fixedDeductionManager = new FixedDeductionManager(db);
            _userStationPermissionManager = new UserStationPermissionManager(db);
            salaryController = new SalaryController(db, _userManager, _webHostEnvironment);
        }
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult UserFundReport()
        {
            ViewBag.Stations = _userStationPermissionManager.UserWiseLoadStation(_userManager.GetUserId(User));
            ViewBag.Wings = _wingsManager.GetWings();
            ViewBag.Grade = _gradeManager.GetList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserFundReport(int StationId, int? WingId, int? FromGradeId, int? ToGradeId, int year, int month)
        {

            try
            {
                // var users = _userManager.Users.Where(c => c.IsActive &&  c.StationId == StationId && (WingId == null || c.WingId == WingId) && c.GradeId >= FromGradeId && c.GradeId <= ToGradeId).Include(c => c.Designation).Include(c => c.Wing).OrderBy(c => c.Wing.Id).ThenBy(c => c.Designation.DisgOrder);

                List<UserFundVm> sources = new List<UserFundVm>();
                var fundInfos = _userFundManager.GetFundDataForSpecificMonth(StationId, WingId, FromGradeId, ToGradeId, year, month).OrderBy(c => c.AppUser.Designation.DisgOrder); ;
                var getWingName = _wingsManager.GetWingById(WingId ?? 0);
                decimal WelfSum = 0;
                decimal GroupSum = 0;
                decimal RehaSum = 0;

                foreach (var data in fundInfos)
                {
                    //var data = fundInfos.FirstOrDefault(c => c.AppUserId == user.Id);

                    WelfSum += data?.WelfareFund ?? 0;
                    GroupSum += data?.GroupInsurance ?? 0;
                    RehaSum += data?.Rehabilitation ?? 0;

                    //UserFundVm fund = new UserFundVm()
                    //{
                    //    EmployeeName = data.AppUser.EmployeeCodeBangla + "-" + data.AppUser.FullNameBangla + "," + data.AppUser.Designation.Name,
                    //    WelfareFund = string.Concat(data.WelfareFund.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                    //    GroupInsurance = string.Concat(data.GroupInsurance.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),                      
                    //    Rehabilitation = string.Concat(data.Rehabilitation.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),

                    //};

                    UserFundVm fund = new UserFundVm();
                    fund.EmployeeName = data.AppUser.EmployeeCodeBangla + "-" + data.AppUser.FullNameBangla + "," + data.AppUser.Designation.Name;
                    fund.WelfareFund = string.Concat(data.WelfareFund.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    fund.GroupInsurance = string.Concat(data.GroupInsurance.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    fund.Rehabilitation = string.Concat(data.Rehabilitation.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    sources.Add(fund);
                }
                string wings = "";
                var wing = string.Concat(WingId.ToString().Select(c => (char)('\u09E6' + c - '0')));

                string gradeName = "";
                var fgrade = string.Concat(FromGradeId.ToString().Select(c => (char)('\u09E6' + c - '0')));
                var tgrade = string.Concat(ToGradeId.ToString().Select(c => (char)('\u09E6' + c - '0')));
                if (FromGradeId != null && ToGradeId != null)
                {
                    gradeName = fgrade + "  থেকে  " + tgrade;
                }
                else
                {
                    gradeName = " সকল ";
                }
                if (WingId != null)
                {
                    wings = wing;
                }
                else
                {
                    WingId = 2;
                    wings = "কৃষি";
                }
                var parameters = new[]
                {
                     new ReportParameter("rehaSum", string.Concat(RehaSum.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")),
                     new ReportParameter("groupSum", string.Concat(GroupSum.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")),
                     new ReportParameter("welSum", string.Concat(WelfSum.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")),
                     new ReportParameter("wing", getWingName?.Name?.ToString()),
                     new ReportParameter("gradeName", gradeName),
                     new ReportParameter("monthYear", GetMonthName.MonthInBangla(month) + "/" + string.Concat(year.ToString().Select(c => (char)('\u09E6' + c - '0')))),
                };
                string renderFormat = "PDF";
                string mimtype = "application/pdf";
                using var report = new LocalReport();
                report.EnableExternalImages = true;
                string rptPath = $"{this._webHostEnvironment.WebRootPath}\\Reports\\FundReport.rdlc";
                report.DataSources.Add(new ReportDataSource("UserFund", sources));
                report.ReportPath = rptPath;
                report.SetParameters(parameters);
                var pdf = report.Render(renderFormat);
                return File(pdf, mimtype);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        // [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult GradeWiseUserFund()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GradeWiseUserFund(int year, int month)
        {
            List<UserFundInfo> insertlist = new List<UserFundInfo>();
            List<UserFundInfo> updatelist = new List<UserFundInfo>();

            IIncludableQueryable<AppUser, Designation> users = _userManager.Users.Where(c => c.IsActive).Include(c => c.Station).Include(v => v.Department).Include(v => v.Designation);
            var salary = salaryController.CalculateSalary(year, month, users, false);
            foreach (DataRow dtRow in salary.Rows)
            {
                var userid = dtRow["UserId"].ToString();
                var groupInsurancepf = dtRow["GroupInsurance"].ToString();
                var rehabilitation = dtRow["Rehabilitation"].ToString();
                var userId = dtRow["UserId"].ToString();
                var WelfareFund = dtRow["WelfareFund"].ToString();
                UserFundInfo FundInfo = new UserFundInfo();

                var getMonthData = _userFundManager.GetByUserAndMonth(userid, month, year);
                if (getMonthData == null)
                {
                    FundInfo.AppUserId = userId;
                    FundInfo.GroupInsurance = Convert.ToDecimal(groupInsurancepf);
                    FundInfo.Rehabilitation = Convert.ToDecimal(rehabilitation);
                    FundInfo.WelfareFund = Convert.ToDecimal(WelfareFund);
                    FundInfo.Month = month;
                    FundInfo.Year = year;
                    insertlist.Add(FundInfo);
                }
                else
                {
                    getMonthData.GroupInsurance = Convert.ToDecimal(groupInsurancepf);
                    getMonthData.WelfareFund = Convert.ToDecimal(WelfareFund);
                    getMonthData.Rehabilitation = Convert.ToDecimal(rehabilitation);
                    updatelist.Add(getMonthData);
                }

            }
            _userFundManager.Add(insertlist);
            _userFundManager.Update(updatelist);
            return RedirectToAction("List");

        }

        public IActionResult List()
        {
            ViewBag.Success = TempData["Success"];
            //ViewBag.users = _userManager.Users.ToList();
            var list = _userFundManager.GetList();
            return View(list);
        }

        /// <summary>
        /// Currently using user fund report
        /// </summary>
        /// <returns></returns>
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult UserFundMonthlyReport()
        {
            ViewBag.Stations = _userStationPermissionManager.UserWiseLoadStation(_userManager.GetUserId(User));
            ViewBag.Wings = _wingsManager.GetWings();
            ViewBag.Grade = _gradeManager.GetList();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult UserFundMonthlyReport(int StationId, int? FromGradeId, int? WingId, int? ToGradeId, int fYear, int fMonth, int tYear, int tMonth)
        {
            var allData = _userFundManager.GetBetweenMonthRange(StationId, WingId, FromGradeId, ToGradeId, fYear, fMonth, tYear, tMonth);
            var users = _userManager.Users.Where(c =>
            c.IsActive && (c.StationId == StationId) && (WingId == 0 || c.WingId == WingId) && (FromGradeId == null || c.GradeId >= FromGradeId) &&
            (ToGradeId == null || c.GradeId <= ToGradeId)).Include(c => c.Designation).Include(c => c.Wing);
            var getwing = _wingsManager.GetWingById(WingId);
            var getStation = _stationManager.GetById(StationId);

            List<MonthWiseFundVm> list = new List<MonthWiseFundVm>();
            var data = allData.OrderBy(c => c.Year).ThenBy(c => c.Month);

            var groupByData = data.GroupBy(item => new { item.Month, item.Year }).Select(g =>
            new MonthWiseFundVm
            {
                GroupInsurance = g.Sum(x => Math.Round(Convert.ToDecimal(x.GroupInsurance), 2)),
                WelfareFund = g.Sum(x => Math.Round(Convert.ToDecimal(x.WelfareFund), 2)),
                Rehabilitation = g.Sum(x => Math.Round(Convert.ToDecimal(x.Rehabilitation), 2)),
                EmployeeCount = g.Count(),
                MonthName = GetMonthName.MonthInBangla(g.Key.Month) + "/" + string.Concat(g.Key.Year.ToString().Select(c => (char)('\u09E6' + c - '0')))
            }).ToList();
            decimal groupInsurance = 0;
            decimal welfareFund = 0;
            decimal rehabilitation = 0;

            foreach (var item in groupByData)
            {
                groupInsurance += item.GroupInsurance;
                welfareFund += item.WelfareFund;
                rehabilitation += item.Rehabilitation;
            }
            var fromYear = string.Concat(fYear.ToString().Select(c => (char)('\u09E6' + c - '0')));
            var toYear = string.Concat(tYear.ToString().Select(c => (char)('\u09E6' + c - '0')));
            var fromMonth = GetMonthName.MonthInBangla(fMonth);//string.Concat(fMonth.ToString().Select(c => (char)('\u09E6' + c - '0')));
            var toMonth = GetMonthName.MonthInBangla(tMonth);//string.Concat(tMonth.ToString().Select(c => (char)('\u09E6' + c - '0')));

            string yearMonth = fromMonth + " / " + fromYear + " হতে " + toMonth + " / " + toYear;

            string wing = "";
            var wingId = string.Concat(WingId.ToString().Select(c => (char)('\u09E6' + c - '0')));
            if (wingId != null)
            {
                wing = wingId;
            }
            else
            {
                wing = "সকল";
            }
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
            var parameters = new[]
            {
                    new ReportParameter("wing", getwing != null ? getwing.Name.ToString() : " সকল "),
                     new ReportParameter("Station",getStation?.NameBangla.ToString()),
                    new ReportParameter("yearMonth",yearMonth.ToString()),
                    new ReportParameter("groupInsurance",groupInsurance.ToString()),
                    new ReportParameter("welfareFund",welfareFund.ToString()),
                    new ReportParameter("rehabilitation",rehabilitation.ToString()),
                    new ReportParameter("grade",grade.ToString()),
             };

            string renderFormat = "PDF";
            string mimtype = "application/pdf";
            using var report = new LocalReport();
            report.EnableExternalImages = true;
            string rptPath = $"{this._webHostEnvironment.WebRootPath}\\Reports\\UserFundMonthly.rdlc";
            report.DataSources.Add(new ReportDataSource("UserFundMonthlyReport", groupByData));
            report.ReportPath = rptPath;
            report.SetParameters(parameters);
            var pdf = report.Render(renderFormat);
            return File(pdf, mimtype);
        }



    }

}
