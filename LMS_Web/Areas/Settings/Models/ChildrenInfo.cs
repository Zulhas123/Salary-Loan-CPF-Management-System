using System;
using System.ComponentModel.DataAnnotations.Schema;
using LMS_Web.Models;

namespace LMS_Web.Areas.Settings.Models
{
    public class ChildrenInfo:BaseClass
    {
        public int Id { get; set; }

        [ForeignKey("AppUser")]
        public string  AppUserId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsApprove { get; set; }
        public string MemorandumNo { get; set; }
        public DateTime ApproveDate { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}
