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
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Web.Areas.Settings.Controllers
{
    [Area("Settings")]
    public class UserWiseRoleController : Controller
    {

        private RoleManager<IdentityRole> roleManager;
        private UserManager<AppUser> userManager;


        public UserWiseRoleController(ApplicationDbContext db, RoleManager<IdentityRole> _roleManager, UserManager<AppUser> _userManager)
        {
            roleManager = _roleManager;
            userManager = _userManager;

        }
        [HttpGet]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public async Task<IActionResult> Add()
        {
            var users = userManager.Users.Include(c => c.Designation).Where(c => c.IsActive).ToList().Select(s => new
            {
                Text = s.EmployeeCode + "-" + s.FullName + "_" + s.Designation.Name,
                Value = s.Id

            }).ToList();
            ViewBag.Users = new SelectList(users, "Value", "Text");
            ViewBag.RoleList = roleManager.Roles.ToList();
            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(string AppUserId, string RoleId, string btnValue)
        {
            var user = userManager.Users.FirstOrDefault(c => c.Id == AppUserId);
            if(user != null) {
                var existingRole = userManager.GetRolesAsync(user).Result;
                foreach (var item in existingRole)
                {
                   await userManager.RemoveFromRoleAsync(user, item);
                }


                var result = await userManager.AddToRoleAsync(user, RoleId);
                if (result!=null)
                {
                    TempData["Success"] = "Role Added";
                }
                else
                {
                    TempData["Error"] = "Role Add Failed";
                }
            }
            return RedirectToAction("Add");

        }
        public string LoadRole(string userId)
        {
            var user=userManager.FindByIdAsync(userId).Result;
            var roleName = userManager.GetRolesAsync(user).Result;
            var role = roleManager.FindByNameAsync(roleName.FirstOrDefault());
            return role.Result.Name;
        }

    }
}
