using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Web.Models
{
    public class NotDueLeave
    {
        public long Id { get; set; }
        public string AppUserId { get; set; }
        public int Obtain { get; set; }
        public int NonPaidAmount { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}
