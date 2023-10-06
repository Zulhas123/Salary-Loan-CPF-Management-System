using System.Collections.Generic;

namespace LMS_Web.ViewModels
{

    public class LeaveCalenderViewModel
    {
        public List<DayWeekViewModel> DayWeekViewModels { get; set; }
        public List<LeaveInfoViewModel> LeaveInfoViewModels { get; set; }

        //public Leave Vacation { get; set; }
    }
    public class DayWeekViewModel
    {
        public int Day { get; set; }
        public string Week { get; set; }
    }

    public class LeaveInfoViewModel
    {
        public int Day { get; set; }
        public string Week { get; set; }

        public string Type { get; set; }

        public string Details { get; set; }
    }
}
