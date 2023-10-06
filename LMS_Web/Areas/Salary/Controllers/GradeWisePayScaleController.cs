using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS_Web.Areas.Salary.Manager;
using LMS_Web.Data;
using Microsoft.AspNetCore.Hosting;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.SecurityExtension;

namespace LMS_Web.Areas.Salary.Controllers
{
    [Area("Salary")]
    public class GradeWisePayScaleController : Controller
    {
        private GradeWisePayScaleManager gradWisePayScaleManager;
        private GradeManager gradeManager;
        private PayScaleManager payScaleManager;

        public GradeWisePayScaleController(ApplicationDbContext dbContext, IWebHostEnvironment _environment)
        {
            gradWisePayScaleManager = new GradeWisePayScaleManager(dbContext);
            gradeManager = new GradeManager(dbContext);
            payScaleManager = new PayScaleManager(dbContext);

        }

        [HttpGet]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult Add(int? id)
        {
            GradeWisePayScale gradeWisePayScale = new GradeWisePayScale();
            if (id != null)
            {
                gradeWisePayScale = gradWisePayScaleManager.GetById((int)id);
            }
            ViewBag.Grade = gradeManager.GetAll();
            ViewBag.PayScale = payScaleManager.GetAll();
            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];
            return View(gradeWisePayScale);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(GradeWisePayScale g, string btnValue)
        {

            if (btnValue == "Save")
            {
                if (g.FixedAmount <= 0)
                {
                    TempData["Error"] = "Negative amounts are not allowed";
                    return RedirectToAction("Add");
                }
                else
                {
                    var payScales = gradWisePayScaleManager.GetAll();
                    foreach (var item in payScales)
                    {
                        if (item.GradeId == g.GradeId && item.PayScaleId == g.PayScaleId)
                        {
                            TempData["Error"] = "Already save data";
                            return RedirectToAction("Add");
                        }
                    }
                    var res = gradWisePayScaleManager.Add(g);
                    if (res)
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
                var oldGrade = gradWisePayScaleManager.GetById(g.Id);
                if (oldGrade != null)
                {
                    if (g.FixedAmount <= 0)
                    {
                        TempData["Error"] = "Negative amounts are not allowed";
                        return RedirectToAction("Add");
                    }
                    if (oldGrade.GradeId == g.GradeId && oldGrade.PayScaleId == g.PayScaleId)
                    {

                    }
                    else
                    {
                        var payScales = gradWisePayScaleManager.GetAll();
                        foreach (var item in payScales)
                        {
                            if (item.GradeId == g.GradeId && item.PayScaleId == g.PayScaleId)
                            {
                                TempData["Error"] = "Already save data";
                                return RedirectToAction("Add");
                            }
                        }
                    }
                    oldGrade.GradeId = g.GradeId;
                    oldGrade.PayScaleId = g.PayScaleId;
                    oldGrade.FixedAmount = g.FixedAmount;
                    oldGrade.Percentage = g.Percentage;
                    oldGrade.IsFixed = g.IsFixed;

                    var res = gradWisePayScaleManager.Update(oldGrade);
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
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult List()
        {
            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];
            var list = gradWisePayScaleManager.GetList();
            return View(list);
        }

    }
}
