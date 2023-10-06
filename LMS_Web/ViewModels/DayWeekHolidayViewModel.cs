using System.Collections.Generic;

namespace LMS_Web.ViewModels
{
    public class DayWeekHolidayViewModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string Week { get; set; }
        public string Name { get; set; }

    }

    public class CalenderHolidayViewModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string Week { get; set; }

        public List<DayWeekHolidayViewModel> DayWeekHolidayViewModels = new List<DayWeekHolidayViewModel>();

    }

    public class HolidayViewModel
    {
        public int Day { get; set; }
        public int Month { get; set; }

        public string Name { get; set; }
    }
}