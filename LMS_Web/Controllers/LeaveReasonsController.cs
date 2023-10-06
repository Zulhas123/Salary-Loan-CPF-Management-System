using LMS_Web.Data;
using LMS_Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Web.Controllers
{
    public class LeaveReasonsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public LeaveReasonsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LeaveReason.Include(l => l.CreatedBy).Include(l => l.UpdatedBy);
            return View(await applicationDbContext.ToListAsync());
        }


        public IActionResult Create()
        {
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["UpdatedById"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveReason leaveReason)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                leaveReason.CreatedById = userId;
                leaveReason.IsActive = true;
                leaveReason.CreatedDateTime = DateTime.Now;
                _context.Add(leaveReason);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", leaveReason.CreatedById);
            ViewData["UpdatedById"] = new SelectList(_context.Users, "Id", "Id", leaveReason.UpdatedById);
            return View(leaveReason);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveReason = await _context.LeaveReason.FindAsync(id);
            if (leaveReason == null)
            {
                return NotFound();
            }
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", leaveReason.CreatedById);
            ViewData["UpdatedById"] = new SelectList(_context.Users, "Id", "Id", leaveReason.UpdatedById);
            return View(leaveReason);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveReason leaveReason)
        {
            if (id != leaveReason.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _userManager.GetUserId(User);
                    var currentLeaveReason = await _context.LeaveReason.FindAsync(id);

                    currentLeaveReason.Name = leaveReason.Name;
                    currentLeaveReason.UpdatedById = userId;
                    currentLeaveReason.UpdatedDateTime = DateTime.Now; ;

                    _context.Update(currentLeaveReason);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveReasonExists(leaveReason.Id))
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
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", leaveReason.CreatedById);
            ViewData["UpdatedById"] = new SelectList(_context.Users, "Id", "Id", leaveReason.UpdatedById);
            return View(leaveReason);
        }

        private bool LeaveReasonExists(int id)
        {
            return _context.LeaveReason.Any(e => e.Id == id);
        }
    }
}
