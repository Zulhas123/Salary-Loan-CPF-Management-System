using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Manager;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Data;
using LMS_Web.Models;
using LMS_Web.SecurityExtension;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Web.Areas.Salary.Controllers
{
    [Area("Salary")]
    public class DeductionController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private UserDeductionManager _userDeductionManager;
        private DeductionManager _deductionManager;
        private FixedDeductionManager _fixedDeductionManager;
        private readonly IProcessSalaryManager processSalaryManager;
        public DeductionController(ApplicationDbContext dbContext, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _userDeductionManager = new UserDeductionManager(dbContext);
            _deductionManager = new DeductionManager(dbContext);
            _fixedDeductionManager = new FixedDeductionManager(dbContext);
            processSalaryManager = new ProcessSalaryManager(dbContext);
        }
        [HttpGet]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult FixedDeduction(int? id)
        {

            ViewBag.SuccessMessage = TempData["Success"];
            ViewBag.ErrorMessage = TempData["Error"];
            ViewBag.FixedDeduction = _deductionManager.GetFixedList();
            ViewBag.List = _fixedDeductionManager.GetList();
            var getData = _fixedDeductionManager.GetById(id ?? 0);
            return View(getData);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FixedDeduction(FixedDeduction fixedDeduction)
        {

            if (fixedDeduction.Id != 0)
            {
                var data = _fixedDeductionManager.GetByDeductionId(fixedDeduction.DeductionId);
                if (data != null)
                {
                    TempData["Error"] = "Already saved. Please update";
                    return RedirectToAction("FixedDeduction");
                }
                //var data = _fixedDeductionManager.GetById(fixedDeduction.Id);
                if (fixedDeduction.Amount > 0)
                {

                    if (data != null)
                    {
                        data.Amount = fixedDeduction.Amount;
                        data.UpdatedById = _userManager.GetUserId(User);
                        data.UpdatedDateTime = DateTime.Now;
                        var update = _fixedDeductionManager.Update(fixedDeduction);
                        if (update)
                        {
                            TempData["Success"] = "Successfully updated";
                        }
                        else
                        {
                            TempData["Error"] = "Fail to update";
                        }
                    }
                }
                else
                {
                    TempData["Error"] = "Amount should not less than or equal 0";
                }

            }
            else
            {
                var data = _fixedDeductionManager.GetByDeductionId(fixedDeduction.DeductionId);
                if (data != null)
                {
                    TempData["Error"] = "Already saved. Please update";
                    return RedirectToAction("FixedDeduction");
                }
                fixedDeduction.CreatedById = _userManager.GetUserId(User);
                fixedDeduction.CreatedDateTime = DateTime.Now;

                if (fixedDeduction.Amount > 0)
                {

                    var save = _fixedDeductionManager.Add(fixedDeduction);

                    if (save)
                    {
                        TempData["Success"] = "Successfully saved";
                    }
                    else
                    {
                        TempData["Error"] = "Not saved. Please try again";
                    }
                }
                else
                {
                    TempData["Error"] = "Amount should not less than or equal 0 ";
                }

            }

            return RedirectToAction("FixedDeduction");
        }



        [HttpGet]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult AddUserDeduction(int? id)
        {
            UserDeduction userDeduction = new UserDeduction();
            if (id != null)
            {
                userDeduction = _userDeductionManager.GetById((int)id);
            }
            ViewBag.Deduction = _deductionManager.GetUserSpecific();
            var users = _userManager.Users.Include(c=>c.Designation).Where(c => c.IsActive).ToList().Select(s => new
            {
                Text = s.EmployeeCode + "-" + s.FullName +"-"+ s.Designation.Name,
                Value = s.Id

            }).ToList();
            ViewBag.Users = new SelectList(users, "Value", "Text");
            ViewBag.SuccessMessage = TempData["Success"];
            ViewBag.ErrorMessage = TempData["Error"];
            return View(userDeduction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddUserDeduction(UserDeduction u, String btnValue)
        {
            var getData = processSalaryManager.GetByMonthYear(u.Month, u.Year);
            if (getData?.IsFinal == true)
            {
                TempData["Error"] = "Process allready completed for this Month";
                return RedirectToAction("List");
            }
            else
            {
                if (u.IsSameEveryMonth)
                {
                    u.Month = 0;
                    u.Year = 0;
                }
                if (btnValue == "Save")
                {
                    var data = _userDeductionManager.GetDeductionByInfo(u.AppUserId, u.DeductionId, u.IsSameEveryMonth, u.Month, u.Year);

                    if (data != null)
                    {
                        TempData["Error"] = "Data already saved.";
                        return RedirectToAction("List");
                    }

                    if (u.Amount >= 0)
                    {
                        var result = _userDeductionManager.Add(u);
                        if (result)
                        {
                            TempData["Success"] = "Successfully Added";
                        }
                        else
                        {
                            TempData["Error"] = "Failed to saved";
                        }

                    }
                    else
                    {
                        TempData["Error"] = "Amount should not less than 0";
                    }

                }
                else
                {
                    var oldUserDeduction = _userDeductionManager.GetById(u.Id);
                    if (u.Amount >= 0)
                    {
                        if (oldUserDeduction != null)
                        {
                            oldUserDeduction.DeductionId = u.DeductionId;
                            oldUserDeduction.AppUserId = u.AppUserId;
                            oldUserDeduction.IsSameEveryMonth = u.IsSameEveryMonth;
                            oldUserDeduction.Amount = u.Amount;
                            oldUserDeduction.Month = u.Month;
                            oldUserDeduction.Year = u.Year;

                            var result = _userDeductionManager.Update(u);
                            if (result)
                            {
                                TempData["Success"] = "Successfully Update";
                            }
                            else
                            {
                                TempData["Error"] = "Failed to update";

                            }
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Amount should not less than or equal 0";
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
            var list = _userDeductionManager.GetList();
            return View(list);
        }


        public IActionResult LoadList(int fromYear, int fromMonth, int toYear, int toMonth)
        {
            var list = _userDeductionManager.GetListByDateRange(fromYear, fromMonth, toYear, toMonth);
            bool isFound = true;
            if (list.Count <= 0)
            {
                isFound = false;
            }
            ViewBag.IsFound = isFound;
            ViewBag.IsSame = false;
            return PartialView("_LoadDeductionList", list);
        }
        public IActionResult LoadSameInEveryMonthList()
        {
            var list = _userDeductionManager.GetSameDataInEveryMonth();
            bool isFound = true;
            if (list.Count <= 0)
            {
                isFound = false;
            }
            ViewBag.IsFound = isFound;
            ViewBag.IsSame = true;
            return PartialView("_LoadDeductionList", list);
        }
        
    }
}
