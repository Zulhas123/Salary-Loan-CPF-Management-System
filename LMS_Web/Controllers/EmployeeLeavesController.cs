using LMS_Web.Data;
using LMS_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Web.Controllers
{
    public class EmployeeLeavesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeLeavesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EmployeeLeaves
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.EmployeeLeave.Include(e => e.Employee);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: EmployeeLeaves/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeLeave = await _context.EmployeeLeave
                .Include(e => e.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeeLeave == null)
            {
                return NotFound();
            }

            return View(employeeLeave);
        }

        // GET: EmployeeLeaves/Create
        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: EmployeeLeaves/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AppUserId,LeaveTypeId,TotalLeave,LeaveTaken,LeaveBalance,Year,CreatedBy,CreatedDateTime,UpdatedBy,UpdatedDateTime")] EmployeeLeave employeeLeave)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employeeLeave);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", employeeLeave.EmployeeId);
            return View(employeeLeave);
        }

        // GET: EmployeeLeaves/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeLeave = await _context.EmployeeLeave.FindAsync(id);
            if (employeeLeave == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", employeeLeave.EmployeeId);
            return View(employeeLeave);
        }

        // POST: EmployeeLeaves/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AppUserId,LeaveTypeId,TotalLeave,LeaveTaken,LeaveBalance,Year,CreatedBy,CreatedDateTime,UpdatedBy,UpdatedDateTime")] EmployeeLeave employeeLeave)
        {
            if (id != employeeLeave.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employeeLeave);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeLeaveExists(employeeLeave.Id))
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
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", employeeLeave.EmployeeId);
            return View(employeeLeave);
        }

        // GET: EmployeeLeaves/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeLeave = await _context.EmployeeLeave
                .Include(e => e.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeeLeave == null)
            {
                return NotFound();
            }

            return View(employeeLeave);
        }

        // POST: EmployeeLeaves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employeeLeave = await _context.EmployeeLeave.FindAsync(id);
            _context.EmployeeLeave.Remove(employeeLeave);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeLeaveExists(int id)
        {
            return _context.EmployeeLeave.Any(e => e.Id == id);
        }
    }
}
