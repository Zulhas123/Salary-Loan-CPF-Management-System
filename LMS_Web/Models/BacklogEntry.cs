namespace LMS_Web.Models
{
    public class BacklogEntry
    {
        public int Id { get; set; }
        public string ApplicantId { get; set; }
        public int LeaveTypeId { get; set; }

        public int Days { get; set; }
    }
}
