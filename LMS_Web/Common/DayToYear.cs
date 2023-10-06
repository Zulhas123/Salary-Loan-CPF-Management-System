using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Web.Common
{
    public static class DayToYear
    {
        public static string Days(decimal days)
        {
            var totalYears = Math.Truncate(days / 365);
            var totalMonths = Math.Truncate((days % 365) / 30);
            var remainingDays = Math.Truncate((days % 365) % 30);

            return totalYears + " বছর " + totalMonths + " মাস " + remainingDays + " দিন";
        }
    }
}
