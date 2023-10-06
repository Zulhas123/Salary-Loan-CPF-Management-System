using LMS_Web.Areas.Settings.Interface;
using LMS_Web.Areas.Settings.Manager;
using LMS_Web.Data;
using LMS_Web.Models;
using LMS_Web.SecurityExtension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using Microsoft.Reporting.NETCore;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.AspNetCore.Hosting;
using LMS_Web.Areas.Salary.ViewModels;
using LMS_Web.Interface.Manager;
using LMS_Web.Manager;
using Microsoft.AspNetCore.Identity;
using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Manager;

namespace LMS_Web.Areas.Salary.Controllers
{
    [Area("Salary")]
    public class IncomeTaxController : Controller
    {
        private readonly ITaxInstallmentInfoManager _taxInstallmentInfoManager;
        private readonly IWingsManager _wingsManager;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserStationPermissionManager _userStationPermissionManager;
        private readonly IGradeManager _gradeManager;
        public IncomeTaxController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, UserManager<AppUser> userManager)
        {
            _taxInstallmentInfoManager = new TaxInstallmentInfoManager(db);
            _webHostEnvironment = webHostEnvironment;
            _wingsManager = new WingsManager(db);
            _userStationPermissionManager = new UserStationPermissionManager(db);
            _gradeManager = new GradeManager(db);
            _userManager = userManager;
        }
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult MonthlyReport()
        {
            ViewBag.Stations = _userStationPermissionManager.UserWiseLoadStation(_userManager.GetUserId(User));
            ViewBag.Wings = _wingsManager.GetWings();
            ViewBag.Grade = _gradeManager.GetList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]     
        public IActionResult MonthlyReport(int? FromGradeId, int? ToGradeId, int? WingId, int year, int month, int stationId)
        {
            var dataAll = _taxInstallmentInfoManager.GetFilteredData(FromGradeId, ToGradeId, year, month, stationId, WingId);
            var data = dataAll.OrderBy(c => c.AppUser.Designation.DisgOrder);
            decimal sumOfTax = 0;
            List<IncomeTaxVm> list = new List<IncomeTaxVm>();
            foreach (var t in data)
            {
                var tax = t.UserTax.MonthlyDeduction;
                sumOfTax += tax;

                IncomeTaxVm obj = new IncomeTaxVm()
                {
                    Name = t.AppUser.EmployeeCodeBangla + "-" + t.AppUser.FullNameBangla + "," + t.AppUser.Designation.Name,
                    Amount = string.Concat(tax.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")
                };
                list.Add(obj);
            }


            string renderFormat = "PDF";
            string mimtype = "application/pdf";
            using var report = new LocalReport();
            report.EnableExternalImages = true;
            string rptPath = $"{_webHostEnvironment.WebRootPath}\\Reports\\IncomeTaxRpt.rdlc";
            string gradeName = "";
            if (FromGradeId == null && ToGradeId == null)
            {
                gradeName = "সকল";
            }
            if (FromGradeId != null && ToGradeId != null)
            {
                gradeName = string.Concat(FromGradeId.ToString().Select(c => (char)('\u09E6' + c - '0'))) + " থেকে " + string.Concat(ToGradeId.ToString().Select(c => (char)('\u09E6' + c - '0')));
            }

            var wing = _wingsManager.GetWingById(WingId ?? 0);
            var monthYear = GetMonthName.MonthInBangla(month) + "," + string.Concat(year.ToString().Select(c => (char)('\u09E6' + c - '0')));
            var text = "বিজেআরআই " + wing?.Name + " এর " + gradeName + " গ্রেড কর্মরত কর্মচারীদের " + monthYear + " মাসের বেতন বিল হতে কর্তনকৃত আয়করের তালিকা";
            var parameters = new[]
            {
                new ReportParameter("reportText", text),
                new ReportParameter("monthYear", monthYear),
                new ReportParameter("sumOfTax",string.Concat(sumOfTax.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")),

            };

            report.DataSources.Add(new ReportDataSource("IncomeTax", list));

            report.ReportPath = rptPath;
            report.SetParameters(parameters);
            var pdf = report.Render(renderFormat);
            return File(pdf, mimtype);

        }
    }
}
