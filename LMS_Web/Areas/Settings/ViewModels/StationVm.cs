using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace LMS_Web.Areas.Settings.ViewModels
{
    public class StationVm
    {
        public int Id { get; set; }
    
        public string Name { get; set; }
        public string Address { get; set; }
        public bool HasPermission { get; set; }
    }
}
