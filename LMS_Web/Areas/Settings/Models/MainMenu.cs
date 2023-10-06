namespace LMS_Web.Areas.Settings.Models
{
    public class MainMenu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ActiveMainMenuId { get; set; }
        public string Icon { get; set; }
        public int? Order { get; set; }
    }
}
