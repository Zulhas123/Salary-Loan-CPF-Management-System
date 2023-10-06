using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Web.Models
{
    public class RestAndRecreationLeave
    {
        public long Id { get; set; }
        public string AppUserId { get; set; }
        public DateTime NextAvailableDate{get;set;}
        public DateTime? TakenDate { get; set; }
        public virtual  AppUser AppUser { get; set; }
    }
}
