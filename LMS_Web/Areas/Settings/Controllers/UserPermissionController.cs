using System;
using System.Collections.Generic;
using System.Linq;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Areas.Settings.Interface;
using LMS_Web.Areas.Settings.Manager;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Areas.Settings.ViewModels;
using LMS_Web.Data;
using LMS_Web.Models;
using LMS_Web.SecurityExtension;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LMS_Web.Areas.Settings.Controllers
{
    [Area("Settings")]
    public class UserPermissionController : Controller
    {
        private StationManager stationManager;
        private UserManager<AppUser> userManager;
        private UserStationPermissionManager userStationPermissionManager;
        public UserPermissionController(ApplicationDbContext db, UserManager<AppUser> _userManager)
        {
            stationManager = new StationManager(db);
            userManager = _userManager;
            userStationPermissionManager = new UserStationPermissionManager(db);
        }
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult Add()
        {
            //ViewBag.Station = stationManager.GetAll();
            var users = userManager.Users.Include(c=>c.Designation).Where(c => c.IsActive).ToList().Select(s => new
            {
                Text = s.EmployeeCode + "-" + s.FullName + "-"+s.Designation.Name,
                Value = s.Id

            }).ToList();
            ViewBag.Users = new SelectList(users, "Value", "Text");
            return View();
        }
        public bool SaveStation(string stationIds, string userId)
        {
            List<UserStationPermission> list = new List<UserStationPermission>();
            dynamic submenuIdList = JsonConvert.DeserializeObject(stationIds);
            try
            {
                foreach (var Id in submenuIdList)
                {
                    UserStationPermission model = new UserStationPermission();
                    model.AppUserId = userId;
                    model.StationId = Convert.ToInt32(Id.ToString());
                    list.Add(model);
                }

                var oldData = userStationPermissionManager.GetByUserId(userId);
                if (oldData.Any())
                {
                    bool delete = userStationPermissionManager.Delete(oldData);
                }


                if (userStationPermissionManager.Add(list))
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public IActionResult LoadStation(string userId)
        {
            var station = stationManager.GetAll();
            var alreadyExist = userStationPermissionManager.GetByUserId(userId);
            List<StationVm> list = new List<StationVm>();
            foreach (var s in station)
            {
                StationVm st = new StationVm();
                var check = alreadyExist.FirstOrDefault(c => c.StationId == s.Id);
                if (check != null)
                {
                    st.HasPermission = true;
                }
                st.Address=s.Address;
                st.Id=s.Id;
                st.Name=s.Name;
                list.Add(st);

            }
            return PartialView("_LoadStation", list);
        }
    }
}
