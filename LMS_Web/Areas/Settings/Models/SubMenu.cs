using System.ComponentModel.DataAnnotations;

namespace LMS_Web.Areas.Settings.Models
{
    public class SubMenu
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string ActiveSubMenuId { get; set; }
        public string ControllerName { get; set; }
        [Required]
        [MaxLength(100)]
        public string ActionName { get; set; }
        [MaxLength(100)]
        public string AreaName { get; set; }
        [Required]
        public int MainMenuId { get; set; }

        public virtual MainMenu MainMenu { get; set; }
    }
}
