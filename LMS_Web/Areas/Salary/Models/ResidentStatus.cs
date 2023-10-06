using System.Collections.Generic;
using LMS_Web.Models;

namespace LMS_Web.Areas.Salary.Models
{
    public class ResidentStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsUserSpecific { get; set; }

        public ICollection<AppUser> AppUsers { get; set; }

    }
}
