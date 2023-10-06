using LMS_Web.Data;
using LMS_Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using LMS_Web.SecurityExtension;

namespace LMS_Web.Controllers
{
    [Authorize]
    public class DesignationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public DesignationsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public async Task<IActionResult> Index()
        {
            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];
            var applicationDbContext = _context.Designation.Include(d => d.CreatedBy).Include(d => d.UpdatedBy);
            return View(await applicationDbContext.ToListAsync());
        }

        public IActionResult Create(int? id)
        {
            ViewBag.Error = TempData["Error"];
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["UpdatedById"] = new SelectList(_context.Users, "Id", "Id");
            var designation = _context.Designation.Find(id ?? 0);
            return View(designation);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Designation designation,string btnValue)
        {
            if (btnValue == "Save")
            {
                var userId = _userManager.GetUserId(User);
                designation.CreatedById = userId;
                designation.CreatedDateTime = DateTime.Now;
            
                    _context.Add(designation);
                 var result=   await _context.SaveChangesAsync();
                if(result!=0) 
                { 
                    TempData["Success"] = "পদবি সংরক্ষণ হয়েছে";

                }
                else
                {
                    TempData["Error"] = "পদবি সংরক্ষণ হয়নি";
                }
            }
            else
            {
                var userId = _userManager.GetUserId(User);
                var currentDesignation = await _context.Designation.FindAsync(designation.Id);
                if (currentDesignation != null)
                {
                    currentDesignation.Name = designation.Name;
                    currentDesignation.NameBangla = designation.NameBangla;  
                    currentDesignation.DisgOrder = designation.DisgOrder;
                    currentDesignation.UpdatedById = userId;
                    currentDesignation.UpdatedDateTime = DateTime.Now;
                    _context.Update(currentDesignation);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "পদবি পরিবর্তন হয়েছে";

                }
                else
                {
                    TempData["Error"] = "পদবি পরিবর্তন হয়নি";

                }
            }

            return RedirectToAction("Index");
            
            
        }


        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var designation = await _context.Designation.FindAsync(id);
        //    if (designation == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", designation.CreatedById);
        //    ViewData["UpdatedById"] = new SelectList(_context.Users, "Id", "Id", designation.UpdatedById);
        //    return View(designation);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, Designation designation)
        //{
        //    if (id != designation.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var currentDesignation = await _context.Designation.FindAsync(id);
        //            var userId = _userManager.GetUserId(User);
        //            currentDesignation.UpdatedById = userId;
        //            currentDesignation.UpdatedDateTime = DateTime.Now;
        //            currentDesignation.Name = designation.Name;
        //            currentDesignation.NameBangla= designation.NameBangla;
        //            currentDesignation.DisgOrder = designation.DisgOrder;
        //            _context.Update(currentDesignation);
        //            await _context.SaveChangesAsync();
        //            TempData["Success"] = "পদবি পরিবর্তন হয়েছে";
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!DesignationExists(designation.Id))
        //            {
        //                TempData["Error"] = "পদবি পরিবর্তন হয়নি";
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", designation.CreatedById);
        //    ViewData["UpdatedById"] = new SelectList(_context.Users, "Id", "Id", designation.UpdatedById);
        //    return View(designation);
        //}

        public async Task<IActionResult> Delete(int Id)
        {

            var getDesignation = await _context.Designation.FindAsync(Id);
            try
            {
                _context.Remove(getDesignation);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "ইতিমধ্যে পদবি ব্যবহার হচ্ছে এটা মুছে ফেলা যাবে না";
                return RedirectToAction(nameof(Index));
            }
            TempData["Success"] = "পদবি মুছে ফেলা হয়েছে";
            return RedirectToAction(nameof(Index));
        }
        private bool DesignationExists(int id)
        {
            return _context.Designation.Any(e => e.Id == id);
        }

        public IActionResult GetDesignations(string prefix)
        {
            var designations = _context.Designation.Where(x => x.Name.StartsWith(prefix)).ToList();
            return Json(designations);
        }
    }
}
