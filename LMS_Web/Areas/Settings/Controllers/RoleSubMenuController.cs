using System;
using System.Collections.Generic;
using System.Linq;
using LMS_Web.Areas.Settings.Manager;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Areas.Settings.ViewModels;
using LMS_Web.Data;
using LMS_Web.SecurityExtension;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LMS_Web.Areas.Settings.Controllers
{
    [Area("Settings")]
    public class RoleSubMenuController : Controller
    {
        private RoleSubMenuManager roleSubMenuManager;
        private SubMenuManager subMenuManager;
        private MainMenuManager mainMenuManager;
        private RoleManager<IdentityRole> roleManager;
      

        public RoleSubMenuController(ApplicationDbContext db, RoleManager<IdentityRole> _roleManager)
        {
            roleSubMenuManager = new RoleSubMenuManager(db);
            roleManager = _roleManager;
           
            subMenuManager = new SubMenuManager(db);
            mainMenuManager = new MainMenuManager(db);
           
        }
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult Add()
        {
            ViewBag.RoleList = roleManager.Roles.ToList();
            return View();
        }

        public IActionResult SubMenuList(string roleId)
        {
            List<MainMenuVm> mainMenuList = new List<MainMenuVm>();

            var allSubmenu = subMenuManager.GetAll().ToList();
            var mainMenuData = mainMenuManager.GetAll();
            var mainMenu = allSubmenu.GroupBy(x => x.MainMenuId);
            var checkSubmenu = roleSubMenuManager.GetByRoleId(roleId).Select(c => c.SubMenuId);
            foreach (var main in mainMenu)
            {
                MainMenuVm mainModel = new MainMenuVm();
                mainModel.MainMenuId = main.Key;
                mainModel.MainMenuName = mainMenuData.FirstOrDefault(x => x.Id == main.Key).Name;
                List<RoleSubMenuVm> subList = new List<RoleSubMenuVm>();

                foreach (var sub in main.ToList())
                {
                    RoleSubMenuVm model = new RoleSubMenuVm();
                    model.SubmenuId = sub.Id;
                    model.SubmenuName = sub.Name;
                    if (checkSubmenu.Contains(sub.Id))
                    {
                        model.IsChecked = true;
                    }
                    else
                    {
                        model.IsChecked = false;
                    }

                    subList.Add(model);
                }

                mainModel.SubmenuList = subList;
                mainMenuList.Add(mainModel);


            }
            return PartialView("_SubMenuList", mainMenuList);
        }
        public bool InsertRoleMapping(string submenuId, string roleId)
        {
            List<RoleSubMenu> list = new List<RoleSubMenu>();
            dynamic submenuIdList = JsonConvert.DeserializeObject(submenuId);
            try
            {
                foreach (var Id in submenuIdList)
                {
                    RoleSubMenu model = new RoleSubMenu();
                    model.RoleId = roleId;
                    model.SubMenuId = Convert.ToInt32(Id.ToString());
                    list.Add(model);
                }

                var oldData = roleSubMenuManager.GetByRoleId(roleId);
                if (oldData.Any())
                {
                    bool delete = roleSubMenuManager.Delete(oldData);
                }


                if (roleSubMenuManager.Add(list))
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
    }
}
