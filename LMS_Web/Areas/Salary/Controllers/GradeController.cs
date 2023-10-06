using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS_Web.Areas.Salary.Manager;
using LMS_Web.Data;
using Microsoft.AspNetCore.Hosting;
using LMS_Web.SecurityExtension;

namespace LMS_Web.Areas.Salary.Controllers
{
    [Area("Salary")]
    public class GradeController : Controller
    {
        private GradeManager gradeManager;
        private readonly GradeStepBasicManager gradeStepBasicManager;

        public GradeController(ApplicationDbContext dbContext)
        {
            gradeManager = new GradeManager(dbContext);
            gradeStepBasicManager = new GradeStepBasicManager(dbContext);
        }
        //[MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult GradeWishLoadCurrentBasic(int gradeId)
        {
          var gradeStep = gradeStepBasicManager.GetList(gradeId);
            return Json(gradeStep);
        }
    }
}
