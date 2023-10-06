using AspNetCore.Reporting;
using LMS_Web.Areas.CPF.Manager;
using LMS_Web.Areas.CPF.ViewModels;
using LMS_Web.Areas.Salary.Manager;
using LMS_Web.Areas.Settings.Manager;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Models;
using LMS_Web.SecurityExtension;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using StationManager = LMS_Web.Areas.Salary.Manager.StationManager;

namespace LMS_Web.Areas.CPF.Controllers
{
    [Area("CPF")]
    public class CPFDeductionController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly UserFundManager _userFundManager;
        private readonly WingsManager _wingsManager;
        private readonly GradeManager _gradeManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly DeductionManager _deductionManager;
        private readonly UserDeductionManager _userDeductionManager;
        private readonly GradeWiseFixedDeductionManager _gradeWiseFixedDeductionManager;
        private readonly FixedDeductionManager _fixedDeductionManager;
        private readonly StationManager _stationManager;

        public CPFDeductionController(ApplicationDbContext db, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _userFundManager = new UserFundManager(db);
            _wingsManager = new WingsManager(db);
            _gradeManager = new GradeManager(db);
            _webHostEnvironment = webHostEnvironment;
            _deductionManager = new DeductionManager(db);
            _userDeductionManager = new UserDeductionManager(db);
            _gradeWiseFixedDeductionManager = new GradeWiseFixedDeductionManager(db);
            _fixedDeductionManager = new FixedDeductionManager(db);
            _stationManager= new StationManager(db);
        }
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult CpfDeductionReport()
        {
            ViewBag.users = _userManager.Users.ToList();
            ViewBag.Station = _stationManager.GetAll();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CpfDeductionReport(int AppuserId,int year , int month)
        {
            List<CpfDeductionVM> sources = new List<CpfDeductionVM>();

            string rptPath = $"{this._webHostEnvironment.WebRootPath}\\Reports\\CpfDeduction.rdlc";
            string mimtype = "application/pdf";
            var report = new LocalReport(rptPath);
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            var extenstion = 1;
            //parameters.Add("monthYear", MonthInBangla(month) + "/" + string.Concat(year.ToString().Select(c => (char)('\u09E6' + c - '0'))));
            report.AddDataSource("DsCpfDeduction", "");
            var result = report.Execute(RenderType.Pdf, extenstion, parameters, mimtype);
            return File(result.MainStream, "application/pdf");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
