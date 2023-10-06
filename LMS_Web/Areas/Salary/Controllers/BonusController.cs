using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS_Web.Areas.Salary.Manager;
using LMS_Web.Data;
using Microsoft.AspNetCore.Hosting;
using LMS_Web.Areas.Salary.Interface.Manager;
using Microsoft.AspNetCore.Identity;
using LMS_Web.Areas.Settings.Interface;
using LMS_Web.Interface.Manager;
using LMS_Web.Models;
using LMS_Web.Areas.Settings.Manager;
using LMS_Web.Manager;
using Microsoft.Reporting.NETCore;
using MySql.Data;
using Microsoft.EntityFrameworkCore;
using LMS_Web.Areas.CPF.ViewModels;
using LMS_Web.Common;
using LMS_Web.Areas.Salary.Models;
using Microsoft.ReportingServices.Interfaces;
using LMS_Web.SecurityExtension;
using System.Drawing;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LMS_Web.Areas.Salary.Controllers
{
    [Area("Salary")]
    public class BonusController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserStationPermissionManager _userStationPermissionManager;
        private readonly IWingsManager _wingsManager;
        private readonly IGradeManager _gradesManager;
        private readonly IBonusManager _bonusManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _dbContext;

        public BonusController(ApplicationDbContext dbContext, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _userStationPermissionManager = new UserStationPermissionManager(dbContext);
            _wingsManager = new WingsManager(dbContext);
            _gradesManager = new GradeManager(dbContext);
            _bonusManager = new BonusManager(dbContext);
            _webHostEnvironment = webHostEnvironment;
            _dbContext = dbContext;
        }

        [HttpGet]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult UserSpecificBonus()
        {
            ViewBag.ErrorMessage = TempData["Error"];
            var users = _userManager.Users.Include(c => c.Designation).Where(c => c.IsActive).ToList().Select(s => new
            {
                Text = s.EmployeeCode + "-" + s.FullName + "_" + s.Designation.Name,
                Value = s.Id

            }).ToList();
            ViewBag.Users = new SelectList(users, "Value", "Text");
            ViewBag.Bonus = _bonusManager.GetList();
            return View();
        }

        [HttpPost]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult UserSpecificBonus(string AppUserId, int BonusId, int year, int month)
        {
            Bonus bonus = _bonusManager.GetById(BonusId);
            if (bonus == null)
            {
                TempData["Error"] = "Please select a bonus";
                return RedirectToAction("UserSpecificBonus");
            }
            var user = _userManager.Users.Include(c=>c.Department).Include(c=>c.Designation).FirstOrDefault(c => c.IsActive && c.Id == AppUserId && (bonus.Religion == "All" || c.Religion == bonus.Religion));
            if (user == null)
            {
                TempData["Error"] = "Bonus Not Applicable";
                return RedirectToAction("UserSpecificBonus");
            }

          

            List<BonusVm> sources = new List<BonusVm>();
            var data = Math.Round((user.CurrentBasic ?? 0) * (bonus.Percent / 100), 2);
            BonusVm obj = new BonusVm();
            obj.EmployeeId = user.EmployeeCode;
            obj.EmployeeName = user.FullName;
            obj.EmployeeNameBn = user.FullNameBangla;
            obj.Department = user.Department.Name;
            obj.Designation = user.Designation.Name;
            obj.BasicSalary = user.CurrentBasic ?? 0;
            obj.FestivalAllowance = data;
            obj.InWord = NumberToWordConverter.ConvertToWords(data.ToString());
            obj.ReportText = bonus.ReportText;
            obj.AccountNo = user.BankAccountNoBangla;
            obj.Amount = string.Concat(data.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
            sources.Add(obj);

            string renderFormat = "PDF";
            string mimtype = "application/pdf";
            using var report = new LocalReport();
            report.EnableExternalImages = true;
            string rptPath = $"{this._webHostEnvironment.WebRootPath}\\Reports\\BonusRpt.rdlc";

            var parameters = new[]
            {
            new ReportParameter("monthYear", bonus.Name+ " bill "+  GetMonthName.GetFullName(month) + "/" + year),

             };
            report.DataSources.Add(new ReportDataSource("Bonus", sources));

            report.ReportPath = rptPath;
            report.SetParameters(parameters);
            var pdf = report.Render(renderFormat);
            return File(pdf, mimtype);
        }

        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult Generate()
        {
            ViewBag.ErrorMessage = TempData["Error"];
            ViewBag.Stations = _userStationPermissionManager.UserWiseLoadStation(_userManager.GetUserId(User));
            ViewBag.Wings = _wingsManager.GetWings();
            ViewBag.Grade = _gradesManager.GetList();
            ViewBag.Bonus = _bonusManager.GetList();
            return View();

        }
      
        private List<BonusVm> CalculateBonus(Bonus bonus, int StationId, int? WingId, int? FromGradeId, int? ToGradeId)
        {

            var users = _userManager.Users.Where(c => c.IsActive && (bonus.Religion == "All" || c.Religion == bonus.Religion) && (c.StationId == StationId) && (WingId == null || c.WingId == WingId) && (FromGradeId == null || c.GradeId >= FromGradeId) &&
              (ToGradeId == null || c.GradeId <= ToGradeId)).Include(c => c.Designation).Include(c => c.Wing).OrderBy(c => c.Designation.DisgOrder);
            getwings = users.Where(c => c.WingId == WingId).ToString();
            List<BonusVm> sources = new List<BonusVm>();

            foreach (var user in users)
            {
                if (user.UserName == "01789377312")
                {
                    continue;
                }
                var data = Math.Round((user.CurrentBasic ?? 0) * (bonus.Percent / 100),2);
                BonusVm obj = new BonusVm();
                obj.EmployeeId = user.EmployeeCode;
                obj.EmployeeName = user.FullName;
                obj.EmployeeNameBn = user.FullNameBangla;
                obj.Department = user.Wing?.Name;
                obj.Designation = user.Designation?.Name;
                obj.BasicSalary = user.CurrentBasic ?? 0;
                obj.FestivalAllowance = data;
                obj.InWord = NumberToWordConverter.ConvertToWords(data.ToString());
                obj.ReportText = bonus.ReportText;
                obj.AccountNo = user.BankAccountNoBangla;
                obj.Amount= string.Concat(data.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                sources.Add(obj);


            }
            return sources;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Generate(int BonusId, int StationId, int? WingId, int? FromGradeId, int? ToGradeId, int year, int month)
        {
            var bonus = _bonusManager.GetById(BonusId);
            if (bonus == null)
            {
                TempData["Error"] = "Please select a bonus";
                return RedirectToAction("Generate");
            }
            var sources = CalculateBonus(bonus, StationId, WingId, FromGradeId, ToGradeId);

            string renderFormat = "PDF";
            string mimtype = "application/pdf";
            using var report = new LocalReport();
            report.EnableExternalImages = true;
            string rptPath = $"{this._webHostEnvironment.WebRootPath}\\Reports\\BonusRpt.rdlc";
            
            var parameters = new[]
            {
            new ReportParameter("monthYear", bonus.Name+"bill,"+  GetMonthName.GetFullName(month) + "/" + year),
            
             };
            report.DataSources.Add(new ReportDataSource("Bonus", sources));

            report.ReportPath = rptPath;
            report.SetParameters(parameters);
            var pdf = report.Render(renderFormat);
            return File(pdf, mimtype);

        }

        [HttpGet]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult BankReport()
        {
            ViewBag.ErrorMessage = TempData["Error"];           
            ViewBag.Stations = _userStationPermissionManager.UserWiseLoadStation(_userManager.GetUserId(User));
            ViewBag.Wings = _wingsManager.GetWings();
            ViewBag.Grade = _gradesManager.GetList();
            ViewBag.Bonus = _bonusManager.GetList();


            return View();
        }
        string getwings = "";
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BankReport(int BonusId, int StationId, int? WingId, int? FromGradeId, int? ToGradeId, int year, int month, string checkNo, DateTime bonusDate)
        {
            var bonus = _bonusManager.GetById(BonusId);
            if (bonus == null)
            {
                TempData["Error"] = "Please select a bonus";
                return RedirectToAction("BankReport");
            }
            var users = _userManager.Users.Where(c => c.StationId == StationId && c.IsActive && (WingId == null || c.WingId == WingId) && (FromGradeId==null || c.GradeId >= FromGradeId) &&(ToGradeId==null || c.GradeId <= ToGradeId)).Include(c => c.Station).Include(v => v.Department).Include(v => v.Designation);
            var source = CalculateBonus(bonus, StationId, WingId, FromGradeId, ToGradeId);
            var getwing = users.FirstOrDefault(c => c.WingId == WingId);
            var bonusName = bonus.Name.ToString();

            decimal sumOfNetBonus = source.Sum(c=>c.FestivalAllowance);
            string renderFormat = "PDF";

            string mimtype = "application/pdf";
            using var report = new LocalReport();
            report.EnableExternalImages = true;
            string rptPath = $"{this._webHostEnvironment.WebRootPath}\\Reports\\BankBonusReport.rdlc";

            var day = string.Concat(bonusDate.Day.ToString().Select(c => (char)('\u09E6' + c - '0')));
            var m = string.Concat(bonusDate.Month.ToString().Select(c => (char)('\u09E6' + c - '0')));
            var y = string.Concat(bonusDate.Year.ToString().Select(c => (char)('\u09E6' + c - '0')));
            var parameters = new[]
            {
            new ReportParameter("wing",getwing?.Wing?.Name.ToString()),
            new ReportParameter("bonusName",bonusName.ToString()),
            new ReportParameter("checkNo", string.Concat(checkNo.Select(c => (char)('\u09E6' + c - '0')))),
            new ReportParameter("monthYear", GetMonthName.MonthInBangla(month) + "/" + string.Concat(year.ToString().Select(c => (char)('\u09E6' + c - '0')))),
            new ReportParameter("salaryDate", day + "/" + m + "/" + y),
            new ReportParameter("totalSum", string.Concat(sumOfNetBonus.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")),
            new ReportParameter("totalSumInWord", NumberToWordConverter.ConvertToWordsBangla(sumOfNetBonus.ToString()))

            };
            report.DataSources.Add(new ReportDataSource("BankBonus", source));

            report.ReportPath = rptPath;
            report.SetParameters(parameters);
            var pdf = report.Render(renderFormat);
            return File(pdf, mimtype);
        }
        [HttpGet]
        public IActionResult CreateBonus(int? id)
        {
            Bonus bonus = new Bonus();
            if (id != null)
            {
                bonus = _bonusManager.GetById((int)id);
            }
            
            return View(bonus);
        }
        [HttpPost] 
        public IActionResult CreateBonus(Bonus bonus,string btnValue) 
        {
            if (btnValue == "Save")
            {
                var isSaved = _bonusManager.Add(bonus);
                if (isSaved)
                {
                    TempData["Success"] = "Successfully bonus added.";
                }
                else
                {
                    TempData["Error"] = "Bonus does not saved ";
                }
                
            }
            else
            {
                var check = _bonusManager.GetById(bonus.Id);

               
                    if (check != null)
                    {
                        check.Name = bonus.Name;
                        check.Percent = bonus.Percent;
                        check.Religion=bonus.Religion;
                    check.ReportText = bonus.ReportText;


                        var result = _bonusManager.Update(bonus);
                        if (result)
                        {
                            TempData["Success"] = "Successfully Update";
                        }
                        else
                        {
                            TempData["Error"] = "Failed Update";
                        }

                    }
                    else
                    {
                        TempData["Error"] = "No data found";
                    }
              
            }
            return RedirectToAction("List");
        }
       
        public IActionResult Delete(int id)
        {
            var b = _bonusManager.GetById(id);

         bool isDeleted= _bonusManager.Delete(b);

            if (isDeleted)
            {
                TempData["Delete"] = "Bonus deleted successfully.";
            }
            return RedirectToAction("List");
        }
        public IActionResult List()
        {
            ViewBag.SuccessMessage = TempData["Success"];
            ViewBag.ErrorMessage = TempData["Error"];
            ViewBag.DeleteMessage = TempData["Delete"];
            var list=_bonusManager.GetList();
            return View(list);
        }
    }
}
