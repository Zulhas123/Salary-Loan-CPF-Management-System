using LMS_Web.Areas.CPF.Manager;
using LMS_Web.Areas.Loan.Manager;
using LMS_Web.Areas.Salary.Manager;
using LMS_Web.Controllers;
using LMS_Web.Data;
using LMS_Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;
using LMS_Web.Areas.Settings.Manager;

namespace LMS_Web.Areas.Settings.Controllers
{
    [Area("Settings")]
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
        private readonly ChildrenInfoManager _childrenInfoManager;

        //public CpfHomeController(ApplicationDbContext db)
        public DashboardController(ILogger<HomeController> logger, IConfiguration _configuration, IWebHostEnvironment _environment, ApplicationDbContext db, UserManager<AppUser> userManager)
        {
            cpfPercentManager = new CpfPercentManager(db);
            _userSpecificAllowanceManager = new UserSpecificAllowanceManager(db);
            _cpfInfoManager = new CpfInfoManager(db);
            _loanInstallmentInfoManager = new LoanInstallmentInfoManager(db);
            _plApplicantInfoManager = new PRlApplicantInfoManager(db);
            _childrenInfoManager = new ChildrenInfoManager(db);
            _logger = logger;
            _context = db;
            _userManager = userManager;
            webHostEnvironment = _environment;
            configuration = _configuration;

        }
        public IActionResult Index()
        {
           
            ViewBag.TotalUser = _context.Users.Count();
            ViewBag.TotalDepartment = _context.Department.Count();
            ViewBag.TotalWing = _context.Wing.Count();
            ViewBag.TotalSection = _context.Section.Count();        

            return View();
        }
    }
}
