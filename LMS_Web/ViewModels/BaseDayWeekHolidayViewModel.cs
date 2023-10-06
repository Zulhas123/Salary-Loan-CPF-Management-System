using LMS_Web.Models;
using System.Collections.Generic;

namespace LMS_Web.ViewModels
{
    public class BaseDayWeekHolidayViewModel
    {
        public List<DayWeekHolidayViewModel> DayWeekHolidayViewModels { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string Week { get; set; }
        public Holiday Holiday { get; set; }



        public BaseDayWeekHolidayViewModel()
        {
            DayWeekHolidayViewModels = new List<DayWeekHolidayViewModel>();
        }
    }
}