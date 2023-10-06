using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS_Web.Areas.Salary.Manager;
using LMS_Web.Data;
using Microsoft.AspNetCore.Hosting;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Areas.Settings.Manager;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.SecurityExtension;

namespace LMS_Web.Areas.Salary.Controllers
{
    [Area("Salary")]
    public class GradeWiseFixedDeductionController : Controller
    {
        private GradeWiseFixedDeductionManager gradeWiseFixedDeductionManager;
        private DeductionManager deductionManager;
        private GradeManager gradeManager;

        public GradeWiseFixedDeductionController(ApplicationDbContext dbContext, IWebHostEnvironment _environment)
        {
            gradeWiseFixedDeductionManager = new GradeWiseFixedDeductionManager(dbContext);
            deductionManager = new DeductionManager(dbContext);
            gradeManager = new GradeManager(dbContext);

        }

        [HttpGet]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult Add(int? id)
        {
            GradeWiseFixedDeduction gradeWiseFixedDeduction = new GradeWiseFixedDeduction();
            if (id != null)
            {
                gradeWiseFixedDeduction = gradeWiseFixedDeductionManager.GetById((int)id);
            }
            ViewBag.Grade = gradeManager.GetList();
            ViewBag.Deduction = deductionManager.GetGradeWiseFixedList();
            return View(gradeWiseFixedDeduction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(GradeWiseFixedDeduction g, string btnValue)
        {
            var checkUnique = gradeWiseFixedDeductionManager.CheckUnique(g.DeductionId, g.FromGradeId, g.ToGradeId);
            if (checkUnique != null)
            {
                if (checkUnique.Id != g.Id)
                {
                    TempData["Error"] = "Already exist";
                    return RedirectToAction("List");

                }
            }

            if (btnValue == "Save")
            {
                var res = gradeWiseFixedDeductionManager.Add(g);
                if (res)
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
                var oldGrade = gradeWiseFixedDeductionManager.GetById(g.Id);
                if (oldGrade != null)
                {

                    oldGrade.DeductionId = g.DeductionId;
                    oldGrade.FromGradeId = g.FromGradeId;
                    oldGrade.ToGradeId = g.ToGradeId;
                    oldGrade.Amount = g.Amount;


                    var res = gradeWiseFixedDeductionManager.Update(oldGrade);
                    if (res)
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
            var list = gradeWiseFixedDeductionManager.GetAllIncluded();
            return View(list);
        }

    }
}
