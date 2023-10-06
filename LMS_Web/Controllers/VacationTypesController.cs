using LMS_Web.Data;
using LMS_Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Web.Controllers
{

    [Authorize]
    public class VacationTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public VacationTypesController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {

            return View(await _context.LeaveType.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacationType = await _context.LeaveType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vacationType == null)
            {
                return NotFound();
            }

            return View(vacationType);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveType vacationType)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);

                vacationType.CreatedBy = userId;
                vacationType.CreatedDateTime = DateTime.Now;
                vacationType.UpdatedDateTime = null;

                _context.Add(vacationType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vacationType);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacationType = await _context.LeaveType.FindAsync(id);
            if (vacationType == null)
            {
                return NotFound();
            }
            return View(vacationType);
        }

        // POST: VacationTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CreatedBy,CreatedDateTime,UpdatedBy,UpdatedDateTime")] LeaveType vacationType)
        {
            if (id != vacationType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vacationType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VacationTypeExists(vacationType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vacationType);
        }

        // GET: VacationTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacationType = await _context.LeaveType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vacationType == null)
            {
                return NotFound();
            }

            return View(vacationType);
        }

        // POST: VacationTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vacationType = await _context.LeaveType.FindAsync(id);
            _context.LeaveType.Remove(vacationType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VacationTypeExists(int id)
        {
            return _context.LeaveType.Any(e => e.Id == id);
        }
    }
}
