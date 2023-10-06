using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LMS_Web.Data;
using LMS_Web.Models;
using LMS_Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Web.Components
{
    public class Navbar : ViewComponent
    {
        private ApplicationDbContext db;
        private UserManager<AppUser> userManager;
        public Navbar(ApplicationDbContext _db, UserManager<AppUser> _userManager)
        {
            userManager = _userManager;
            db = _db;

        }
        public IViewComponentResult Invoke()
        {
            var userId = userManager.GetUserId((ClaimsPrincipal)User);
            var lastSingIn = db.UserSignInHistory.Where(x => x.UserId == userId).OrderByDescending(c => c.Id).FirstOrDefault();
            var user = userManager.GetUserAsync((ClaimsPrincipal)User);
            var time = DateTime.Now;
            if (lastSingIn != null)
            {
                time = lastSingIn.LoginDateTime;

            }

            var image = "/image/no-image.jpg";
            if (!string.IsNullOrEmpty(user.Result.Image))
            {
                image = "/image/user/" + user.Result.Image;
            }
            NavModelVm model = new NavModelVm()
            {
                FullName = user.Result.FullName,
                Image = image,
                LastLoginDate = time,
                UserPhone = user.Result.UserName
            };


            return View(model);
        }
    }
}
