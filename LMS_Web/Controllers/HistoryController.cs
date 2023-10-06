using LMS_Web.Data;
using LMS_Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace LMS_Web.Controllers
{
    [Authorize]
    public class HistoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HistoryController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult History()
        {
            var userId = _userManager.GetUserId(User);
            var history = _context.ApprovedHistory
                .Include(x => x.LeaveApplication)
                .ThenInclude(l => l.Applicant)
                .Include(x => x.LeaveApplication).ThenInclude(l => l.LeaveType)
                .Include(x => x.LeaveApplication).Where(x => x.CreatedById == userId);
            return View(history);
        }

        //public IActionResult RejectedHistory()
        //{
        //    var userId = _userManager.GetUserId(User);
        //    var history = _context.ApprovedHistory
        //        .Where(x => x.CreatedById == userId && x.OperationType == "বাতিল");
        //    return View();
        //}
        //public IActionResult ForwaredHistory()
        //{
        //    var userId = _userManager.GetUserId(User);
        //    var history = _context.ApprovedHistory
        //        .Where(x => x.CreatedById == userId && x.OperationType == "ফরওয়ার্ড");
        //    return View();
        //}
    }
}
