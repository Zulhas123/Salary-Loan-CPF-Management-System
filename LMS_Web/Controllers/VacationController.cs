using LMS_Web.Data;
using LMS_Web.Models;
using LMS_Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace LMS_Web.Controllers
{

    [Authorize]
    public class VacationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VacationController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            int days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);


            List<DayWeekViewModel> daysWithWeeks = new List<DayWeekViewModel>();
            for (int dayCounter = 1; dayCounter <= days; dayCounter++)
            {
                string day_of_week = (Convert.ToDateTime("" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + dayCounter + "").DayOfWeek).ToString();
                var dayWithweek = dayCounter + " " + day_of_week;

                string weekName = day_of_week.Substring(0, 3);

                var dayWithWeek = new DayWeekViewModel()
                {
                    Day = dayCounter,
                    Week = weekName
                };




                daysWithWeeks.Add(dayWithWeek);

            }

            LeaveCalenderViewModel leaveCalender = new LeaveCalenderViewModel()
            {
                DayWeekViewModels = daysWithWeeks,
                //LeaveInfoViewModels = 
            };

            //ViewBag.DaysWithWeeks = daysWithWeeks;
            return View(leaveCalender);
        }



        public IActionResult Save()
        {
            ViewData["LeaveType"] = new SelectList(_context.LeaveType, "Id", "Name");
            return View();
        }


        [HttpPost]
        public IActionResult Save(Leave leave)
        {
            ViewData["LeaveType"] = new SelectList(_context.LeaveType, "Id", "Name");
            return View();
        }
    }
}
