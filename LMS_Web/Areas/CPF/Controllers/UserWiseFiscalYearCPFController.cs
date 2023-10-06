
using LMS_Web.Areas.CPF.Interface;
using LMS_Web.Areas.CPF.Manager;
using LMS_Web.Areas.CPF.ViewModels;
using LMS_Web.Areas.Loan.Manager;
using LMS_Web.Areas.Salary.Manager;
using LMS_Web.Areas.Settings.Manager;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Data;
using LMS_Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.NETCore;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using LMS_Web.SecurityExtension;
using Microsoft.EntityFrameworkCore.Query;
using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Settings.Interface;
using LMS_Web.Manager;
using LMS_Web.Areas.CPF.Models;
using Google.Protobuf.WellKnownTypes;

namespace LMS_Web.Areas.CPF.Controllers
{
    [Area("CPF")]
    public class UserWiseFiscalYearCPFController : Controller
    {
        private readonly FiscalYearManager _fiscalYearManager;
        private readonly WingsManager _wingsManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly CpfPercentManager _cpfPercentManager;
        private readonly CpfInfoManager _cpfInfoManager;
        InvestmentInfoController investmentInfoController;
        private readonly PRlApplicantInfoManager _pRlApplicantInfoManager;
        private readonly UserStationPermissionManager _userStationPermissionManager;
        private readonly GradeManager _gradeManager;
        private readonly FisYearInvestManager _fisYearInvestManager;
        public UserWiseFiscalYearCPFController(ApplicationDbContext dbContext, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _cpfPercentManager = new CpfPercentManager(dbContext);
            _fiscalYearManager = new FiscalYearManager(dbContext);
            _cpfInfoManager = new CpfInfoManager(dbContext);
            _pRlApplicantInfoManager = new PRlApplicantInfoManager(dbContext);
            investmentInfoController = new InvestmentInfoController(dbContext, userManager, webHostEnvironment);
            _wingsManager=new WingsManager(dbContext);
            _userStationPermissionManager=new UserStationPermissionManager(dbContext);
            _gradeManager=new GradeManager(dbContext);
            _fisYearInvestManager = new FisYearInvestManager(dbContext);
        }
        public IActionResult Index()
        {
            return View();
        }

        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public ActionResult SelfFiscalYearReport()
        {
            ViewBag.FiscalYear = _fiscalYearManager.GetList();
            return View();
        }

        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public ActionResult UserWiseFiscalCpfReport()
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
            //ViewBag.FiscalYear = new SelectList(_fiscalYearManager.GetAll(), "Id", "Value");
            ViewBag.FiscalYear = _fiscalYearManager.GetList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserWiseFiscalCpfReport( int fiscalYearId, int StationId, int? WingId, int? FromGradeId, int? ToGradeId, string AppUserId)
        {
          
            if (StationId==0 && AppUserId==null) 
            {
                AppUserId = _userManager.GetUserId(User); ;
            }
            var users = _userManager.Users.Where(c => c.IsActive && (AppUserId == null && (c.StationId == StationId && (WingId == null || c.WingId == WingId) && (FromGradeId == null || c.GradeId >= FromGradeId) && (ToGradeId == null || c.GradeId <= ToGradeId))) || c.Id == AppUserId).ToList();
            var sources = new List<UserFiscalCpfVm>();
            foreach (var item in users)
            {
                UserFiscalCpfVm data=new UserFiscalCpfVm();
                var obj = investmentInfoController.GeneralInvestmentCalculate(item.Id, fiscalYearId, null, null);
                var x = obj.ReportParameters;
                var userName = Array.Find(x, n => n.Name == "userName").Values[0];
                var result = obj.description[obj.description.Count - 1];
                var amount = result.InvestAmount;
                var interest = result.InterstAmount;
                var sum = result.Total;
                data.Name= userName;
                data.Sum = sum;
                data.Interest = interest;  
                data.InvestmentAmount= amount;
                sources.Add(data);
            }


            // var prlUser = _pRlApplicantInfoManager.GetListByUser(AppUserId);
           
            var fiscalYear = _fiscalYearManager.GetById(fiscalYearId).Value;

            var splitResult = fiscalYear.Split("-");
            int fyear = Convert.ToInt32(splitResult[0]);
            int tyear = Convert.ToInt32(splitResult[1]);

            int fmonth = 7;
            int toMonth1 = DateTime.Now.AddMonths(-1).Month;


            //int fMonth = 7;
            //int tMonth = 6;



            string renderFormat = "PDF";
            using var report = new LocalReport();
            report.EnableExternalImages = true;
           

            var parameters = new[]
            { 
                //new ReportParameter("name",userName),
                //new ReportParameter("investmentAmount",amount),
                //new ReportParameter("Interest",interest),
                //new ReportParameter("Sum",sum),
                new ReportParameter("fMonth", GetMonthName.MonthInBangla(fmonth) + "/" + string.Concat(fyear.ToString().Select(c => (char)('\u09E6' + c - '0')))),
                new ReportParameter("tMonth", GetMonthName.MonthInBangla(toMonth1) + "/" + string.Concat(tyear.ToString().Select(c => (char)('\u09E6' + c - '0')))),

                
            };
            string rptPath = $"{this._webHostEnvironment.WebRootPath}\\Reports\\UserFiscalCpf.rdlc";
            report.DataSources.Add(new ReportDataSource("DsUserFiscalCpf", sources));
            report.ReportPath = rptPath;
            report.SetParameters(parameters);
            var pdf = report.Render(renderFormat);
            return File(pdf, "application/pdf");
        }


        public IActionResult PrlApplicationList()
        {
            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];
            ViewBag.users = _userManager.Users.ToList();
            var list = _pRlApplicantInfoManager.GetList();
            return View("list");
        }
        public void AddFiscalYearInvestmentInfo()
        {
            var month = 6;
            var year = 2023;
            if (month == 6)
            {
                var fiscalYear = year - 1 + "-" + year;
            }
            var fiscalyearlist = _fisYearInvestManager.GetList();
            //var fiscalyear = _fisYearInvestManager.GetListByFiscalYear(fiscalYear);
            

            var users = _userManager.Users.Where(c => c.IsActive).ToList();
            var fiscalYearIds = fiscalyearlist.Select(c => c.FiscalYear.Id).ToList();

            var maxFiscalId = _fisYearInvestManager.GetList().Max(x => x.Id);
            var newFiscalYearId = maxFiscalId + 1;
            //var fiscalyear = _fisYearInvestManager.GetByUserAndFiscalYear(fiscalYearIds); 

                var dataList = new List<UserFiscalCpfVm>();
                foreach (var item in users)
                {
                    UserFiscalCpfVm dataVm = new UserFiscalCpfVm();

                    foreach (var fiscalYearId in fiscalYearIds)
                    {
                        if(fiscalYearId != newFiscalYearId)
                        {
                            var obj = investmentInfoController.GeneralInvestmentCalculate(item.Id, fiscalYearId, null, null);
                            var result = obj.description[obj.description.Count - 1];
                            var amount = result.InvestAmount;
                            var interest = result.InterstAmount;
                            var sum = result.Total;
                            var AppUserId = result.AppUserId;
                            var fiscalyearId = fiscalYearId;

                            dataVm.Sum = sum;
                            dataVm.Interest = interest;
                            dataVm.InvestmentAmount = amount;
                            dataVm.AppUserId = AppUserId;
                            dataVm.FiscalYearId = fiscalyearId;

                            dataList.Add(dataVm);
                        }

                        
                    }
                }


                foreach (var item in dataList)
                {
                    List<FiscalYearWiseInvestmentInfo> data = new List<FiscalYearWiseInvestmentInfo>();
                    FiscalYearWiseInvestmentInfo fiscal = new FiscalYearWiseInvestmentInfo()
                    {
                        Total = Convert.ToDecimal(item.Sum),
                        InterestAmount = Convert.ToDecimal(item.Interest),
                        InvestmentAmount = Convert.ToDecimal(item.InvestmentAmount),
                        AppUserId = item.AppUserId,
                        FiscalYearId = item.FiscalYearId
                    };
                    data.Add(fiscal);
                    _fisYearInvestManager.Add(data);
                }
        }
          


    }
}
