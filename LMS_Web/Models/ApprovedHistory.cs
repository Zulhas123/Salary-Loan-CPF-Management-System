namespace LMS_Web.Models
{
    public class ApprovedHistory : BaseClass
    {
        public long Id { get; set; }
        public long LeaveApplicationId { get; set; }
        public string OperationType { get; set; }
        public virtual LeaveApplication LeaveApplication { get; set; }




    }
}
