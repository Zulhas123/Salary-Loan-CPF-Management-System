using System.Collections.Generic;
using LMS_Web.Areas.Settings.Models;

namespace LMS_Web.ViewModels
{
    public class DynamicMenuVm
    {
        public int MainMenuId { get; set; }
        public string Icon { get; set; }
        public string MainMenuName { get; set; }
        public string ActiveMenuId { get; set; }
        public string AreaName { get; set; }
        public int? Order { get; set; }
        public List<SubMenu> SubMenuLists { get; set; }
    }
}
