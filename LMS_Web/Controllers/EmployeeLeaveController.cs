using LMS_Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace LMS_Web.Controllers
{
    [Authorize]
    public class EmployeeLeaveController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeLeaveController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult EmployeeLeaveQuota()
        {
            var userLeaveQuotas = _context.UserLeaveQuotas
                .Include(q => q.Employee)
                .Include(q => q.LeaveType);
            return View(userLeaveQuotas);
        }

        public IActionResult EmployeeOnLeave()
        {
            var today = DateTime.Today;
            var userLeaveQuotas = _context.LeaveApplications
                    .Include(la => la.Applicant)
                    .ThenInclude(l => l.Department).Include(l => l.Applicant)
                    .ThenInclude(l => l.Designation)
                    .Where(la => la.IsApproved && today >= la.FromDate.Date && today <= la.ToDate.Date);
            return View(userLeaveQuotas);
        }
    }
}
