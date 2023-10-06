using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Manager;
using LMS_Web.Areas.Settings.Interface;
using LMS_Web.Areas.Settings.Manager;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Data;
using LMS_Web.Models;
using LMS_Web.SecurityExtension;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Web.Areas.Settings.Controllers
{
    [Area("Settings")]
    public class FiscalYearController : Controller
    {
       
        private FiscalYearManager fiscalYearManager;
       


        public FiscalYearController(ApplicationDbContext db)
        {
          fiscalYearManager=new FiscalYearManager(db);

        }
        [HttpGet]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public async Task<IActionResult> Add(int? id)
        {
            FiscalYear fiscalYear = new FiscalYear();
            if (id != null)
            {
                fiscalYear = fiscalYearManager.GetById((int)id);
            }
            return View(fiscalYear);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(FiscalYear fiscalYear, string btnValue)
        {
            var getValue = fiscalYearManager.GetByValue(fiscalYear.Value);
            if (getValue != null)
            {
                TempData["Error"] = "Already saved in the same financial year";
                return RedirectToAction("List");
            }
            if (btnValue == "Save")
            {                
                var result = fiscalYearManager.Add(fiscalYear);
                if (result)
                {
                    TempData["Success"] = "Successfully Added";
                }
                else
                {
                    TempData["Error"] = "Failed to save";
                }
            }
            else
            {
                var FiscalYear = fiscalYearManager.GetById(fiscalYear.Id);

                if (FiscalYear != null)
                {
                    FiscalYear.Id = fiscalYear.Id;
                    FiscalYear.Value = fiscalYear.Value;                  
                    var result = fiscalYearManager.Update(fiscalYear);
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
            return RedirectToAction("List");
        }
        public IActionResult List()
        {
            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];
            var list = fiscalYearManager.GetList();
            return View(list);
        }

    }
}
