using LMS_Web.Data;
using LMS_Web.Models;
using LMS_Web.SecurityExtension;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Web.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public DepartmentsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public async Task<IActionResult> Index()
        {
            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];
            var applicationDbContext = _context.Department.Include(d => d.CreatedBy).Include(d => d.UpdatedBy).Include(d => d.Wing);
            return View(await applicationDbContext.ToListAsync());
        }

        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult Create(int? id)
        {
            ViewBag.Error = TempData["Error"];
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["UpdatedById"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["WingId"] = new SelectList(_context.Wing, "Id", "Name");
            var department = _context.Department.Find(id ?? 0);
            return View(department);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department, string btnValue)
        {
            if (btnValue == "Update")
            {
                var userId = _userManager.GetUserId(User);
                var currentDepartment = await _context.Department.FindAsync(department.Id);
                if (currentDepartment != null)
                {
                    currentDepartment.Name = department.Name;
                    currentDepartment.NameBangla = department.NameBangla;
                    currentDepartment.WingId = department.WingId;
                    currentDepartment.UpdatedById = userId;
                    currentDepartment.UpdatedDateTime = DateTime.Now;
                    _context.Update(currentDepartment);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "ডিপার্টমেন্ট পরিবর্তন হয়েছে";

                }
                else
                {
                    TempData["Error"] = "ডিপার্টমেন্ট পরিবর্তন হয়নি";

                }
            }

            else
            {
                var userId = _userManager.GetUserId(User);
                department.CreatedById = userId;
                department.CreatedDateTime = DateTime.Now;
                department.IsActive = true;
                _context.Add(department);
                var result = await _context.SaveChangesAsync();
                if (result != 0)
                {
                    TempData["Success"] = "ডিপার্টমেন্ট সংরক্ষণ হয়েছে";
                }
                else
                {
                    TempData["Error"] = "ডিপার্টমেন্ট সংরক্ষণ হয়নি";
                }
            }
            return RedirectToAction("Index");
        }
        // [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var department = await _context.Department.FindAsync(id);
        //    if (department == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", department.CreatedById);
        //    ViewData["UpdatedById"] = new SelectList(_context.Users, "Id", "Id", department.UpdatedById);
        //    ViewData["WingId"] = new SelectList(_context.Wing, "Id", "Name", department.WingId);
        //    return View(department);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, Department department)
        //{
        //    if (id != department.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var userId = _userManager.GetUserId(User);
        //            var currentDepartment = await _context.Department.FindAsync(id);
        //            currentDepartment.Name = department.Name;
        //            currentDepartment.WingId = department.WingId;
        //            currentDepartment.UpdatedById = userId;
        //            currentDepartment.UpdatedDateTime = DateTime.Now;
        //            _context.Update(currentDepartment);
        //            await _context.SaveChangesAsync();
        //            TempData["Success"] = "ডিপার্টমেন্ট পরিবর্তন হয়েছে";
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!DepartmentExists(department.Id))
        //            {
        //                TempData["Error"] = "ডিপার্টমেন্ট পরিবর্তন হয়নি";
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", department.CreatedById);
        //    ViewData["UpdatedById"] = new SelectList(_context.Users, "Id", "Id", department.UpdatedById);
        //    ViewData["WingId"] = new SelectList(_context.Wing, "Id", "Name", department.WingId);
        //    return View(department);
        //}
        public async Task<IActionResult> Delete(int Id)
        {

            var getDepartment = await _context.Department.FindAsync(Id);
            try
            {
                _context.Remove(getDepartment);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "ইতিমধ্যে ডিপার্টমেন্ট ব্যবহার হচ্ছে এটা মুছে ফেলা যাবে না";
                return RedirectToAction(nameof(Index));
            }
            TempData["Success"] = "ডিপার্টমেন্ট মুছে ফেলা হয়েছে";
            return RedirectToAction(nameof(Index));
        }
        private bool DepartmentExists(int id)
        {
            return _context.Department.Any(e => e.Id == id);
        }
    }
}
