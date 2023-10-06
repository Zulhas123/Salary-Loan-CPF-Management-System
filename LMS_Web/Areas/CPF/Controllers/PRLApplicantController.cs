using LMS_Web.Areas.CPF.Interface;
using LMS_Web.Areas.CPF.Manager;
using LMS_Web.Areas.CPF.Models;
using LMS_Web.Areas.Settings.Manager;
using LMS_Web.Data;
using LMS_Web.Models;
using LMS_Web.SecurityExtension;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace LMS_Web.Areas.CPF.Controllers
{
    [Area("CPF")]
    public class PRLApplicantController : Controller
    {
        private UserManager<AppUser> userManager;
        private PRlApplicantInfoManager _plApplicantInfoManager;
        public PRLApplicantController(ApplicationDbContext db, UserManager<AppUser> _userManager)
        {
            userManager = _userManager;
            _plApplicantInfoManager = new PRlApplicantInfoManager(db);
        }
        public IActionResult List()
        {
            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];
            ViewBag.users = userManager.Users.ToList();
            var list = _plApplicantInfoManager.GetList();
            return View(list);
        }
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult Application(int Id)
        {
            var oldData =_plApplicantInfoManager.GetById(Id);
            var users = userManager.Users.Include(c => c.Designation).Where(c => c.IsActive).ToList().Select(s => new
            {
                Text = s.EmployeeCode + "-" + s.FullName + "_" + s.Designation.Name,
                Value = s.Id

            }).ToList();
            ViewBag.Users = new SelectList(users, "Value", "Text");
            //ViewBag.users = userManager.Users.Where(c=>c.IsActive).ToList();
            return View(oldData);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Application(PRlApplicantInfo prl, string btnValue)
        {
            try
            {
                var data = _plApplicantInfoManager.GetListByUser(prl.AppUserId);
                if (data == null)
                {
                        //PRlApplicantInfo prlinfo = new PRlApplicantInfo();
                        var userInfo = userManager.Users.FirstOrDefault(c => c.Id == prl.AppUserId);
                        var userDob = userInfo.BirthDate;
                        var prlDate = userDob.AddYears(userInfo.PlrAge);
                        if (prlDate <= prl.ApplicationDate)
                        {
                            prl.PrlDate = prlDate;
                            prl.IsApproved = true;
                            prl.ApproveDate=prl.ApplicationDate;
                            _plApplicantInfoManager.Add(prl);
                        }
                        else
                        {
                            TempData["Success"] = "PRL Application can not be applicable for this User!";
                        }
                    
                        
                    
                   
                }
                else
                {
                    data.ApplicationDate = prl.ApplicationDate;
                    data.ApproveDate = prl.ApplicationDate;
                    data.IsApproved = prl.IsApproved;
                    _plApplicantInfoManager.Update(data);
                    TempData["Success"] = "PRL Application Update Successfully";
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            return RedirectToAction("List");
        }
    }
}
