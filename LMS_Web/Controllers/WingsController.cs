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
    public class WingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public WingsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public async Task<IActionResult> Index()
        {
            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];
            var applicationDbContext = _context.Wing.Include(w => w.CreatedBy).Include(w => w.UpdatedBy);
            return View(await applicationDbContext.ToListAsync());
        }


        public IActionResult Create(int? id)
        {
            ViewBag.Error = TempData["Error"];
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["UpdatedById"] = new SelectList(_context.Users, "Id", "Id");
            var wing = _context.Wing.Find(id ?? 0);
            return View(wing);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Wing wing, string btnValue)
        {
            if (btnValue == "Update")
            {
                var userId = _userManager.GetUserId(User);
                var oldWing = await _context.Wing.FindAsync(wing.Id);
                if (oldWing != null)
                {
                    oldWing.Name = wing.Name;
                    oldWing.UpdatedById = userId;
                    oldWing.UpdatedDateTime = DateTime.Now;
                    _context.Update(oldWing);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "উইং পরিবর্তন হয়েছে";
                }
                else
                {
                    TempData["Error"] = "উইং পরিবর্তন হয়নি";
                }
            }
            else
            {
                var userId = _userManager.GetUserId(User);
                wing.CreatedById = userId;
                wing.CreatedDateTime = DateTime.Now;
                wing.IsActive = true;
                _context.Add(wing);
                var result = await _context.SaveChangesAsync();
                if (result != 0)
                {
                    TempData["Success"] = "উইং সংরক্ষণ হয়েছে";
                }
                else
                {
                    TempData["Error"] = "উইং সংরক্ষণ হয়নি";
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int Id)
        {

            var getWing = await _context.Wing.FindAsync(Id);
            try
            {
                _context.Remove(getWing);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "ইতিমধ্যে উইং ব্যবহার হচ্ছে এটা মুছে ফেলা যাবে না";
                return RedirectToAction(nameof(Index));
            }
            TempData["Success"] = "উইং মুছে ফেলা হয়েছে";
            return RedirectToAction(nameof(Index));
        }

        private bool WingExists(int id)
        {
            return _context.Wing.Any(e => e.Id == id);
        }
    }
}
