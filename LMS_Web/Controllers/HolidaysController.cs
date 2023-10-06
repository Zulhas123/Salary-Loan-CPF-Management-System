using LMS_Web.Data;
using LMS_Web.Models;
using LMS_Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Web.Controllers
{

    [Authorize]
    public class HolidaysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HolidaysController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(GetCalenderHoliday(DateTime.Now));
        }


        public IActionResult GetHolidays(DateTime monthYear)
        {
            return PartialView("_HolidayCalender", GetCalenderHoliday(monthYear));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holiday = await _context.Holiday
                .FirstOrDefaultAsync(m => m.Id == id);
            if (holiday == null)
            {
                return NotFound();
            }

            return View(holiday);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Holiday holiday)
        {
            if (ModelState.IsValid)
            {
                _context.Add(holiday);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(holiday);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holiday = await _context.Holiday.FindAsync(id);
            if (holiday == null)
            {
                return NotFound();
            }
            return View(holiday);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Holiday holiday)
        {
            if (id != holiday.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(holiday);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HolidayExists(holiday.Id))
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
            return View(holiday);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holiday = await _context.Holiday
                .FirstOrDefaultAsync(m => m.Id == id);
            if (holiday == null)
            {
                return NotFound();
            }

            return View(holiday);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var holiday = await _context.Holiday.FindAsync(id);
            _context.Holiday.Remove(holiday);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HolidayExists(int id)
        {
            return _context.Holiday.Any(e => e.Id == id);
        }



        public CalenderHolidayViewModel GetCalenderHoliday(DateTime monthYear)
        {

            int days = DateTime.DaysInMonth(monthYear.Year, monthYear.Month);

            List<DayWeekHolidayViewModel> dayWeekHolidayViewModels = new List<DayWeekHolidayViewModel>();

            //var holidays = _context.Holiday.Where(x => x.FromDate.Year == monthYear.Year && x.FromDate.Month == monthYear.Month);
            var holidays = _context.Holiday.Where(x => x.FromDate.Year == monthYear.Year && x.FromDate.Month == monthYear.Month || x.ToDate.Year == monthYear.Year && x.ToDate.Month == monthYear.Month);

            var holidaysForView = new List<HolidayViewModel>();

            foreach (var item in holidays)
            {
                if (item.FromDate.Month != monthYear.Month && item.ToDate.Month == monthYear.Month)
                {
                    var month = item.ToDate.Month;
                    var day = item.ToDate.Day;
                    var name = item.Name;
                    //var lastDay = item.ToDate.Day;

                    //var distance = lastDay - day;
                    if (day != 0)
                    {
                        for (int i = 1; i <= day; i++)
                        {
                            var holy = new HolidayViewModel
                            {
                                Month = month,
                                Day = i,
                                Name = name
                            };
                            holidaysForView.Add(holy);
                        }
                    }
                    else
                    {
                        var holy = new HolidayViewModel
                        {
                            Month = month,
                            Day = day,
                            Name = name
                        };
                        holidaysForView.Add(holy);
                    }
                }

                else
                {
                    var month = item.FromDate.Month;
                    var day = item.FromDate;
                    var dayCount = day.Day;

                    var name = item.Name;
                    var lastDay = item.ToDate;

                    var distance = lastDay - day;
                    var distanceCount = distance.Days;

                    if (distanceCount != 0)
                    {
                        for (int i = 0; i <= distanceCount; i++)
                        {
                            var holy = new HolidayViewModel
                            {
                                Month = month,
                                Day = dayCount++,
                                Name = name
                            };
                            holidaysForView.Add(holy);
                        }
                    }
                    else
                    {
                        var holy = new HolidayViewModel
                        {
                            Month = month,
                            Day = dayCount,
                            Name = name
                        };
                        holidaysForView.Add(holy);
                    }
                }

            }

            var calenderHolidayViewModel = new CalenderHolidayViewModel();
            for (int dayCounter = 1; dayCounter <= days; dayCounter++)
            {
                string day_of_week = (Convert.ToDateTime("" + monthYear.Year + "-" + monthYear.Month + "-" + dayCounter + "").DayOfWeek).ToString();

                string weekName = day_of_week.Substring(0, 3);

                var holiday = holidaysForView.FirstOrDefault(x => x.Day == dayCounter);

                var dayWeekHolidayViewModel = new DayWeekHolidayViewModel()
                {
                    Day = dayCounter,
                    Week = weekName,
                    Name = holiday?.Name
                };


                dayWeekHolidayViewModels.Add(dayWeekHolidayViewModel);

                calenderHolidayViewModel = new CalenderHolidayViewModel
                {
                    Month = monthYear.Month,
                    Week = weekName,
                    Year = monthYear.Year,
                    DayWeekHolidayViewModels = dayWeekHolidayViewModels
                };

            }
            return calenderHolidayViewModel;
        }
    }
}
