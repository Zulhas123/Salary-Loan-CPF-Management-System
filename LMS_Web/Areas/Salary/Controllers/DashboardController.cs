using AspNetCore.ReportingServices.ReportProcessing.ReportObjectModel;
using Google.Protobuf.WellKnownTypes;
using LMS_Web.Areas.CPF.Controllers;
using LMS_Web.Areas.CPF.Interface;
using LMS_Web.Areas.CPF.Manager;
using LMS_Web.Areas.CPF.Models;
using LMS_Web.Areas.CPF.ViewModels;
using LMS_Web.Areas.Loan.Controllers;
using LMS_Web.Areas.Loan.Interface;
using LMS_Web.Areas.Loan.Manager;
using LMS_Web.Areas.Loan.Models;
using LMS_Web.Areas.Salary.Dataset;
using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Manager;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Areas.Settings.Manager;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Areas.Settings.ViewModels;
using LMS_Web.Controllers;
using LMS_Web.Data;
using LMS_Web.Migrations;
using LMS_Web.Models;
using LMS_Web.SecurityExtension;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LMS_Web.Areas.Salary.Controllers
{
    [Area("Salary")]
    public class DashboardController : Controller
    {
        private CpfPercentManager cpfPercentManager;
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private IWebHostEnvironment webHostEnvironment;
        private IConfiguration configuration;
        private readonly UserSpecificAllowanceManager _userSpecificAllowanceManager;
        private readonly CpfInfoManager _cpfInfoManager;
        private readonly LoanInstallmentInfoManager _loanInstallmentInfoManager;
        private PRlApplicantInfoManager _plApplicantInfoManager;
        private readonly UserHouseRentManager _userHouseRentManager;
        private readonly GradeWisePayScaleManager _gradeWisePayScaleManager;
        private readonly IProcessSalaryManager processSalaryManager;
        private UtilityController utility;
        private LoanInstallmentInfoManager _userInstallmentInfoManager;
        private UserWiseLoanManager _userWiseLoanManager;
        InvestmentInfoController investmentInfoController;
        private readonly FisYearInvestManager _fisYearInvestManager;
        private readonly FiscalYearManager _fiscalYearManager;
        private readonly GradeStepBasicManager _gradeStepBasicManager;
        private readonly SalaryStepInfoManager _salaryStepInfoManager;

        public DashboardController(ILogger<HomeController> logger, IConfiguration _configuration, IWebHostEnvironment _environment, ApplicationDbContext db, UserManager<AppUser> userManager)
        {
            cpfPercentManager = new CpfPercentManager(db);
            _userSpecificAllowanceManager = new UserSpecificAllowanceManager(db);
            _cpfInfoManager = new CpfInfoManager(db);
            _loanInstallmentInfoManager = new LoanInstallmentInfoManager(db);
            _plApplicantInfoManager = new PRlApplicantInfoManager(db);
            _userHouseRentManager = new UserHouseRentManager(db);
            _gradeWisePayScaleManager = new GradeWisePayScaleManager(db);
            _logger = logger;
            _context = db;
            _userManager = userManager;
            webHostEnvironment = _environment;
            configuration = _configuration;
            processSalaryManager = new ProcessSalaryManager(db);
            utility = new UtilityController(db, userManager, _environment);
            _fisYearInvestManager = new FisYearInvestManager(db);
            investmentInfoController = new InvestmentInfoController(db, userManager, webHostEnvironment);
            _fiscalYearManager = new FiscalYearManager(db);
            _gradeStepBasicManager = new GradeStepBasicManager(db);
            _salaryStepInfoManager = new SalaryStepInfoManager(db);
        }
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult Index()
        {
          //  SalaryStepUpdate();
            //    ViewBag.TotalGradeWisePayScale = _gradeWisePayScaleManager.GetAll().Count;
            //    ViewBag.TotalUserSpecificAllowance = _userSpecificAllowanceManager.GetAll().Count;
            return View();

        }
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult ProcessSalary()
        {
            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];
            return View();
        }
        [HttpPost]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult ProcessSalary(Models.ProcessSalary processSalary)
        {
            try
            {
                var getData = processSalaryManager.GetByMonthYear(processSalary.Month, processSalary.Year);
                if (getData == null)
                {
                    if (processSalary.IsFinal)
                    {
                        DoCalC(processSalary);
                    }
                    processSalaryManager.Add(processSalary);
                }
                else
                {
                    if (getData.IsFinal == true)
                    {

                        TempData["Error"] = "Already completed";
                        return RedirectToAction("ProcessSalary");
                    }
                    else
                    {
                        if (processSalary.IsFinal == true)
                        {
                            DoCalC(processSalary);
                        }
                        processSalaryManager.Add(processSalary);
                    }

                }
                TempData["Success"] = "Salary process completed";
            }
            catch (Exception e)
            {

                TempData["Error"] = "Failed to complete";
            }

            return RedirectToAction("ProcessSalary");
        }
        private void DoCalC(Models.ProcessSalary processSalary)
        {
            utility.SaveSalaryHistory(processSalary.Year, processSalary.Month);
            utility.CalculateCpf(processSalary.Year, processSalary.Month);
            utility.Investment(processSalary.Year, processSalary.Month);
            utility.FundCalculate(processSalary.Year, processSalary.Month);
            utility.PaidLoanAfterlastInstallment(processSalary.Year, processSalary.Month);
            if (processSalary.Month == 6)
            {
                var getFiscalYear = processSalary.Year - 1 + "-" + processSalary.Year;
                var fiscalYear = _fiscalYearManager.GetByValue(getFiscalYear);
                if (fiscalYear != null)
                {
                    FiscalYearWiseInvestment(fiscalYear.Id);
                   
                }
            }
        }
        public void UpdateInvestmentOnly(int year, int month)
        {
            utility.Investment(year, month);
            //utility.SaveSalaryHistory(year,month);
        }
        public void CalculateLoanData(int year, int month)
        {
            utility.PaidLoanAfterlastInstallment(year, month);
        }
        private void FiscalYearWiseInvestment(int fiscalYearId)
        {

            var users = _userManager.Users.Where(c => c.IsActive).ToList();
            List<FiscalYearWiseInvestmentInfo> insertList = new List<FiscalYearWiseInvestmentInfo>();
            List<FiscalYearWiseInvestmentInfo> updateList = new List<FiscalYearWiseInvestmentInfo>();
            foreach (var item in users)
            {
                FiscalYearWiseInvestmentInfo data = new FiscalYearWiseInvestmentInfo();
                var obj = investmentInfoController.GeneralInvestmentCalculate(item.Id, fiscalYearId, null, null);

                var result = obj.description[obj.description.Count - 1];
                var amount = result.InvestAmount;
                var interest = result.InterstAmount;
                var sum = result.Total;
                var existingData = _fisYearInvestManager.GetByUserAndFiscalYear(fiscalYearId, item.Id);
                if (existingData == null)
                {
                    data.AppUserId = item.Id;
                    data.FiscalYearId = fiscalYearId;
                    data.Total = Convert.ToDecimal((string.Concat(sum.Select(c => (char)('0' + c - '\u09E6'))).Replace("", ".")));
                    data.InterestAmount = Convert.ToDecimal((string.Concat(interest.Select(c => (char)('0' + c - '\u09E6'))).Replace("", ".")));
                    data.InvestmentAmount = Convert.ToDecimal((string.Concat(amount.Select(c => (char)('0' + c - '\u09E6'))).Replace("", ".")));
                    insertList.Add(data);
                }
                else
                {
                    existingData.Total = Convert.ToDecimal((string.Concat(sum.Select(c => (char)('0' + c - '\u09E6'))).Replace("", ".")));
                    existingData.InterestAmount = Convert.ToDecimal((string.Concat(interest.Select(c => (char)('0' + c - '\u09E6'))).Replace("", ".")));
                    existingData.InvestmentAmount = Convert.ToDecimal((string.Concat(amount.Select(c => (char)('0' + c - '\u09E6'))).Replace("", ".")));
                    updateList.Add(existingData);

                }
            }
            _fisYearInvestManager.Add(insertList);
            _fisYearInvestManager.Update(updateList);
        }

        //private void SalaryStepUpdate()
        //{
        //    List<AppUser> userList = new List<AppUser>();
        //    var allStep = _gradeStepBasicManager.GetListAllStep();
        //    var users = _userManager.Users.Where(c => c.IsActive).ToList();
        //    foreach (var user in users)
        //    {

        //        var basic = allStep.FirstOrDefault(c => c.GradeId == user.CurrentGradeId && c.Amount == user.CurrentBasic);
        //        var step = basic.StepNo + 1;
        //        var currentAmmount = allStep.FirstOrDefault(c => c.StepNo == step && c.GradeId == user.CurrentGradeId);
        //        if (currentAmmount != null)
        //        {
        //            user.CurrentBasic = currentAmmount.Amount;
        //            userList.Add(user);
        //        }


        //    }
           
        //    _context.UpdateRange(userList);


        //}

        private void SalaryStepUpdate()
        {
            var allStep = _gradeStepBasicManager.GetListAllStep();
            var users = _userManager.Users.Where(c => c.IsActive).ToList();

            foreach (var user in users)
            {
                var basic = allStep.FirstOrDefault(c => c.GradeId == user.CurrentGradeId && c.Amount == user.CurrentBasic);
                if (basic != null)
                {
                    var step = basic.StepNo + 1;
                    var currentAmount = allStep.FirstOrDefault(c => c.StepNo == step && c.GradeId == user.CurrentGradeId);
                    if (currentAmount != null)
                    {
                        user.CurrentBasic = currentAmount.Amount;
                    }
                }
            }

            _context.SaveChanges();
        }





        [HttpGet]
        public IActionResult SalaryStep()
        {
            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];
            return View();
        }
        [HttpPost]
        public IActionResult SalaryStep(SalaryIncrement increment)
        {
            try
            {
                SalaryStepUpdate();
          

                //var getData = _salaryStepInfoManager.GetByMonthYear(step.Month, step.Year);
                //if (step.Month == 7)
                //{
                //    if (getData != null)
                //    {
                //        TempData["Error"] = "Already completed";
                //    }
                //    else
                //    {
                //        SalaryStepUpdate();
                //        _salaryStepInfoManager.Add(step);
                //        TempData["Success"] = "Salary Step completed";
                //    }
                //}

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);

            }
            return RedirectToAction("SalaryStep");
        }              

    }

}


//public IActionResult FiscalYearWiseInvestments(int fiscalYearId)
//{

//    var users = _userManager.Users.Where(c => c.IsActive).ToList();
//    List<FiscalYearWiseInvestmentInfo> insertList = new List<FiscalYearWiseInvestmentInfo>();
//    List<FiscalYearWiseInvestmentInfo> updateList = new List<FiscalYearWiseInvestmentInfo>();
//    foreach (var item in users)
//    {
//        FiscalYearWiseInvestmentInfo data = new FiscalYearWiseInvestmentInfo();
//        var obj = investmentInfoController.GeneralInvestmentCalculate(item.Id, fiscalYearId, null, null);

//        var result = obj.description[obj.description.Count - 1];
//        var amount = result.InvestAmount;
//        var interest = result.InterstAmount;
//        var sum = result.Total;
//        var existingData = _fisYearInvestManager.GetByUserAndFiscalYear(fiscalYearId, item.Id);
//        if (existingData == null)
//        {
//            data.AppUserId = item.Id;
//            data.FiscalYearId = fiscalYearId;
//            data.Total = Convert.ToDecimal((string.Concat(sum.Select(c => (char)('0' + c - '\u09E6'))).Replace("", ".")));
//            data.InterestAmount = Convert.ToDecimal((string.Concat(interest.Select(c => (char)('0' + c - '\u09E6'))).Replace("", ".")));
//            data.InvestmentAmount = Convert.ToDecimal((string.Concat(amount.Select(c => (char)('0' + c - '\u09E6'))).Replace("", ".")));
//            insertList.Add(data);
//        }
//        else
//        {
//            existingData.Total = Convert.ToDecimal((string.Concat(sum.Select(c => (char)('0' + c - '\u09E6'))).Replace("", ".")));
//            existingData.InterestAmount = Convert.ToDecimal((string.Concat(interest.Select(c => (char)('0' + c - '\u09E6'))).Replace("", ".")));
//            existingData.InvestmentAmount = Convert.ToDecimal((string.Concat(amount.Select(c => (char)('0' + c - '\u09E6'))).Replace("", ".")));
//            updateList.Add(existingData);

//        }
//    }
//    _fisYearInvestManager.Add(insertList);
//    _fisYearInvestManager.Update(updateList);
//    return Ok();
//        //}
//    }
//}
