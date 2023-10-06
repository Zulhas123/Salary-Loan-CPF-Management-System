using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS_Web.Models;

namespace LMS_Web.Areas.Loan.Models
{
    public class InterestSlab:BaseClass
    {
        public int Id { get; set; }
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; set; }
        public decimal InterestRate { get; set; }
    }
}
