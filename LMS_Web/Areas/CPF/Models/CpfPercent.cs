using LMS_Web.Models;

namespace LMS_Web.Areas.CPF.Models
{
    public class CpfPercent:BaseClass
    {
        public int Id { get; set; }
        public string  Name { get; set; }
        public decimal  Percent{ get; set; }
    }
}
