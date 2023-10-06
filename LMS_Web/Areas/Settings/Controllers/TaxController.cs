using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LMS_Web.Areas.Loan.DataSet;
using LMS_Web.Areas.Loan.Interface;
using LMS_Web.Areas.Loan.Manager;
using LMS_Web.Areas.Loan.Models;
using LMS_Web.Areas.Salary.ViewModels;
using LMS_Web.Areas.Settings.Interface;
using LMS_Web.Areas.Settings.Manager;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Data;
using LMS_Web.Models;
using LMS_Web.SecurityExtension;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LMS_Web.Areas.Settings.Controllers
{
    [Area("Salary")]
    public class TaxController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly UserTaxManager _userTaxManager;
        private readonly TaxInstallmentInfoManager _taxInstallmentInfoManager;
        private readonly FiscalYearManager _fiscalYearManager;
        public TaxController(ApplicationDbContext db, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _userTaxManager = new UserTaxManager(db);
            _taxInstallmentInfoManager = new TaxInstallmentInfoManager(db);
            _fiscalYearManager = new FiscalYearManager(db);
        }
        [HttpGet]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult Add()
        {
            //ViewBag.Users = _userManager.Users.ToList();
            var users = _userManager.Users.Include(c=>c.Designation).Where(c=>c.IsActive).ToList().Select(s => new
            {
                Text = s.EmployeeCode + "-" + s.FullName + "-" +s.Designation.Name,
                Value = s.Id

            }).ToList();
            ViewBag.Users = new SelectList(users, "Value", "Text");
            ViewBag.FiscalYear = new SelectList(_fiscalYearManager.GetAll().ToList(), "Id", "Value");

            ViewBag.SuccessMessage = TempData["Success"];
            ViewBag.ErrorMessage = TempData["Error"];
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(UserTax userTax, int month, int year)
        {

            var tax = _userTaxManager.GetByUserId(userTax.AppUserId, userTax.FiscalYearId);
            bool save;
            if (tax != null)
            {
                /*  Never delete this comment code. It will be applied after back data entry  */

                //tax.UpdatedById = _userManager.GetUserId(User);
                //tax.UpdatedDateTime = DateTime.Now;
                //tax.TotalAmount = userTax.TotalAmount;
                ////tax.MonthlyDeduction = userTax.TotalAmount / userTax.TotalInstallment;
                //var installmentAmount = userTax.TotalAmount / tax.TotalInstallment;
                //tax.MonthlyDeduction = installmentAmount;
                //tax.TotalInstallment = userTax.TotalInstallment;

                //save = _userTaxManager.Update(tax);

                //if (save)
                //{

                //    var getExisting = _taxInstallmentInfoManager.GetListById(tax.Id);

                //    if (getExisting.Any())
                //    {
                //        _taxInstallmentInfoManager.Delete(getExisting);

                //    }
                //    SaveInstallment(tax, month, year, installmentAmount);
                //}

                TempData["Error"] = "Tax information already exist for select user and fiscal year";


            }
            else
            {
                userTax.CreatedById = _userManager.GetUserId(User);
                userTax.CreatedDateTime = DateTime.Now;
                var installmentAmount = userTax.TotalAmount / userTax.TotalInstallment;
                userTax.MonthlyDeduction = installmentAmount;
                userTax.DeductedAmount = 0;
                save = _userTaxManager.Add(userTax);

                // Start -------  New Code for Tax InstallmentInfo ----------------
                var fiscalYear = _fiscalYearManager.GetById(userTax.FiscalYearId).Value;
                var splitResult = fiscalYear.Split("-");
                int fyear = Convert.ToInt32(splitResult[0]);
                int tyear = Convert.ToInt32(splitResult[1]);
                if (month >= 7)
                {
                    year = fyear;
                }
                else
                {
                    year = tyear;
                }

                if (save)
                {
                    SaveInstallment(userTax, month, year, installmentAmount);
                    TempData["Success"] = "Successfully Tax Saved";
                }
                else
                {
                    TempData["Error"] = "Failed to save. Please try again";
                }
                // End---------
            }

            return RedirectToAction("Add");
        }
        //[MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]

        private void SaveInstallment(UserTax userTax, int month, int year, decimal installmentAmount)
        {
            List<TaxInstallmentInfo> allList = new List<TaxInstallmentInfo>();

            for (int i = 0; i < userTax.TotalInstallment; i++)
            {
                TaxInstallmentInfo taxInstallmentInfo = new TaxInstallmentInfo();
                taxInstallmentInfo.AppUserId = userTax.AppUserId;
                taxInstallmentInfo.InstallmentNo = i + 1;
                taxInstallmentInfo.Month = month;
                taxInstallmentInfo.Year = year;
                taxInstallmentInfo.UserTaxId = userTax.Id;
                taxInstallmentInfo.MonthlyDeduction = installmentAmount;
                allList.Add(taxInstallmentInfo);

                month += 1;
                if (month > 12)
                {
                    month = 1;
                    year++;

                }

            }
            _taxInstallmentInfoManager.Add(allList);
        }
        //[MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult LoadUserTax(string userId, int fiscalYear)
        {
            var tax = _userTaxManager.GetByUserId(userId, fiscalYear);
            bool isFound = false;
            if (tax != null)
            {
                isFound = true;
                var taxInstallmentInfo = _taxInstallmentInfoManager.GetUserTaxInstallmentInfo(tax.Id);
                ViewBag.UserTax = tax;
                ViewBag.TaxInstallmentInfo = taxInstallmentInfo;
            }
            ViewBag.IsFound = isFound;
            return PartialView("_LoadUserTax");
        }

        public IActionResult UpdateUserTaxInstallment(int id, decimal monthlyDeduction)
        {
            var userTaxInstallment = _taxInstallmentInfoManager.GetById(id);
            var Tax = _userTaxManager.GetByTaxUserId(userTaxInstallment.UserTaxId);

            if (userTaxInstallment.MonthlyDeduction > monthlyDeduction)
            {
                Tax.TotalAmount -= (userTaxInstallment.MonthlyDeduction - monthlyDeduction);
            }
            else
            {
                Tax.TotalAmount += (monthlyDeduction - userTaxInstallment.MonthlyDeduction);
            }

            userTaxInstallment.MonthlyDeduction = monthlyDeduction;
            _taxInstallmentInfoManager.Update(userTaxInstallment);


            var res = _userTaxManager.Update(Tax);
            return Json(res);
        }
    }
}
