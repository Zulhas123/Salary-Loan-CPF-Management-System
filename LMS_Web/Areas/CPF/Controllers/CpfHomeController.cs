using LMS_Web.Areas.CPF.Dataset;
using LMS_Web.Areas.CPF.Interface;
using LMS_Web.Areas.CPF.Manager;
using LMS_Web.Areas.CPF.Models;
using LMS_Web.Areas.CPF.ViewModels;
using LMS_Web.Areas.Loan.Interface;
using LMS_Web.Areas.Loan.Manager;
using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Manager;
using LMS_Web.Areas.Settings.Interface;
using LMS_Web.Areas.Settings.Manager;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Controllers;
using LMS_Web.Data;
using LMS_Web.Interface.Manager;
using LMS_Web.Manager;
using LMS_Web.Migrations;
using LMS_Web.Models;
using LMS_Web.SecurityExtension;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Reporting.NETCore;
using Microsoft.ReportingServices.Interfaces;
using MySql.Data.MySqlClient;
using Quartz.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using static System.Net.Mime.MediaTypeNames;

namespace LMS_Web.Areas.CPF.Controllers
{
    [Area("CPF")]
    public class CpfHomeController : Controller
    {
        private CpfPercentManager cpfPercentManager;
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private IWebHostEnvironment webHostEnvironment;
        private IConfiguration configuration;
        private readonly UserSpecificAllowanceManager _userSpecificAllowanceManager;
        private readonly CpfInfoManager _cpfInfoManager;
        private readonly LoanInstallmentInfoManager _loanInstallmentInfoManager;
        private PRlApplicantInfoManager _plApplicantInfoManager;
        private readonly InvestmentInfoManager _investmentInfoManager;
        private readonly FisYearInvestManager _fisYearInvestManager;

        private IWingsManager _wingsManager;
        private IUserStationPermissionManager _userStationPermissionManager;
        private IGradeManager _gradeManager;
        private IFiscalYearManager _fiscalYearManager;

        private InvestmentInfoController investCon;

        public CpfHomeController(ILogger<HomeController> logger, IConfiguration _configuration, IWebHostEnvironment _environment, ApplicationDbContext db, UserManager<AppUser> userManager)
        {
            cpfPercentManager = new CpfPercentManager(db);
            _userSpecificAllowanceManager = new UserSpecificAllowanceManager(db);
            _cpfInfoManager = new CpfInfoManager(db);
            _loanInstallmentInfoManager = new LoanInstallmentInfoManager(db);
            _plApplicantInfoManager = new PRlApplicantInfoManager(db);
            _investmentInfoManager = new InvestmentInfoManager(db);
            _fisYearInvestManager = new FisYearInvestManager(db);
            _logger = logger;
            _context = db;
            _userManager = userManager;
            webHostEnvironment = _environment;
            configuration = _configuration;
            investCon = new InvestmentInfoController(db, userManager, _environment);
            _wingsManager = new WingsManager(db);
            _userStationPermissionManager = new UserStationPermissionManager(db);
            _gradeManager = new GradeManager(db);
            _fiscalYearManager = new FiscalYearManager(db);
        }
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        //[MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult Edit(int? id)
        {
            CpfPercent cpfPercent = new CpfPercent();
            if (id != null)
            {
                cpfPercent = cpfPercentManager.GetById((int)id);
            }

            return View(cpfPercent);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CpfPercent c)
        {

            var Child = cpfPercentManager.GetById(c.Id);

            if (Child != null)
            {
                //Child.Id = c.Id;
                Child.Percent = c.Percent;
                //Child.Name = c.Name;


                var result = cpfPercentManager.Update(c);
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

        public IActionResult CPFDashboard()
        {
            return View();
        }
        public IActionResult List()
        {
            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];
            var list = cpfPercentManager.GetList();
            return View(list);
        }


        public IActionResult ConvertToBanglaNumber()
        {
            var emplo = _context.Users.ToList();

            foreach (var x in emplo)
            {
                string bengali_text = string.Concat(x.EmployeeCode.ToString().Select(c => (char)('\u09E6' + c - '0')));
                x.EmployeeCodeBangla = bengali_text;
                _context.Users.Update(x);
                _context.SaveChanges();

            }
            return Ok();
        }

        public IActionResult NameWithDesignation()
        {
            var employees = _context.Users.Include(c => c.Designation).ToList();
            foreach (var employee in employees)
            {
                var designation = _context.Designation.FirstOrDefault(c => c.Id == employee.DesignationId);
                if (designation != null)
                {
                    //var nameWithDesignation = employee.EmployeeCode + employee.FullName + designation.DesignationName;
                    //employee.NamewithDesignation = nameWithDesignation;
                    _context.Users.Update(employee);
                }
            }
            _context.SaveChanges();
            return Ok();
        }


        private List<InvestmentVm> InvestmentVmAllData = new List<InvestmentVm>();
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult InterestReport()
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
            ViewBag.FiscalYear = _fiscalYearManager.GetList();
            return View();

        }
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        [HttpPost]
        public IActionResult InterestReport(int StationId, int? WingId, int? FromGradeId, int? ToGradeId, int fiscalYear)
        {

            List<InvestmentMainDs> mainDsList = new List<InvestmentMainDs>();

            var allusers = _userManager.Users.Where(c => c.IsActive && (c.StationId == StationId && (WingId == null || c.WingId == WingId) && (FromGradeId == null || c.GradeId >= FromGradeId) && (ToGradeId == null || c.GradeId <= ToGradeId))).Include(c => c.Designation).ToList();
            //var users = _userManager.Users.Where(c => c.IsActive && c.StationId == 1 && c.WingId == 1).ToList();
            var users = allusers.OrderBy(c => c.Designation.DisgOrder);
            foreach (var item in users)
            {
                InvestmentMainDs obj = new InvestmentMainDs();


                var data = investCon.GeneralInvestmentCalculate(item.Id, fiscalYear, null, null);
                InvestmentVmAllData.AddRange(data.sources);
                var x = data.ReportParameters;

                obj.Id = item.Id;
                obj.Name = Array.Find(x, n => n.Name == "userName")?.Values[0];
                obj.Comment = Array.Find(x, n => n.Name == "comments")?.Values[0];
                obj.FMonth = Array.Find(x, n => n.Name == "fMonth")?.Values[0];
                obj.TMonth = Array.Find(x, n => n.Name == "tMonth")?.Values[0];
                obj.PrlMessage = Array.Find(x, n => n.Name == "PrlMessage")?.Values[0] ?? "";
                obj.FinalTotalInvest = Array.Find(x, n => n.Name == "finaltotalInvest")?.Values[0];
                obj.FinalTotalInterest = Array.Find(x, n => n.Name == "finaltotalInte")?.Values[0];
                obj.FinalTotalContribution = Array.Find(x, n => n.Name == "finalTotalContri")?.Values[0];
                mainDsList.Add(obj);


            }




            string renderFormat = "PDF";
            using var report = new LocalReport();
            report.EnableExternalImages = true;

            string rptPath = $"{this.webHostEnvironment.WebRootPath}\\Reports\\InvestmentMainRpt.rdlc";
            report.DataSources.Add(new ReportDataSource("InvestmentMainRpt", mainDsList));

            report.ReportPath = rptPath;

            report.SubreportProcessing += new SubreportProcessingEventHandler(SubReportProcessing);

            var pdf = report.Render(renderFormat);
            byte[] renderedBytes = report.Render(renderFormat);
            return File(pdf, "application/pdf");

        }

        void SubReportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            var id = e.Parameters["AppUserId"].Values[0].ToString();
            var topSource = InvestmentVmAllData.Where(c => c.AppUserId == id).ToList();
            var nextSource = investCon.investmentDescriptionVmsList.Where(c => c.AppUserId == id).ToList();
            string rptPath = $"{this.webHostEnvironment.WebRootPath}\\Reports\\Investment.rdlc";
            e.DataSources.Add(new ReportDataSource("DsInvestment", topSource));
            e.DataSources.Add(new ReportDataSource("InvestDes", nextSource));
            // e.Parameters.Add(new ReportParameter("SubReportParameter", "ParameterValue"));
            // e.ReportPath = rptPath;
            //e.DataSources.(data.ReportParameters);

        }
        public void PrintSub(int month, int year)
        {
            List<InvestmentInfo> updateList = new List<InvestmentInfo>();
            List<InvestmentInfo> a = new List<InvestmentInfo>();
            var getCpf = _cpfInfoManager.GetListByMonth(year, month);
            var allInvest = _investmentInfoManager.GetList();
            var iuvestDate = new DateTime(year, month, 1).AddMonths(1);

            foreach (var item in getCpf)
            {
                if (item.SelfContribution == 0)
                {
                    var getInvest = allInvest.FirstOrDefault(c => c.AppUserId == item.AppUserId && c.Month == iuvestDate.Month && c.Year == iuvestDate.Year);
                    if (getInvest != null)
                    {
                        getInvest.TotalInvestment = 0;
                        getInvest.InvestmentAmount = 0;
                        updateList.Add(getInvest);
                    }
                    else
                    {
                        a.Add(getInvest);
                    }
                }
            }

            _investmentInfoManager.Update(updateList);
            //string renderFormat = "PDF";
            //using var report = new LocalReport();
            //report.EnableExternalImages = true;
            //var data = investCon.GeneralInvestmentCalculate("00d1cbae-13e1-4d77-accd-5d68e233bdd0", 1, null, null);

            //string rptPath = $"{this.webHostEnvironment.WebRootPath}\\Reports\\TestMainRpt.rdlc";
            //report.DataSources.Add(new ReportDataSource("DsInvestment", data.sources));
            //report.DataSources.Add(new ReportDataSource("InvestDes", investCon.investmentDescriptionVmsList));

            ////report.DataSources.Add(new ReportDataSource("InvestDes", investmentDescriptionVmsList));
            //var parameters = new[]
            //   {

            //        new ReportParameter("AppUserId", "00d1cbae-13e1-4d77-accd-5d68e233bdd0")


            //    };
            //report.ReportPath = rptPath;
            //report.SetParameters(parameters);

            ////subreport
            //// report.SubreportProcessing += new SubreportProcessingEventHandler(SubReportProcessing);

            //var pdf = report.Render(renderFormat);
            //byte[] renderedBytes = report.Render(renderFormat);
            //return File(pdf, "application/pdf");
        }

        public IActionResult UpdateCpfData()
        {
            string sourceConnectionString = "server=127.0.0.1; port=3306; database=bjri_salary; user=root; password=123456; Persist Security Info=False; Connect Timeout=300; Convert Zero Datetime=True";
            string targetConnectionString = "server=192.168.0.160; port=3306; database=bjri_salary; user=root; password=Asad@123; Persist Security Info=False; Connect Timeout=300; CharSet=utf8mb4";
           
            // Create connection objects
            using MySqlConnection sourceConnection = new MySqlConnection(sourceConnectionString);
            using MySqlConnection targetConnection = new MySqlConnection(targetConnectionString);

            // Open the connections
            sourceConnection.Open();
            targetConnection.Open();

            // Retrieve data from the source database
            string selectQuery = "SELECT * FROM bjri_salary.cpfinfo where year=2022 and month=6;";
            using MySqlCommand selectCommand = new MySqlCommand(selectQuery, sourceConnection);
            using MySqlDataReader reader = selectCommand.ExecuteReader();

            // Update data in the target database
            string updateQuery = "UPDATE bjri_salary.cpfinfo SET BasicSalary = @BasicSalary, SelfContribution = @SelfContribution, GovtContribution = @GovtContribution,ArrearsBasic = @ArrearsBasic,Month = @Month,Year = @Year,TotalContribution = @TotalContribution WHERE year = 2022 and month = 6 and AppUserId=@UserId";
            using MySqlCommand updateCommand = new MySqlCommand(updateQuery, targetConnection);

            while (reader.Read())
            {
                // Extract data from the reader
                string basicSalary = reader.GetString("BasicSalary");
                string selfContribution = reader.GetString("SelfContribution");
                string govtContribution = reader.GetString("GovtContribution");
                string arrearsBasic = reader.GetString("ArrearsBasic");
                string month = reader.GetString("Month");
                string year = reader.GetString("Year");
                string totalContribution = reader.GetString("TotalContribution");
                var UserId = reader.GetString("AppUserId");
                // Set parameters for the update command

                
                updateCommand.Parameters.Clear();
                updateCommand.Parameters.AddWithValue("@UserId", UserId);
                updateCommand.Parameters.AddWithValue("@BasicSalary", basicSalary);
                updateCommand.Parameters.AddWithValue("@SelfContribution", selfContribution);
                updateCommand.Parameters.AddWithValue("@GovtContribution", govtContribution);
                updateCommand.Parameters.AddWithValue("@ArrearsBasic", arrearsBasic);
                updateCommand.Parameters.AddWithValue("@Month", month);
                updateCommand.Parameters.AddWithValue("@Year", year);
                updateCommand.Parameters.AddWithValue("@TotalContribution", totalContribution);

                // Execute the update command

                updateCommand.ExecuteNonQuery();
            }

            // Close the connections
            sourceConnection.Close();
            targetConnection.Close();

            return Content("Data transfer completed.");
        }



    }
}
