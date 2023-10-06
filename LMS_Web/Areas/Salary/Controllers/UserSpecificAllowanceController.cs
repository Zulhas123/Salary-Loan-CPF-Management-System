using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Manager;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Areas.Salary.ViewModels;
using LMS_Web.Areas.Settings.Manager;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Data;
using LMS_Web.Models;
using LMS_Web.SecurityExtension;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.Linq;

namespace LMS_Web.Areas.Salary.Controllers
{
    [Area("Salary")]
    public class UserSpecificAllowanceController : Controller
    {
       
        private UserManager<AppUser> userManager;
        private readonly UserSpecificAllowanceManager userSpecificAllowanceManager;
        private readonly PayScaleManager payScaleManager;
        private readonly IProcessSalaryManager processSalaryManager;
        public UserSpecificAllowanceController(ApplicationDbContext db, UserManager<AppUser> _userManager)
        {
            userManager = _userManager;
            userSpecificAllowanceManager = new UserSpecificAllowanceManager(db);
            payScaleManager = new PayScaleManager(db);
            processSalaryManager = new ProcessSalaryManager(db);
        }
        [HttpGet]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult Add(int? id)
        {
            UserSpecificAllowance userSpecificAllowance = new UserSpecificAllowance();
            if (id != null)
            {
                userSpecificAllowance = userSpecificAllowanceManager.GetById((int)id);
            }
            var users = userManager.Users.Include(c=>c.Designation).Where(c => c.IsActive).ToList().Select(s => new
            {
                Text = s.EmployeeCode + "-" + s.FullName +"-"+s.Designation.Name,
                Value = s.Id

            }).ToList();
            ViewBag.Users = new SelectList(users, "Value", "Text");
            ViewBag.payScale = payScaleManager.GetUserSpecific();
            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];
            return View(userSpecificAllowance);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(UserSpecificAllowance h, String btnValue)
        {
            var getData = processSalaryManager.GetByMonthYear(h.Month, h.Year);

            if (getData?.IsFinal == true)
            {
                TempData["Error"] = " Process already completed for this Month";
            }
            else 
            {
                if (btnValue == "Save")
                {
                    if (h.Amount <= 0)
                    {
                        TempData["Error"] = "Negative amounts are not allowed";
                        return RedirectToAction("Add");
                    }
                    else
                    {
                        var allowance = userSpecificAllowanceManager.GetAll();
                        foreach (var item in allowance)
                        {
                            if (item.AppUserId == h.AppUserId && item.PayScaleId == h.PayScaleId)
                            {
                                TempData["Error"] = "Already save data";
                                return RedirectToAction("Add");
                            }
                        }
                        var result = userSpecificAllowanceManager.Add(h);
                        if (result)
                        {
                            TempData["Success"] = "Successfully Added";
                        }
                        else
                        {
                            TempData["Error"] = "Failed to save";
                        }
                    }
                }
                else
                {
                    var SpecificAllowance = userSpecificAllowanceManager.GetById(h.Id);

                    if (SpecificAllowance != null)
                    {
                        if (h.Amount <= 0)
                        {
                            TempData["Error"] = "Negative amounts are not allowed";
                            return RedirectToAction("Add");
                        }
                        if (SpecificAllowance.AppUserId == h.AppUserId && SpecificAllowance.PayScaleId == h.PayScaleId)
                        {
                        }
                        else
                        {
                            var allowance = userSpecificAllowanceManager.GetAll();
                            foreach (var item in allowance)
                            {
                                if (item.AppUserId == h.AppUserId && item.PayScaleId == h.PayScaleId)
                                {
                                    TempData["Error"] = "Already save data";
                                    return RedirectToAction("Add");
                                }
                            }
                        }
                        SpecificAllowance.Id = h.Id;
                        SpecificAllowance.AppUserId = h.AppUserId;
                        SpecificAllowance.PayScaleId = h.PayScaleId;
                        SpecificAllowance.Amount = h.Amount;
                        SpecificAllowance.Month = h.Month;
                        SpecificAllowance.Year = h.Year;

                        var result = userSpecificAllowanceManager.Update(h);
                        if (result)
                        {
                            TempData["Success"] = "Successfully Update";
                        }
                        else
                        {
                            TempData["Error"] = "Failed Update";
                        }

                    }
                }
               
            }
            return RedirectToAction("List");

        }
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult List()
        {
            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];          
            return View();
        }
        public IActionResult LoadList(int fromYear, int fromMonth, int toYear, int toMonth)
        {
            var list = userSpecificAllowanceManager.GetListByDateRange(fromYear, fromMonth, toYear, toMonth);
            bool isFound = true;
            if (list.Count <= 0)
            {
                isFound = false;
            }
            ViewBag.IsFound = isFound;
            return PartialView("_LoadUserSpecificAllowance", list);
        }
    }
}
