using LMS_Web.Areas.CPF.Interface;
using LMS_Web.Areas.CPF.Manager;
using LMS_Web.Areas.CPF.Models;
using LMS_Web.Areas.CPF.ViewModels;
using LMS_Web.Areas.Loan;
using LMS_Web.Areas.Loan.Interface;
using LMS_Web.Areas.Loan.Manager;
using LMS_Web.Areas.Loan.Models;
using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Manager;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Areas.Salary.ViewModels;
using LMS_Web.Areas.Settings.Manager;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Areas.Settings.ViewModels;
using LMS_Web.Data;
using LMS_Web.Manager;
using LMS_Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LMS_Web.Common;
using Microsoft.Reporting.NETCore;
using StationManager = LMS_Web.Areas.Salary.Manager.StationManager;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using Microsoft.ReportingServices.Interfaces;
using LMS_Web.SecurityExtension;
using LMS_Web.Areas.Settings.Interface;
using System.Drawing;
using MySql.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LMS_Web.Areas.CPF.Controllers
{
    [Area("CPF")]
    public class CpfInfoController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly UserWiseLoanManager _userWiseLoanManager;
        private readonly CpfInfoManager _cpfInfoManager;
        private readonly WingsManager _wingsManager;
        private readonly StationManager _stationManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly FiscalYearManager _fiscalYearManager;
        private readonly PayScaleManager _payScaleManager;
        private readonly CpfPercentManager _cpfPercentManager;
        private readonly GradeManager _gradeManager;
        private readonly UserSpecificAllowanceManager _userSpecificAllowanceManager;
        private readonly LoanInstallmentInfoManager _loanInstallmentInfoManager;
        private readonly IUserStationPermissionManager _userStationPermissionManager;
        private readonly IProcessSalaryManager processSalaryManager;
        public CpfInfoController(ApplicationDbContext dbContext, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _userWiseLoanManager = new UserWiseLoanManager(dbContext);
            _cpfInfoManager = new CpfInfoManager(dbContext);
            _wingsManager = new WingsManager(dbContext);
            _stationManager = new StationManager(dbContext);
            _fiscalYearManager = new FiscalYearManager(dbContext);
            _payScaleManager = new PayScaleManager(dbContext);
            _cpfPercentManager = new CpfPercentManager(dbContext);
            _gradeManager = new GradeManager(dbContext);
            _userSpecificAllowanceManager = new UserSpecificAllowanceManager(dbContext);
            _loanInstallmentInfoManager = new LoanInstallmentInfoManager(dbContext);
            _userStationPermissionManager = new UserStationPermissionManager(dbContext);
            processSalaryManager = new ProcessSalaryManager(dbContext);
        }
        //[MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]

        //**Start************** CPF Report for all User With all Data*********************
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult Calculate()
        {
            ViewBag.Wings = _wingsManager.GetWings();
            ViewBag.Stations = _userStationPermissionManager.UserWiseLoadStation(_userManager.GetUserId(User));
            ViewBag.Grade = _gradeManager.GetList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Calculate(int year, int month, int StationId, int WingId, int? FromGradeId, int? ToGradeId)
        {
            var cpfstatement = CalculateCPF(year, month, StationId, WingId, FromGradeId, ToGradeId);
            var source = cpfstatement.sources;
            var parassm = cpfstatement.ReportParameters;
            string renderFormat = "PDF";
            string mimtype = "application/pdf";
            using var report = new LocalReport();
            report.EnableExternalImages = true;
            string rptPath = $"{this._webHostEnvironment.WebRootPath}\\Reports\\CpfReport.rdlc";
            report.DataSources.Add(new ReportDataSource("dsCpfReport", source));
            report.ReportPath = rptPath;
            report.SetParameters(parassm);
            var pdf = report.Render(renderFormat);
            return File(pdf, mimtype);

        }
        //**END************** CPF Report for all User With all Data*********************




        //Start****CPF Statement Report Show Only Total for All User At a Glance ***********
        
        [HttpGet]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult CpfStatement() //InvestMent Monthly Statement
        {


            ViewBag.Wings = _wingsManager.GetWings();
            ViewBag.Stations = _userStationPermissionManager.UserWiseLoadStation(_userManager.GetUserId(User));
            ViewBag.Grade = _gradeManager.GetList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CpfStatement(int fyear, int fmonth, int tyear, int tmonth, int StationId, int? WingId, int? FromGradeId, int? ToGradeId)
        {
            var users = _userManager.Users.Where(c =>
            c.IsActive && (c.StationId == StationId) && (WingId == null || c.WingId == WingId) && (FromGradeId == null || c.GradeId >= FromGradeId) &&
            (ToGradeId == null || c.GradeId <= ToGradeId)).Include(c=>c.Station).Include(c => c.Designation).Include(c => c.Wing);
            var getwing = _wingsManager.GetWingById(WingId);
            var getStation = _stationManager.GetById(StationId);
            var cpfLoans = _loanInstallmentInfoManager.GetBetweenDate(fyear, fmonth, tyear, tmonth,  StationId,   WingId,  FromGradeId,  ToGradeId);
            var data = _cpfInfoManager.GetListByMonthStation(fyear, fmonth, tyear, tmonth, StationId, WingId, FromGradeId, ToGradeId).OrderBy(c=>c.Year);

            // var j = cpfLoans.Where(c => c.Year == 2023 && c.Month == 1 && c.LoanHeadId == 1).ToList().OrderBy(c=>c.Id).ToList();

            
            var result = data.GroupBy(s => new { s.Year, s.Month })
           .Select(g =>
           new CpfVM
           {
               BasicSalary = g.Sum(x => Math.Round(Convert.ToDecimal(x.BasicSalary), 2)).ToString(),
               SelfContribution = g.Sum(x => Math.Round(Convert.ToDecimal(x.SelfContribution), 2)).ToString(),
               GovtContribution = g.Sum(x => Math.Round(Convert.ToDecimal(x.GovtContribution), 2)).ToString(),
               ArrearsBasic = g.Sum(x => Math.Round(Convert.ToDecimal(x.ArrearsBasic), 2)).ToString(),
               TotalContribution = g.Sum(x => Math.Round(Convert.ToDecimal(x.TotalContribution), 2)).ToString(),
               Month = GetMonthName.MonthInBangla(g.Key.Month) + "/" + g.Key.Year,         
               CpfFirstLoan = cpfLoans.Where(c=>c.LoanHeadId==1 && c.Year== g.Key.Year && c.Month== g.Key.Month).Sum(c=>c.UserWiseLoan.CapitalDeductionAmount).ToString(),
               CpfSecondLoan = cpfLoans.Where(c=> c.LoanHeadId == 2 &&  c.Year== g.Key.Year && c.Month== g.Key.Month).Sum(c=>c.UserWiseLoan.CapitalDeductionAmount).ToString(),
           }).ToList();
            decimal basicSalary = 0;
            decimal SelfCon = 0;
            decimal GovtCon = 0;
            decimal arrea = 0;
            decimal TotalCon = 0;
            decimal Cpf1 = 0;
            decimal Cpf2 = 0;
            foreach (var c in result)
            {

                basicSalary += Convert.ToDecimal(c.BasicSalary);
                SelfCon+= Convert.ToDecimal(c.SelfContribution);
                GovtCon += Convert.ToDecimal(c.GovtContribution);
                arrea += Convert.ToDecimal(c.ArrearsBasic);
                TotalCon += Convert.ToDecimal(c.TotalContribution);
                Cpf1 += Convert.ToDecimal(c.CpfFirstLoan);
                Cpf2 += Convert.ToDecimal(c.CpfSecondLoan);
            }

            var fromMonth = GetMonthName.MonthInBangla(fmonth) + "/" + string.Concat(fyear.ToString().Select(c => (char)('\u09E6' + c - '0')));
            var toMonth = GetMonthName.MonthInBangla(tmonth) + "/" + string.Concat(tyear.ToString().Select(c => (char)('\u09E6' + c - '0')));
            string yearMonth =  fromMonth + " হতে "+ toMonth;

            string gradeName = "";
            var fgrade = string.Concat(FromGradeId.ToString().Select(c => (char)('\u09E6' + c - '0')));
            var tgrade = string.Concat(ToGradeId.ToString().Select(c => (char)('\u09E6' + c - '0')));
            if (FromGradeId != null && ToGradeId != null)
            {
                gradeName = fgrade + "থেকে" + tgrade;
            }
            else
            {
                gradeName = "সকল";
            }


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

            var parameters = new[]
            {
                    new ReportParameter("wing", getwing != null ? getwing.Name.ToString() : ""),
                    new ReportParameter("Station",getStation.Name.ToString()),
                    new ReportParameter("gradeName",gradeName.ToString()),
                    new ReportParameter("yearMonth",yearMonth.ToString()),
                    new ReportParameter("basicSalary",basicSalary.ToString()),
                    new ReportParameter("SelfCon",SelfCon.ToString()),
                    new ReportParameter("GovtCon",GovtCon.ToString()),
                    new ReportParameter("arrea",arrea.ToString()),
                    new ReportParameter("TotalCon",TotalCon.ToString()),
                    new ReportParameter("Cpf1",Cpf1.ToString()),
                    new ReportParameter("Cpf2",Cpf2.ToString()),
             };

            string renderFormat = "PDF";
            string mimtype = "application/pdf";
            using var report = new LocalReport();
            report.EnableExternalImages = true;
            string rptPath = $"{this._webHostEnvironment.WebRootPath}\\Reports\\CpfStatement.rdlc";
            report.DataSources.Add(new ReportDataSource("CpfStatement", result));
            report.ReportPath = rptPath;
            report.SetParameters(parameters);
            var pdf = report.Render(renderFormat);
            return File(pdf, mimtype);
        }

        //END****CPF Statement Report Show Only Total for All User At a Glance ***********

        //*********************************************************************************//

        // ** Start ***** Common Method Used for  General CPF & CPF Statement Report ******
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        private Cpfstatement CalculateCPF(int year, int month, int StationId, int? WingId, int? FromGradeId, int? ToGradeId)
        {
            //var users = _userManager.Users.Where(c =>
            //  c.IsActive && (c.StationId == StationId) && (WingId == null || c.WingId == WingId) && (FromGradeId == null || c.GradeId >= FromGradeId) &&
            //  (ToGradeId == null || c.GradeId <= ToGradeId)).Include(c=>c.Station).Include(c => c.Wing).Include(c => c.Designation).OrderBy(c => c.Designation.DisgOrder);
            //var getwing = _wingsManager.GetWingById(WingId);
            //var getStation= _stationManager.GetById(StationId);

            var users = _userManager.Users.Where(c =>
            c.IsActive && (c.StationId == StationId) && (WingId == 0 || c.WingId == WingId) && (FromGradeId == null || c.GradeId >= FromGradeId) &&
            (ToGradeId == null || c.GradeId <= ToGradeId)).Include(c => c.Designation).Include(c => c.Wing).OrderBy(c => c.Designation.DisgOrder);
            var getwing = _wingsManager.GetWingById(WingId);
            var getStation = _stationManager.GetById(StationId);



            try
            {
                List<CpfVM> sources = new List<CpfVM>();
                var cpfInfo = _cpfInfoManager.GetListByMonth(year, month);
                var loans = _loanInstallmentInfoManager.GetCurrentMonthCpfLoan(year, month);
                decimal selfSum = 0;
                decimal govtSum = 0;
                decimal areaSum = 0;
                decimal totContr = 0;
                decimal basictotal = 0;
                decimal CpfFirstsum = 0;
                decimal Cpfsecdsum = 0;
                //var cpfFirst = loans.Where(c => c.LoanHeadId == 1 && c.UserWiseLoan.AppUsers.StationId==1).ToList().Sum(c=>c.UserWiseLoan.CapitalDeductionAmount);
                foreach (var user in users)
                {
                    if (user.UserName == "01789377312")
                    {
                        continue;
                    }
                    var item = cpfInfo.FirstOrDefault(c => c.AppUserId == user.Id);
                    CpfVM obj = new CpfVM();
                    if (item == null)
                    {
                        continue;
                    }

                    selfSum += item?.SelfContribution ?? 0;
                    govtSum += item?.GovtContribution ?? 0;
                    areaSum += item?.ArrearsBasic ?? 0;
                    totContr += item?.TotalContribution ?? 0;
                    basictotal += item?.BasicSalary ?? 0;
                    CpfFirstsum += loans.FirstOrDefault(c => c.LoanHeadId == 1 && c.AppUserId == user.Id)?.UserWiseLoan?.CapitalDeductionAmount ?? 0;
                    Cpfsecdsum += loans.FirstOrDefault(c => c.LoanHeadId == 2 && c.AppUserId == user.Id)?.UserWiseLoan?.CapitalDeductionAmount ?? 0;



                    //obj.ArrearsBasic = string.Concat((item?.ArrearsBasic).ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    //obj.BasicSalary = string.Concat(item?.BasicSalary.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    //obj.CpfFirstLoan = string.Concat((loans.FirstOrDefault(c => c.LoanHeadId == 1 && c.AppUserId == item?.AppUserId)?.UserWiseLoan?.CapitalDeductionAmount ?? 0).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    //obj.CpfSecondLoan = string.Concat((loans.FirstOrDefault(c => c.LoanHeadId == 2 && c.AppUserId == item?.AppUserId)?.UserWiseLoan?.CapitalDeductionAmount ?? 0).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    //obj.EmployeeName = user.EmployeeCodeBangla + "-" + user.FullNameBangla + "," + user.Designation.Name;
                    //obj.GovtContribution = string.Concat(item?.GovtContribution.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    //obj.GrandTotal = string.Concat(item?.GrandTotal.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    //obj.SelfContribution = string.Concat(item?.SelfContribution.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    //obj.TotalContribution = string.Concat(item?.TotalContribution.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");


                    obj.ArrearsBasic = (item?.ArrearsBasic).ToString();
                    obj.BasicSalary = (item?.BasicSalary).ToString();
                    obj.CpfFirstLoan = (loans.FirstOrDefault(c => c.LoanHeadId == 1 && c.AppUserId == item?.AppUserId)?.UserWiseLoan?.CapitalDeductionAmount ?? 0).ToString("0.00");
                    obj.CpfSecondLoan = (loans.FirstOrDefault(c => c.LoanHeadId == 2 && c.AppUserId == item?.AppUserId)?.UserWiseLoan?.CapitalDeductionAmount ?? 0).ToString("0.00");
                    obj.EmployeeName = user.EmployeeCodeBangla + "-" + user.FullNameBangla + "," + user.Designation.Name;
                    obj.GovtContribution = (item?.GovtContribution).ToString();
                    obj.GrandTotal = (item?.GrandTotal).ToString();
                    obj.SelfContribution = (item?.SelfContribution).ToString();
                    obj.TotalContribution = (item?.TotalContribution).ToString();

                    sources.Add(obj);

                }

                var selfAreaTotal = (selfSum + areaSum);
                var Cpf1and2Total = (CpfFirstsum + Cpfsecdsum);
                var grandTotal = (selfAreaTotal + Cpf1and2Total);

                var grandTotalBn = NumberToWordConverter.ConvertToWordsBangla(grandTotal.ToString());
                var Cpf1and2TotalBn = NumberToWordConverter.ConvertToWordsBangla(Cpf1and2Total.ToString());
                var selfAreaTotalBn = NumberToWordConverter.ConvertToWordsBangla(selfAreaTotal.ToString());

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
                string gradeName = "";
                var fgrade = string.Concat(FromGradeId.ToString().Select(c => (char)('\u09E6' + c - '0')));
                var tgrade = string.Concat(ToGradeId.ToString().Select(c => (char)('\u09E6' + c - '0')));
                if (FromGradeId != null && ToGradeId != null)
                {
                    gradeName = fgrade + "থেকে" + tgrade;
                }
                else
                {
                    gradeName = "সকল";
                }
                var parameters = new[]
                {
                    new ReportParameter("monthYear",GetMonthName.MonthInBangla(month) + "/" +
                        string.Concat(year.ToString().Select(c => (char)('\u09E6' + c - '0')))),
                    new ReportParameter("SeTotal",
                        string.Concat(selfSum.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")),
                    new ReportParameter("govTotal",
                        string.Concat(govtSum.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")),
                    new ReportParameter("AreaTotal",
                        string.Concat(areaSum.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")),
                    new ReportParameter("ConTotal",
                        string.Concat(totContr.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")),
                    new ReportParameter("BasicTotal",
                        string.Concat(basictotal.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")),
                    new ReportParameter("CpfFirstTotal",
                        string.Concat(CpfFirstsum.ToString().Select(c => (char)('\u09E6' + c - '0')))
                            .Replace("৤", ".")),
                    new ReportParameter("CpfSecTotal",
                        string.Concat(Cpfsecdsum.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")),
                    new ReportParameter("selfAreaTotal",
                        string.Concat(selfAreaTotal.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")),
                    new ReportParameter("Cpf1and2Total",
                        string.Concat(Cpf1and2Total.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")),
                    new ReportParameter("grandTotal",
                        string.Concat(grandTotal.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")),

                   new ReportParameter("grandTotalBn", grandTotalBn),
                   new ReportParameter("selfAreaTotalBn", selfAreaTotalBn),
                   new ReportParameter("Cpf1and2TotalBn", Cpf1and2TotalBn),
                   //new ReportParameter("wing",getwing.Name.ToString()),
                   new ReportParameter("wing", getwing != null ? getwing.Name.ToString() : ""),
                   new ReportParameter("gradeName",gradeName.ToString()),
                   new ReportParameter("Station",getStation.Name.ToString()),
                };


                Cpfstatement cpfstatement = new Cpfstatement();
                cpfstatement.sources = sources;
                cpfstatement.ReportParameters = parameters;
                return cpfstatement;

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        // **END ***** Common Method Used for  General CPF & CPF Statement Report ******


        //** Start *** Self CPF Report Show who login the page****************
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult UserSpcificCpfReport()
        {
            var users = _userManager.Users.Include(c => c.Designation).Where(c => c.IsActive).ToList().Select(s => new
            {
                Text = s.EmployeeCode + "-" + s.FullName + "_" + s.Designation.Name,
                Value = s.Id

            }).ToList();
            ViewBag.Users = new SelectList(users, "Value", "Text");
            ViewBag.Wings = _wingsManager.GetWings();
            ViewBag.Station = _stationManager.GetAll();
            ViewBag.Grade = _gradeManager.GetList();
            return View();
        }
        [HttpPost]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult UserSpcificCpfReport(int year, int month, string AppUserId)
        {
            List<CpfVM> sources = new List<CpfVM>();
            var cpfInfo = _cpfInfoManager.GetListByMonth(year, month);
            var loans = _loanInstallmentInfoManager.GetCurrentMonthCpfLoan(year, month);
            decimal selfSum = 0;
            decimal govtSum = 0;
            decimal areaSum = 0;
            decimal totContr = 0;
            decimal basictotal = 0;
            decimal CpfFirstsum = 0;
            decimal Cpfsecdsum = 0;

            try
            {
                var item = cpfInfo.FirstOrDefault(c => c.AppUserId == AppUserId);
                CpfVM obj = new CpfVM();
                if (item == null)
                {
                    item = new CpfInfo();
                }
                selfSum += item?.SelfContribution ?? 0;
                govtSum += item?.GovtContribution ?? 0;
                areaSum += item?.ArrearsBasic ?? 0;
                totContr += item?.TotalContribution ?? 0;
                basictotal += item?.BasicSalary ?? 0;
                CpfFirstsum += loans.FirstOrDefault(c => c.LoanHeadId == 1 && c.AppUserId == item?.AppUserId)?.UserWiseLoan?.CapitalDeductionAmount ?? 0;
                Cpfsecdsum += loans.FirstOrDefault(c => c.LoanHeadId == 2 && c.AppUserId == item?.AppUserId)?.UserWiseLoan?.CapitalDeductionAmount ?? 0;



                obj.ArrearsBasic = string.Concat((item?.ArrearsBasic).ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                obj.BasicSalary = string.Concat(item?.BasicSalary.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                obj.CpfFirstLoan = string.Concat((loans.FirstOrDefault(c => c.LoanHeadId == 1 && c.AppUserId == item?.AppUserId)?.UserWiseLoan?.CapitalDeductionAmount ?? 0).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                obj.CpfSecondLoan = string.Concat((loans.FirstOrDefault(c => c.LoanHeadId == 2 && c.AppUserId == item?.AppUserId)?.UserWiseLoan?.CapitalDeductionAmount ?? 0).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                // obj.EmployeeName = users.EmployeeCodeBangla + "-" + user.FullNameBangla + "," + user.Designation.Name;
                obj.GovtContribution = string.Concat(item?.GovtContribution.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                obj.GrandTotal = string.Concat(item?.GrandTotal.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                obj.SelfContribution = string.Concat(item?.SelfContribution.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                obj.TotalContribution = string.Concat(item?.TotalContribution.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");

                sources.Add(obj);
                var getUser = _userManager.Users.Include(c => c.Designation).Include(c => c.Wing)
               .FirstOrDefault(c => c.Id == AppUserId);
                string renderFormat = "PDF";
                string mimtype = "application/pdf";
                using var report = new LocalReport();
                report.EnableExternalImages = true;

                string rptPath = $"{this._webHostEnvironment.WebRootPath}\\Reports\\SelfCpfReport.rdlc";

                var parameters = new[]
                {
                     new ReportParameter("name", getUser.EmployeeCodeBangla + "," + getUser.FullNameBangla + "," + getUser.Designation.Name+"," + getUser.Wing.Name),
                     new ReportParameter("monthYear",GetMonthName.MonthInBangla(month) + "/" +
                     string.Concat(year.ToString().Select(c => (char)('\u09E6' + c - '0')))),
                     
                };

                report.DataSources.Add(new ReportDataSource("SelfCpf", sources));

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
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult SelfCPFReport()
        {
            ViewBag.users = _userManager.Users.ToList();
            ViewBag.Wings = _wingsManager.GetWings();
            ViewBag.Station = _stationManager.GetAll();
            ViewBag.Grade = _gradeManager.GetList();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelfCPFReport(int year, int month)
        {
           
                var Loginguser = _userManager.GetUserId(User);

                List<CpfVM> sources = new List<CpfVM>();
                var cpfInfo = _cpfInfoManager.GetListByMonth(year, month);
                var loans = _loanInstallmentInfoManager.GetCurrentMonthCpfLoan(year, month);
                decimal selfSum = 0;
                decimal govtSum = 0;
                decimal areaSum = 0;
                decimal totContr = 0;
                decimal basictotal = 0;
                decimal CpfFirstsum = 0;
                decimal Cpfsecdsum = 0;


                try
                {
                    var item = cpfInfo.FirstOrDefault(c => c.AppUserId == Loginguser);
                    CpfVM obj = new CpfVM();
                    if (item == null)
                    {
                        item = new CpfInfo();
                    }
                    selfSum += item?.SelfContribution ?? 0;
                    govtSum += item?.GovtContribution ?? 0;
                    areaSum += item?.ArrearsBasic ?? 0;
                    totContr += item?.TotalContribution ?? 0;
                    basictotal += item?.BasicSalary ?? 0;
                    CpfFirstsum += loans.FirstOrDefault(c => c.LoanHeadId == 1 && c.AppUserId == item?.AppUserId)?.UserWiseLoan?.CapitalDeductionAmount ?? 0;
                    Cpfsecdsum += loans.FirstOrDefault(c => c.LoanHeadId == 2 && c.AppUserId == item?.AppUserId)?.UserWiseLoan?.CapitalDeductionAmount ?? 0;



                    obj.ArrearsBasic = string.Concat((item?.ArrearsBasic).ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    obj.BasicSalary = string.Concat(item?.BasicSalary.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    obj.CpfFirstLoan = string.Concat((loans.FirstOrDefault(c => c.LoanHeadId == 1 && c.AppUserId == item?.AppUserId)?.UserWiseLoan?.CapitalDeductionAmount ?? 0).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    obj.CpfSecondLoan = string.Concat((loans.FirstOrDefault(c => c.LoanHeadId == 2 && c.AppUserId == item?.AppUserId)?.UserWiseLoan?.CapitalDeductionAmount ?? 0).ToString("0.00").Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    // obj.EmployeeName = users.EmployeeCodeBangla + "-" + user.FullNameBangla + "," + user.Designation.Name;
                    obj.GovtContribution = string.Concat(item?.GovtContribution.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    obj.GrandTotal = string.Concat(item?.GrandTotal.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    obj.SelfContribution = string.Concat(item?.SelfContribution.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                    obj.TotalContribution = string.Concat(item?.TotalContribution.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");

                    sources.Add(obj);
                    var getUser = _userManager.Users.Include(c => c.Designation).Include(c => c.Wing)
                   .FirstOrDefault(c => c.Id == Loginguser);
                    string renderFormat = "PDF";
                    string mimtype = "application/pdf";
                    using var report = new LocalReport();
                    report.EnableExternalImages = true;

                    string rptPath = $"{this._webHostEnvironment.WebRootPath}\\Reports\\SelfCpfReport.rdlc";

                    var parameters = new[]
                    {
                     new ReportParameter("name", getUser.EmployeeCodeBangla + "," + getUser.FullNameBangla + "," + getUser.Designation.Name+"," + getUser.Wing.Name),
                     new ReportParameter("monthYear",GetMonthName.MonthInBangla(month) + "/" +
                        string.Concat(year.ToString().Select(c => (char)('\u09E6' + c - '0')))),

                };

                    report.DataSources.Add(new ReportDataSource("SelfCpf", sources));

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

        //** END *** Self CPF Report Show who login the page****************

        //****Start *********** Use This Method for CPF Entry Using Api ****************
        [HttpGet]
        //[MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public async Task<IActionResult> CPFCalculate()
        {
            ViewBag.SuccessMessage = TempData["Success"];
            ViewBag.ErrorMessage = TempData["Error"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CPFCalculate(int year, int month)
        {

            string endpoint = "/api/Utility/CalculateCpf?year=" + year + "&&month=" + month;

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
                    TempData["Success"] = "CPF calculation successful";
                    //var data = await response.Content.ReadAsStringAsync();
                    //var table = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Data.DataTable>(data);

                }
                else
                {
                    TempData["Error"] = "CPF calculation failed";
                }
            }
            return RedirectToAction("CPFCalculate");
        }

        //****END *********** Use This Method for CPF Entry Using Api ****************




        // ************ CPF List*******************
        [HttpGet]
        public IActionResult List()
        {
            var users = _userManager.Users.Where(c => c.IsActive).ToList().Select(s => new
            {
                Text = s.EmployeeCode + "-" + s.FullName,
                Value = s.Id

            }).ToList();
            ViewBag.Users = new SelectList(users, "Value", "Text");
            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];
            var list = _cpfInfoManager.GetList();
            return View(list);
        }
        [HttpPost]
        public IActionResult List(string AppUserId, int fmonth, int fyear, int tmonth, int tyear)
        {
            var GetSearchData = _cpfInfoManager.GetListByMonthUser(fyear, fmonth, tyear, tmonth, AppUserId);
            if (GetSearchData!=null)
            {
                var users = _userManager.Users.ToList().Select(s => new
                {
                    Text = s.EmployeeCode + "-" + s.FullName,
                    Value = s.Id

                }).ToList();
                ViewBag.Users = new SelectList(users, "Value", "Text");
                return View(GetSearchData);
            }
            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];
            var list = _cpfInfoManager.GetList();
            return View(list);
        }




        // Start ***** Back/Previous  CPF Information Entry ***********************

        //[MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult PreviousCPFInfo()
        {
            var users = _userManager.Users.Include(c => c.Designation).Where(c => c.IsActive).ToList().Select(s => new
            {
                Text = s.EmployeeCode + "-" + s.FullName + "_" + s.Designation.Name,
                Value = s.Id

            }).ToList();
            ViewBag.Users = new SelectList(users, "Value", "Text");
            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PreviousCPFInfo(CpfInfo cpfInfo, int Year, int Month, string AppUserId)
        {
            try
            {
                var existData = _cpfInfoManager.GetListByMonthUser(Year, Month, AppUserId);
                if (existData != null)
                {
                    existData.GovtContribution = cpfInfo?.GovtContribution ?? 0;
                    existData.SelfContribution = cpfInfo?.SelfContribution ?? 0;
                    existData.ArrearsBasic = cpfInfo?.ArrearsBasic ?? 0;
                    existData.BasicSalary = cpfInfo.BasicSalary;
                    //existData.GrandTotal=cpfInfo.GrandTotal;
                    existData.Year = cpfInfo.Year;
                    existData.Month = cpfInfo.Month;
                    existData.TotalContribution = cpfInfo?.TotalContribution ?? 0;
                    _cpfInfoManager.Update(existData);
                }
                else
                {
                    _cpfInfoManager.Add(cpfInfo);
                }
                TempData["Success"] = "Added Successfully";
                return RedirectToAction("PreviousCPFInfo");
            }
            catch (Exception)
            {

                TempData["Error"] = "Failed";
                return RedirectToAction("PreviousCPFInfo");
            }
         
        }
        // END ***** Back CPF Information Entry ***********************

        
    }

}
