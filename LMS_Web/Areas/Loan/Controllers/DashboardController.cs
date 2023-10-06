using LMS_Web.Areas.CPF.Manager;
using LMS_Web.Areas.Loan.Manager;
using LMS_Web.Areas.Salary.Manager;
using LMS_Web.Controllers;
using LMS_Web.Data;
using LMS_Web.Models;
using LMS_Web.SecurityExtension;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;
using LMS_Web.Areas.Loan.Interface;

namespace LMS_Web.Areas.Loan.Controllers
{
    [Area("Loan")]
    public class DashboardController : Controller
    {
        //[MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]

        private CpfPercentManager cpfPercentManager;
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly UserWiseLoanManager _userWiseLoanManager;
        private IWebHostEnvironment webHostEnvironment;
        private IConfiguration configuration;
        private readonly UserSpecificAllowanceManager _userSpecificAllowanceManager;
        private readonly CpfInfoManager _cpfInfoManager;
        private readonly LoanInstallmentInfoManager _loanInstallmentInfoManager;
        private PRlApplicantInfoManager _plApplicantInfoManager;
        //public CpfHomeController(ApplicationDbContext db)
        public DashboardController(ILogger<HomeController> logger, IConfiguration _configuration, IWebHostEnvironment _environment, ApplicationDbContext db, UserManager<AppUser> userManager)
        {
            cpfPercentManager = new CpfPercentManager(db);
            _userSpecificAllowanceManager = new UserSpecificAllowanceManager(db);
            _cpfInfoManager = new CpfInfoManager(db);
            _loanInstallmentInfoManager = new LoanInstallmentInfoManager(db);
            _plApplicantInfoManager = new PRlApplicantInfoManager(db);
            _logger = logger;
            _context = db;
            _userManager = userManager;
            _userWiseLoanManager = new UserWiseLoanManager(db);
            webHostEnvironment = _environment;
            configuration = _configuration;

        }
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult Index()
        {
            var currentDate = DateTime.Now;
            var firstDay = new DateTime(currentDate.Year, currentDate.Month, 1);
           // ViewBag.CurrentMonthCPFLoan=
            return View();
        }

        public IActionResult TotalPaidLoanList()
        {
           var totalPaidLoan = _userWiseLoanManager.GetPaidLoans();
            return View(totalPaidLoan);
        }
        public IActionResult TotalCPFLoanList()
        {
            var totalCpfLoan = _userWiseLoanManager.GetCpfLoans();
            return View(totalCpfLoan);
        }
    }
}
