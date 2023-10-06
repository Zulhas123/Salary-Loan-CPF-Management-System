using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS_Web.Areas.Settings.ViewModels;
using LMS_Web.Data;
using LMS_Web.Models;
using LMS_Web.SecurityExtension;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Web.Areas.Settings.Controllers
{
    [Area("Settings")]
    public class RoleController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;

        }
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult Create()
        {

            var roleList = _roleManager.Roles.ToList();
            List<RolesVm> roles = new List<RolesVm>();
            foreach (var role in roleList)
            {
                RolesVm r = new RolesVm();
                r.Id = role.Id;
                r.Name = role.Name;
                roles.Add(r);
            }

            ViewBag.SuccessMessage = TempData["Message"];
            ViewBag.ErrorMessage = TempData["Error"];
            return View(roles);



        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(string roleName, string btnValue, string id)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                TempData["Error"] = "Role should not blank";
                return RedirectToAction("Create");
            }
            bool x = await _roleManager.RoleExistsAsync(roleName);
            if (x)
            {

                TempData["Error"] = "Role already exist";
                return RedirectToAction("Create");
            }

            if (btnValue == "Update")
            {
                var getRole=_roleManager.FindByIdAsync(id).Result;
                getRole.Name = roleName;
               await _roleManager.UpdateAsync(getRole);
                TempData["Message"] = "Successfully Updated";
            }
            else
            {
                var role = new IdentityRole();
                role.Name = roleName;
                await _roleManager.CreateAsync(role);
                TempData["Message"] = "Successfully Added";
            }

            return RedirectToAction("Create");

        }
        [HttpPost]
        //[MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var getRole = _roleManager.FindByIdAsync(roleId).Result;
            if (getRole != null)
            {
                await _roleManager.DeleteAsync(getRole);
                TempData["Message"] = "Successfully Deleted";
            }
            return RedirectToAction("Create");
        }
    }
}
