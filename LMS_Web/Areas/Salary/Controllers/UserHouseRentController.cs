using LMS_Web.Areas.Salary.Manager;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Areas.Settings.Manager;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Data;
using LMS_Web.Models;
using LMS_Web.SecurityExtension;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using System;
using System.Linq;

namespace LMS_Web.Areas.Salary.Controllers
{
    [Area("Salary")]
    public class UserHouseRentController : Controller
    {
        private UserHouseRentManager userHouseRentManager;
        private UserManager<AppUser> userManager;
        private ResidentStatusManager residentStatusManager;
        public UserHouseRentController(ApplicationDbContext db, UserManager<AppUser> _userManager)
        {
            userManager = _userManager;
            userHouseRentManager = new UserHouseRentManager(db);
            residentStatusManager = new ResidentStatusManager(db);

        }
        [HttpGet]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult Add(int? id)
        {
            UserHouseRent userHouseRent = new UserHouseRent();
            if (id != null)
            {
                userHouseRent = userHouseRentManager.GetById((int)id);
            }
            var users = userManager.Users.Include(c => c.Designation).Where(c => c.IsActive).ToList().Select(s => new
            {
                Text = s.EmployeeCode + "-" + s.FullName + "-" + s.Designation.Name,
                Value = s.Id

            }).ToList();
            ViewBag.Users = new SelectList(users, "Value", "Text");
            ViewBag.residentStatus = residentStatusManager.GetList();
            return View(userHouseRent);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(UserHouseRent h, String btnValue)
        {
          

            if (btnValue == "Save")
            {
                var houseRent = userHouseRentManager.GetByEmployeeId(h.AppUserId);
                if (houseRent == null)
                {
                    if (h.Amount > 0)
                    {

                        var result = userHouseRentManager.Add(h);
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
                        TempData["Error"] = "Amount should not less than or equal 0";
                    }
                    
                }
                else
                {
                    TempData["Error"] = "Already saved";
                }





            }
            else
            {
                var houseRent = userHouseRentManager.GetById(h.Id);

                if (h.Amount > 0)
                {
                    if (houseRent != null)
                    {
                        houseRent.ResidentStatusId = h.ResidentStatusId;
                        houseRent.Amount = h.Amount;


                        var result = userHouseRentManager.Update(houseRent);
                        if (result)
                        {
                            TempData["Success"] = "Successfully Update";
                        }
                        else
                        {
                            TempData["Error"] = "Failed Update";
                        }

                    }
                    else
                    {
                        TempData["Error"] = "No data found";
                    }
                }
                else
                {
                    TempData["Error"] = "Amount should not less than or equal 0";
                }

            }
            return RedirectToAction("List");
        }
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult List()
        {
            ViewBag.SuccessMessage = TempData["Success"];
            ViewBag.ErrorMessage = TempData["Error"];
            var list = userHouseRentManager.GetList();
            return View(list);
        }
    }
}
