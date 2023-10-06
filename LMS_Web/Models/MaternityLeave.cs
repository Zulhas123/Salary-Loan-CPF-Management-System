using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Web.Models
{
    public class MaternityLeave
    {
        public long Id { get; set; }
        public string AppUserId { get; set; }
        public int TakenTime { get; set; }
        public  AppUser AppUser { get; set; }
    }
}
