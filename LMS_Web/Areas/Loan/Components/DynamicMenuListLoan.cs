using System.Collections.Generic;
using System.Linq;
using LMS_Web.Data;
using LMS_Web.Models;
using LMS_Web.SecurityExtension;
using LMS_Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Web.Areas.Loan.Components
{
    public class DynamicMenuListLoan : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        public DynamicMenuListLoan(ApplicationDbContext db,UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
           // roleSubMenuManager = new RoleSubMenuManager(db);
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IViewComponentResult Invoke()
        {
            var res = GetMenuByRole("Loan");
            return View(res);
        }
        public List<DynamicMenuVm> GetMenuByRole(string areaName = "")
        {
            List<DynamicMenuVm> dMList = new List<DynamicMenuVm>();
            var userId = User.Identity.GetUserEmail();
            if (userId != null)
            {
                var userData = _userManager.FindByNameAsync(userId);
                var roles = _userManager.GetRolesAsync(userData.Result);
                var roleName = roles.Result.FirstOrDefault();
                var role = _roleManager.FindByNameAsync(roleName);
                var roleId = role.Result.Id;
                var data = (from sr in _db.RoleSubMenus
                            join sub in _db.SubMenus on sr.SubMenuId equals sub.Id
                            join m in _db.MainMenus on sub.MainMenuId equals m.Id
                            where sr.RoleId == roleId.ToString() && sub.MainMenuId == m.Id 
                                                                 &&  sub.AreaName == areaName

                            select new DynamicMenuVm
                            {
                                MainMenuId = m.Id,
                                MainMenuName = m.Name,
                                Icon = m.Icon,
                                ActiveMenuId = m.ActiveMainMenuId,
                                AreaName = sub.AreaName,
                                Order=m.Order

                            }).Distinct().ToList();


                foreach (var item in data)
                {
                    DynamicMenuVm model = new DynamicMenuVm();
                    model.MainMenuId = item.MainMenuId;
                    model.MainMenuName = item.MainMenuName;
                    model.ActiveMenuId = item.ActiveMenuId;
                    model.Icon = item.Icon;
                    model.AreaName= item.AreaName;
                    model.Order = item.Order;
                    model.SubMenuLists = (from rm in _db.RoleSubMenus
                                          join sm in _db.SubMenus on rm.SubMenuId equals sm.Id
                                          join m in _db.MainMenus on sm.MainMenuId equals m.Id
                                          where rm.RoleId == roleId && m.Id == item.MainMenuId && sm.AreaName == areaName
                                          select sm).OrderBy(c => c.Id).ToList();
                    dMList.Add(model);

                }

            }

            return dMList.OrderBy(c=>c.Order).ToList();
        }
    }
}
