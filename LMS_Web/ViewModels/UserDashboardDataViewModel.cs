using LMS_Web.Models;
using System.Collections.Generic;

namespace LMS_Web.ViewModels
{
    public class UserDashboardDataViewModel
    {
        public int TotalLeaveCount { get; set; }

        public List<UserLeaveQuota> LeaveQuotas { get; set; }

    }
}
