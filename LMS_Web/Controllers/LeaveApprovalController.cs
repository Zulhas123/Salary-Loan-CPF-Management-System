using LMS_Web.Common;
using LMS_Web.Data;
using LMS_Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace LMS_Web.Controllers
{
    [Authorize]
    public class LeaveApprovalController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private IConfiguration configuration;

        private readonly LeaveApproval _leaveApproval;

        public LeaveApprovalController(ApplicationDbContext context, UserManager<AppUser> userManager, IConfiguration _configuration)
        {
            _context = context;
            _userManager = userManager;
            _leaveApproval = new LeaveApproval(_context);
            configuration = _configuration;
        }

        public IActionResult ApproveApplication(long id, string userId)
        {
            bool isBacklog = true;
            if (string.IsNullOrEmpty(userId))
            {
                userId = _userManager.GetUserId(User);
                isBacklog = false;

            }
                 
           
            ApprovedHistory approvedHistory = new ApprovedHistory
            {
                LeaveApplicationId = id,
                CreatedById = userId,
                CreatedDateTime = DateTime.Now,
                OperationType = "অনুমোদিত"
            };

            _context.Add(approvedHistory);
            _context.SaveChanges();



            var returnMessage = "";
            var leave = _context.LeaveApplications.Find(id);
            var user = _context.Users.FirstOrDefault(x => x.Id == leave.ApplicantId);
            switch (leave.LeaveTypeId)
            {
                case 1:
                    returnMessage = _leaveApproval.CasualLeave(leave);
                    break;
                case 2:
                    returnMessage = _leaveApproval.SickLeave(leave);
                    break;
                case 3:
                    returnMessage = _leaveApproval.EarnLeaveApproval(leave);
                    break;
                case 4:
                    returnMessage = _leaveApproval.ExtraOrdinaryLeave(leave);
                    break;
                case 5:
                    returnMessage = _leaveApproval.StudyLeave(leave);
                    break;
                case 6:
                    returnMessage = _leaveApproval.QuarantineLeave(leave);
                    break;
                case 7:

                    if (user?.Gender == "মহিলা")
                    {
                        returnMessage = _leaveApproval.MaternityLeave(leave);
                    }
                    else
                    {
                        returnMessage = "শুধুমাত্র মহিলাদের জন্য প্রযোজ্য";
                    }
                    break;
                case 8:
                    returnMessage = _leaveApproval.LeaveNotDue(leave);
                    break;
                case 9:
                    returnMessage = _leaveApproval.PlrLeave(leave);
                    break;
                case 10:
                    returnMessage = _leaveApproval.ApproveOptionalLeave(leave);
                    break;
                case 11:
                    returnMessage = _leaveApproval.RAndRLeave(leave);
                    break;
                case 12:
                    returnMessage = _leaveApproval.ApproveSpecialDisabilityLeave(leave);
                    break;
                case 13:
                    returnMessage = _leaveApproval.CompulsoryLeave(leave);
                    break;
                case 14:
                    returnMessage = _leaveApproval.WithoutPayLeave(leave);
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;

            }

            if (isBacklog)
            {
                return RedirectToAction("SaveUserApplicationAdmin", "Leaves");
            }
            TempData["ReturnMessage"] = returnMessage;
            Utility utility = new Utility(_context, configuration);
            utility.CalculateSingleUser(user);
            return RedirectToAction("Index", "Leaves");
        }


        public string RejectApplication(long id, string remarks)
        {
            var userId = _userManager.GetUserId(User);
            ApprovedHistory approvedHistory = new ApprovedHistory
            {
                LeaveApplicationId = id,
                CreatedById = userId,
                CreatedDateTime = DateTime.Now,
                OperationType = "বাতিল"
            };

            _context.Add(approvedHistory);
            _context.SaveChanges();

            var returnMessage = "";
            var leave = _context.LeaveApplications.Find(id);
            leave.CancellationRemarks = remarks;
            leave.RejectedById = _userManager.GetUserId(User);
            leave.RejectedDate = DateTime.Now;
            switch (leave.LeaveTypeId)
            {
                case 1:
                    returnMessage = _leaveApproval.RejectCasualLeave(leave);
                    break;
                case 2:
                    returnMessage = _leaveApproval.RejectSickLeave(leave);
                    break;
                case 3:
                    returnMessage = _leaveApproval.RejectEarnLeave(leave);
                    break;
                case 4:
                    returnMessage = _leaveApproval.RejectExtraOrdinaryLeave(leave);
                    break;
                case 5:
                    returnMessage = _leaveApproval.RejectStudyLeave(leave);
                    break;
                case 6:
                    returnMessage = _leaveApproval.RejectQuarantineLeave(leave);
                    break;
                case 7:
                    var user = _context.Users.FirstOrDefault(x => x.Gender == "মহিলা");
                    if (user?.Gender == "মহিলা")
                    {
                        _leaveApproval.RejectMaternityLeave(leave);
                    }
                    else
                    {
                        returnMessage = "শুধুমাত্র মহিলাদের জন্য প্রযোজ্য";
                    }
                    break;
                case 8:
                    returnMessage = _leaveApproval.RejectLeaveNotDue(leave);
                    break;
                case 9:
                    returnMessage = _leaveApproval.RejectPlrLeave(leave);
                    break;
                case 10:
                    returnMessage = _leaveApproval.RejectOptionalLeave(leave);
                    break;
                case 11:
                    returnMessage = _leaveApproval.RejectRAndRLeave(leave);
                    break;
                case 12:
                    returnMessage = _leaveApproval.RejectSpecialDisabilityLeave(leave);
                    break;
                case 13:
                    returnMessage = _leaveApproval.RejectCompulsoryLeave(leave);
                    break;
                case 14:
                    returnMessage = _leaveApproval.RejectWithoutPayLeave(leave);
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;

            }
            //ViewBag.ReturnMessage = returnMessage;
            return returnMessage;
        }





        public IActionResult Forward(long id, int leaveTypeId, string applicantId, DateTime fromDate, DateTime toDate)
        {
            var userId = _userManager.GetUserId(User);
            ApprovedHistory approvedHistory = new ApprovedHistory
            {
                LeaveApplicationId = id,
                CreatedById = userId,
                CreatedDateTime = DateTime.Now,
                OperationType = "ফরওয়ার্ড"
            };

            _context.Add(approvedHistory);
            _context.SaveChanges();

            var returnMessage = "";
            var leave = _context.LeaveApplications.Find(id);

            switch (leave.LeaveTypeId)
            {
                case 1:
                    returnMessage = _leaveApproval.ForwardCasualLeave(leave, applicantId, fromDate, toDate);
                    break;
                case 2:
                    returnMessage = _leaveApproval.ForwardSickLeave(leave, applicantId, fromDate, toDate);
                    break;
                case 3:
                    returnMessage = _leaveApproval.ForwardEarnLeave(leave, applicantId, fromDate, toDate);
                    break;
                case 4:
                    returnMessage = _leaveApproval.ForwardExtraOrdinaryLeave(leave, applicantId, fromDate, toDate);
                    break;
                case 5:
                    returnMessage = _leaveApproval.ForwardStudyLeave(leave, applicantId, fromDate, toDate);
                    break;
                case 7:
                    returnMessage = _leaveApproval.ForwardQuarantineLeave(leave, applicantId, fromDate, toDate);
                    break;
                case 6:
                    returnMessage = _leaveApproval.ForwardMaternityLeave(leave, applicantId, fromDate, toDate);
                    break;
                case 8:
                    returnMessage = _leaveApproval.ForwardLeaveNotDue(leave, applicantId, fromDate, toDate);
                    break;
                case 9:
                    returnMessage = _leaveApproval.ForwardPlrLeave(leave, applicantId, fromDate, toDate);
                    break;
                case 10:
                    returnMessage = _leaveApproval.ForwardOptionalLeave(leave, applicantId, fromDate, toDate);
                    break;
                case 11:
                    returnMessage = _leaveApproval.ForwardRAndRLeave(leave, applicantId, fromDate, toDate);
                    break;
                case 12:
                    returnMessage = _leaveApproval.ForwardSpecialDisabilityLeave(leave, applicantId, fromDate, toDate);
                    break;
                case 13:
                    returnMessage = _leaveApproval.ForwardCompulsoryLeave(leave, applicantId, fromDate, toDate);
                    break;
                case 14:
                    returnMessage = _leaveApproval.ForwardWithoutPayLeave(leave, applicantId, fromDate, toDate);
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;

            }
            ViewBag.ReturnMessage = returnMessage;
            return RedirectToAction("Index", "Leaves");
        }
    }
}
