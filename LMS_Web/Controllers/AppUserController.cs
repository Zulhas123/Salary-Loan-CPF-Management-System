using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Areas.Salary.Manager;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Data;
using LMS_Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LMS_Web.Areas.Settings.Manager;
using LMS_Web.Areas.Settings.Models;
using Renci.SshNet.Common;
using LMS_Web.SecurityExtension;

namespace LMS_Web.Controllers
{

    [Authorize]
    public class AppUserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly SuspensionHistoryManager _suspensionHistoryManager;
        private readonly LienUserManager _lienUserManager;
        private readonly TransferHistoryManager transferHistoryManager;
        private string filePath;

        public AppUserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _suspensionHistoryManager = new SuspensionHistoryManager(context);
            _lienUserManager = new LienUserManager(context);
            transferHistoryManager = new TransferHistoryManager(context);
        }

        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult Index()
        {
            ViewBag.Success = TempData["SuccessMessage"];
            ViewBag.Error = TempData["ErrorMessage"];
            ViewData["Station"] = new SelectList(_context.Stations, "Id", "Name");
            var users = _context.Users
                .Where(c => c.IsActive)
                .Include(c => c.Wing)
                .Include(c => c.Section)
                .Include(c => c.Designation)
                .Include(c => c.Department)
                .ToList();

            return View(users);
        }
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult ArchiveList()
        {
            ViewBag.Success = TempData["SuccessMessage"];
            ViewBag.Error = TempData["ErrorMessage"];
            var users = _context.Users
                .Where(c => c.IsActive == false)
                .Include(c => c.Wing)
                .Include(c => c.Section)
                .Include(c => c.Designation)
                .Include(c => c.Department)
                .ToList();
            return View(users);
        }
        public IActionResult Reactive(string id)
        {
            var user = _context.Users.FirstOrDefault(c => c.Id == id);
            if (user != null)
            {
                user.IsActive = true;
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Successfully re activated user";
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult UpdateUserInfo()
        {
            var user = _userManager.GetUserAsync(User).Result;
            ViewData["Wing"] = new SelectList(_context.Wing.Where(c => c.IsActive), "Id", "Name");
            ViewData["Designation"] = new SelectList(_context.Designation, "Id", "Name");
            ViewData["Department"] = new SelectList(_context.Department, "Id", "Name");
            ViewData["Grade"] = new SelectList(_context.Grades, "Id", "Name");
            ViewData["GradeStep"] = new SelectList(_context.GradeSteps, "Id", "Name");
            ViewData["Station"] = new SelectList(_context.Stations, "Id", "Name");
            ViewData["ResidentialStatus"] = new SelectList(_context.ResidentStatus, "Id", "Name");
            ViewData["Success"] = TempData["SuccessMessage"];
            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateUserInfo(AppUser appUser)
        {

            var userId = _userManager.GetUserId(User);
            var user = await _userManager.GetUserAsync(HttpContext.User);

            string uplaodsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "image/user/");

            string uniqueFileName = null;
            if (appUser.ImagePath != null)
            {
                uniqueFileName = Guid.NewGuid().ToString() + "_" + appUser.ImagePath.FileName;
                filePath = Path.Combine(uplaodsFolder, uniqueFileName);


                await using var stream = new FileStream(filePath, FileMode.Create);
                await appUser.ImagePath.CopyToAsync(stream);
            }
            else
            {
                uniqueFileName = user.Image;
            }



            // user.Id = user.Id;

            user.Gender = appUser.Gender;
            user.FullName = appUser.FullName;
            user.FullNameBangla = appUser.FullNameBangla;
            user.StationId = appUser.StationId;
            user.GradeId = appUser.GradeId;
            user.CurrentGradeId = appUser.CurrentGradeId;
            user.CurrentBasic = appUser.CurrentBasic;
            user.BankAccountNo = appUser.BankAccountNo;
            user.BankAccountNoBangla = string.Concat(appUser.BankAccountNo.Select(c => (char)('\u09E6' + c - '0')));
            user.EmployeeCodeBangla = string.Concat(user.EmployeeCode.Select(c => (char)('\u09E6' + c - '0')));
            user.ResidentialStatusId = appUser.ResidentialStatusId;
            user.StationId = appUser.StationId;
            user.Image = uniqueFileName;
            user.DepartmentId = appUser.DepartmentId;
            user.DesignationId = appUser.DesignationId;
            user.SectionId = appUser.SectionId;
            user.WingId = appUser.WingId;
            user.UpdatedBy = userId;
            user.UpdatedDateTime = DateTime.Now;
            user.NID = appUser.NID;
            user.Type = appUser.Type;
            if (appUser.BirthDate != DateTime.MinValue)
            {
                user.BirthDate = appUser.BirthDate;
            }

            if (appUser.JoiningDate != DateTime.MinValue)
            {
                user.JoiningDate = appUser.JoiningDate;
            }

            user.Religion = appUser.Religion;
            user.PhoneNumber = appUser.PhoneNumber;
            user.UserName = appUser.PhoneNumber;
            user.NormalizedUserName = appUser.PhoneNumber;
            user.IsLiveNear3Km = appUser.IsLiveNear3Km;
            user.IsAllowedForChargeAllowance = appUser.IsAllowedForChargeAllowance;

            var res = _context.SaveChanges();

            if (res > 0)
            {
                TempData["SuccessMessage"] = "ইউজার আপডেট হয়েছে";
            }
            else
            {
                TempData["ErrorMessage"] = "ইউজার আপডেট হয়নি";
            }

            return RedirectToAction("UpdateUserInfo");
        }
        public IActionResult AddTransferUser(string userId, int toStation, DateTime newStationJoinDate, DateTime currentStationLastOfficeDate)
        {
            TransferHistory transfer = new TransferHistory();
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);
            if (user != null)
            {
                transfer.AppUserId = user.Id;
                transfer.FromStationId = user.StationId ?? 0;
                transfer.ToStationId = toStation;
                transfer.TransferDate = currentStationLastOfficeDate;
                transfer.CreatedById = _userManager.GetUserId(User);
                transfer.CreatedDateTime = DateTime.Now;
                var res = transferHistoryManager.Add(transfer);

                user.CurrentStationJoiningDate = newStationJoinDate;
                user.StationId = toStation;
                _userManager.UpdateAsync(user);
                if (res)
                {
                    TempData["SuccessMessage"] = "Successfully Added";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed Add";
                }

            }
            else
            {
                TempData["ErrorMessage"] = "User Not Found";
            }


            return RedirectToAction("Index");

        }

        public IActionResult ResignUser(string resignUserId, DateTime resignationDate)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == resignUserId);
            if (user != null)
            {
                user.ResignationDate = resignationDate;
                _userManager.UpdateAsync(user);
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> SuspendUser(string SuspensionuserId, DateTime StartDate, DateTime EndDate)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == SuspensionuserId);
            if (user != null)
            {
                SuspensionHistory suspensionHistory = new SuspensionHistory();
                suspensionHistory.AppUserId = user.Id;
                suspensionHistory.StartDate = StartDate;
                suspensionHistory.EndDate = EndDate;
                suspensionHistory.CreatedById = _userManager.GetUserId(User);
                user.IsSuspended = true;
                _context.SaveChanges();
                var result = _suspensionHistoryManager.Add(suspensionHistory);
                if (result)
                {
                    TempData["SuccessMessage"] = "Susspended successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to susspend";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Data not found";
            }
            return RedirectToAction("SuspendedList");

        }

        public async Task<IActionResult> LienUser(string id)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                LienUser lienUser = new LienUser();
                lienUser.StartDate = DateTime.Now;
                lienUser.AppUserId = id;
                lienUser.CreatedById = _userManager.GetUserId(User);
                user.IsLien = true;
                _context.SaveChanges();
                var result = _lienUserManager.Add(lienUser);
                if (result)
                {
                    TempData["SuccessMessage"] = "User Liened Successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "Not Liened";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Data not found";
            }
            return RedirectToAction("LienedList");
        }
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult SuspendedList()
        {
            ViewBag.Success = TempData["SuccessMessage"];
            ViewBag.Error = TempData["ErrorMessage"];
            var history = _suspensionHistoryManager.GetAll();
            return View(history);
        }
        public IActionResult RemoveSuspend(int id)
        {
            var suspendUser = _suspensionHistoryManager.GetById(id);
            if (suspendUser != null)
            {
                var user = _userManager.Users.FirstOrDefault(x => x.Id == suspendUser.AppUserId);
                user.IsSuspended = false;
                _context.SaveChanges();
                var result = _suspensionHistoryManager.Delete(suspendUser);

                if (result)
                {
                    TempData["SuccessMessage"] = "Remove Suspended";
                }
                else
                {
                    TempData["ErrorMessage"] = "Not removed";
                }
            }

            else
            {
                TempData["ErrorMessage"] = "Data not found";
            }

            return RedirectToAction("SuspendedList");
        }

        public IActionResult UpdateSusspension(int id, DateTime startDate, DateTime endDate)
        {
            var suss = _suspensionHistoryManager.GetById(id);
            if (suss != null)
            {
                suss.StartDate = startDate;
                suss.EndDate = endDate;
                if (_suspensionHistoryManager.Update(suss))
                {
                    TempData["SuccessMessage"] = "Successfully updated";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Data not found";
            }

            return RedirectToAction("SuspendedList");
        }


        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult LienedList()
        {
            ViewBag.Success = TempData["SuccessMessage"];
            ViewBag.Error = TempData["ErrorMessage"];
            var users = _context.Users
                .Where(c => c.IsLien == true)
                .Include(c => c.Wing)
                .Include(c => c.Section)
                .Include(c => c.Designation)
                .Include(c => c.Department)
                .ToList();

            return View(users);
        }
        public IActionResult RemoveLien(string id)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                user.IsLien = false;
            }
            var lienUser = _lienUserManager.GetByUserId(id);
            if (lienUser != null)
            {
                lienUser.EndDate = DateTime.Now;
            }
            _context.SaveChanges();
            var result=_lienUserManager.Update(lienUser);
            if (result)
            {
                TempData["SuccessMessage"] = "Lien Removed";
            }
            else
            {
                TempData["ErrorMessage "] = "Lien Removed Failed";
            }
            return RedirectToAction("LienedList");
        }
        [HttpGet]
        public IActionResult UpdateUserInfoForAdmin(string id)
        {

            ViewData["Wing"] = new SelectList(_context.Wing.Where(c => c.IsActive), "Id", "Name");
            ViewData["Designation"] = new SelectList(_context.Designation, "Id", "Name");
            ViewData["Department"] = new SelectList(_context.Department, "Id", "Name");
            ViewData["Grade"] = new SelectList(_context.Grades, "Id", "Name");
            ViewData["GradeStep"] = new SelectList(_context.GradeSteps, "Id", "Name");
            ViewData["Station"] = new SelectList(_context.Stations, "Id", "Name");
            ViewData["ResidentialStatus"] = new SelectList(_context.ResidentStatus, "Id", "Name");
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            ViewData["Success"] = TempData["SuccessMessage"];
            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateUserInfoForAdmin(AppUser appUser)
        {
            var userId = appUser.Id;
            var user = _context.Users.FirstOrDefault(x => x.Id == appUser.Id);
            //if (user.PhoneNumber != appUser.PhoneNumber)
            //{
            //    var users = _userManager.Users;

            //    if (users.Any(x => x.PhoneNumber == appUser.PhoneNumber))
            //    {
            //        ModelState.AddModelError("DuplicateMobileNo", "উপরোক্ত নাম্বার দিয়ে ইতিপূর্বে কর্মকর্তা তৈরী করা হয়েছে");
            //    }

            //}
            _context.Entry(user).State = EntityState.Detached;
            string previousUserImageUrl = user.Image;
            string uplaodsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "image/user/");
            bool isImageModified = appUser.ImagePath != null;
            string uniqueFileName = null;
            if (appUser.ImagePath != null)
            {
                uniqueFileName = Guid.NewGuid().ToString() + "_" + appUser.ImagePath.FileName;
                filePath = Path.Combine(uplaodsFolder, uniqueFileName);
                await using var stream = new FileStream(filePath, FileMode.Create);
                await appUser.ImagePath.CopyToAsync(stream);
            }
            else
            {
                uniqueFileName = user.Image;
            }
            user.Id = user.Id;
            user.Gender = appUser.Gender;
            user.FullName = appUser.FullName;
            user.FullNameBangla = appUser.FullNameBangla;
            user.StationId = appUser.StationId;
            user.GradeId = appUser.GradeId;
            user.CurrentGradeId = appUser.CurrentGradeId;
            user.CurrentBasic = appUser.CurrentBasic;
            user.BankAccountNo = appUser.BankAccountNo;
            user.BankAccountNoBangla = string.Concat(appUser.BankAccountNo.Select(c => (char)('\u09E6' + c - '0')));
            user.EmployeeCodeBangla = string.Concat(user.EmployeeCode.Select(c => (char)('\u09E6' + c - '0')));
            user.ResidentialStatusId = appUser.ResidentialStatusId;
            user.StationId = appUser.StationId;
            user.Image = uniqueFileName;
            user.DepartmentId = appUser.DepartmentId;
            user.DesignationId = appUser.DesignationId;
            user.EmployeeCode = appUser.EmployeeCode;
            user.PhoneNumber = appUser.PhoneNumber;
            user.UserName = appUser.PhoneNumber;
            user.NormalizedUserName = appUser.PhoneNumber;
            user.SectionId = appUser.SectionId;
            user.WingId = appUser.WingId;
            user.UpdatedBy = userId;
            user.UpdatedDateTime = DateTime.Now;
            user.NID = appUser.NID;
            user.Type = appUser.Type;
            user.IsLiveNear3Km = appUser.IsLiveNear3Km;
            user.IsAllowedForChargeAllowance = appUser.IsAllowedForChargeAllowance;
            user.JoiningDate = appUser.JoiningDate;
            user.Religion = appUser.Religion;
            user.BirthDate = appUser.BirthDate;
            user.JoiningDate = appUser.JoiningDate;
            user.CPFStartDate = appUser.CPFStartDate;
            var result = _context.Entry(user).State = EntityState.Modified;
            var saved = _context.SaveChanges();

            if (saved > 0)
            {
                //if (isImageModified)
                //{
                //    if (System.IO.File.Exists(Path.Combine(uplaodsFolder, previousUserImageUrl)))
                //    {

                //        System.IO.File.Delete(Path.Combine(uplaodsFolder, previousUserImageUrl));
                //    }

                //}

                TempData["SuccessMessage"] = "ইউজার আপডেট হয়েছে";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "ইউজার আপডেট হয়নি";

            }

            return RedirectToAction("UpdateUserInfoForAdmin");
        }



        public IActionResult Roles()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }


        public IActionResult AddRole() => View();

        [HttpPost]
        public async Task<IActionResult> AddRole([Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                    return RedirectToAction("Index");
            }
            return View(name);
        }

        public IActionResult Delete(string id)
        {
            var user = _context.Users.Find(id);

            user.IsActive = false;

            _context.Update(user);

            if (_context.SaveChanges() > 0)
            {
                TempData["ErrorMessage"] = "কর্মকর্তা মুছে ফেলা হয়েছে";
            }
            return RedirectToAction("ArchiveList");
        }
        public async Task<IActionResult> ResetPassword(string id, string password, string confirmPassword)
        {
            if (password == null)
            {
                TempData["ErrorMessage"] = "Password can't be null";
                return RedirectToAction("Index");
            }
            if (password != confirmPassword)
            {
                TempData["ErrorMessage"] = "Password not matched";
                return RedirectToAction("Index");

            }
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetPassResult = await _userManager.ResetPasswordAsync(user, token, password);
                if (resetPassResult.Succeeded)
                {
                    TempData["SuccessMessage"] = "Password reset successful";
                    return RedirectToAction("Index");
                }

            }
            TempData["ErrorMessage"] = "Password reset failed";
            return RedirectToAction("Index");
        }
    }
}
