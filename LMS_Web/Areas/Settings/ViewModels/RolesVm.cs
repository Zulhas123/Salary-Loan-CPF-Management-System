using System.ComponentModel.DataAnnotations;

namespace LMS_Web.Areas.Settings.ViewModels
{
    public class RolesVm
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Enter Name")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Please enter email address")]
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string RoleId { get; set; }
        public string Image { get; set; }
        [Required(ErrorMessage = "Select Ministry")]
        public int MinistryId { get; set; }
        [Required(ErrorMessage = "Select Designation")]
        public int DesignationId { get; set; }
        public int EmployeeId { get; set; }
    }
}
