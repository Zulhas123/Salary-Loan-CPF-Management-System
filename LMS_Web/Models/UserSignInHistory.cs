using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Web.Models
{
    public class UserSignInHistory
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public DateTime LoginDateTime { get; set; }
    }
}
