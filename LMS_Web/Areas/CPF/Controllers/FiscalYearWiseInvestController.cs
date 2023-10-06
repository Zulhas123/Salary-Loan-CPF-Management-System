using LMS_Web.Areas.CPF.Interface;
using LMS_Web.Areas.CPF.Manager;
using LMS_Web.Areas.CPF.Models;
using LMS_Web.Areas.Settings.Manager;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Data;
using LMS_Web.Models;
using LMS_Web.SecurityExtension;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace LMS_Web.Areas.CPF.Controllers
{
    [Area("CPF")]
    public class FiscalYearWiseInvestController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly FiscalYearManager _fiscalYearManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly FisYearInvestManager _fisYearInvestManager;

        public FiscalYearWiseInvestController(ApplicationDbContext dbContext, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _fisYearInvestManager = new FisYearInvestManager(dbContext);
            _fiscalYearManager = new FiscalYearManager(dbContext);
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult AddFiscalYearInvest()
        {
            var users = _userManager.Users.Include(c => c.Designation).Where(c => c.IsActive).ToList().Select(s => new
            {
                Text = s.EmployeeCode + "-" + s.FullName + "_" + s.Designation.Name,
                Value = s.Id

            }).ToList();
            ViewBag.Users = new SelectList(users, "Value", "Text");
            //ViewBag.users = _userManager.Users.Where(c => c.IsActive).ToList();
            ViewBag.FiscalYear = _fiscalYearManager.GetList();
            ViewBag.SuccessMessage = TempData["Success"];
            ViewBag.ErrorMessage = TempData["Error"];
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddFiscalYearInvest(FiscalYearWiseInvestmentInfo fiscInfo,int fiscalYear, string AppUserId)
        {
            var existData = _fisYearInvestManager.GetByUserAndFiscalYear(fiscalYear, AppUserId);
            FiscalYearWiseInvestmentInfo fisyear = new FiscalYearWiseInvestmentInfo();
            bool save;
            if (existData != null)
            {
                
               
                existData.InvestmentAmount= fiscInfo.InvestmentAmount;
                existData.InterestAmount= fiscInfo.InterestAmount;
                existData.Total = (existData.InvestmentAmount + existData.InterestAmount);
                save = _fisYearInvestManager.Update(existData);

            }
            else
            {
                
                fisyear.InvestmentAmount = fiscInfo.InvestmentAmount;
                fisyear.InterestAmount= fiscInfo.InterestAmount;
                fisyear.Total = (fisyear.InvestmentAmount + fisyear.InterestAmount);
                fisyear.AppUserId= AppUserId;
                fisyear.FiscalYearId = fiscalYear;
                save = _fisYearInvestManager.Add(fisyear);
            }
            if (save)
            {

                TempData["Success"] = "Successfully Saved";
            }
            else
            {
                TempData["Error"] = "Failed to save. Please try again";
            }

            return RedirectToAction("List");
        }


        public IActionResult List()
        {
            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];
            ViewBag.users = _userManager.Users.ToList();
            var list = _fisYearInvestManager.GetList();
            return View(list);
        }
    }
}
