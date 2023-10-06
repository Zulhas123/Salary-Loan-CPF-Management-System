using LMS_Web.Areas.Settings.Interface;
using LMS_Web.Areas.Settings.Manager;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Data;
using LMS_Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.SecurityExtension;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LMS_Web.Areas.Settings.Controllers
{
    [Area("Salary")]
    public class ChildrenInfoController : Controller
    {
        private ChildrenInfoManager childrenInfoManager;
        private UserManager<AppUser> userManager;
        public ChildrenInfoController(ApplicationDbContext db, UserManager<AppUser> _userManager)
        {
            userManager = _userManager;
            childrenInfoManager = new ChildrenInfoManager(db);
        }
        [HttpGet]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult Add(int? id)
        {
            ChildrenInfo childrenInfo = new ChildrenInfo();
            if (id != null)
            {
                childrenInfo = childrenInfoManager.GetById((int)id);
            }
            var users = userManager.Users.Include(c=>c.Designation).Where(c => c.IsActive).ToList().Select(s => new
            {
                Text = s.EmployeeCode + "-" + s.FullName +"-" +s.Designation.Name,
                Value = s.Id

            }).ToList();
            ViewBag.Users = new SelectList(users, "Value", "Text");
            return View(childrenInfo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(ChildrenInfo c, String btnValue)
        {
            if (btnValue == "Save")
            {
                var result = childrenInfoManager.Add(c);
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
                var Child = childrenInfoManager.GetById(c.Id);

                if (Child != null)
                {
                    Child.Id = c.Id;
                    Child.AppUserId = c.AppUserId;
                    Child.Name = c.Name;
                    Child.DateOfBirth = c.DateOfBirth;
                    Child.IsApprove =c.IsApprove;
                    Child.ApproveDate =c.ApproveDate;

                    var result = childrenInfoManager.Update(c);
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
            TempData["UserId"] = c.AppUserId;
            return RedirectToAction("List");
        }

        public IActionResult LoadUserChildreninfo(string userId)
        {
            var userChildren =childrenInfoManager.GetUserChildrenList(userId);
           bool isFound = true;
            if (userChildren.Count<=0)
            {
                isFound = false;
            }
            ViewBag.IsFound = isFound;
            ViewBag.LoadUserChildren = userChildren;
            return PartialView("_LoadUserChildreninfo");
        }

        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult List()
        {
            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];
            ViewBag.UserId = TempData["UserId"];
            var users = userManager.Users.Include(c => c.Designation).Where(c => c.IsActive).ToList().Select(s => new
            {
                Text = s.EmployeeCode + "-" + s.FullName + "-" + s.Designation.Name,
                Value = s.Id

            }).ToList();
            ViewBag.Users = new SelectList(users, "Value", "Text");
            return View();
        }
    }
}
