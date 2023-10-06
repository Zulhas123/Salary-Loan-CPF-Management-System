using LMS_Web.Data;
using LMS_Web.Models;
using System;
using System.Linq;

namespace LMS_Web.Common
{
    public class LeaveApproval
    {

        private static ApplicationDbContext db;

        public LeaveApproval(ApplicationDbContext _db)
        {
            db = _db;
        }

        public string ForwardCasualLeave(LeaveApplication leave, string nextApprovalPerson, DateTime fromDate, DateTime toDate)
        {
            leave.NextApprovedPersonId = nextApprovalPerson;
            leave.TotalDays = (toDate - fromDate).Days + 1;
            if (leave.FromDate != fromDate || leave.ToDate != toDate)
            {
                var leaveHistory = new LeaveHistory
                {
                    LeaveId = leave.Id,
                    FromDate = leave.FromDate,
                    ToDate = leave.ToDate
                };
                db.LeaveHistories.Add(leaveHistory);
            }

            leave.FromDate = fromDate;
            leave.ToDate = toDate;

            db.LeaveApplications.Update(leave);

            if (db.SaveChanges() > 0)
            {
                return "আবেদন ফরওয়ার্ড হয়েছে";
            }

            return "আবেদন ফরওয়ার্ড হয়নি";
        }


        public string EarnLeaveApproval(LeaveApplication leave)
        {
            var existingData = db.EarnLeave.FirstOrDefault(l => l.AppUserId == leave.ApplicantId && l.Type == leave.EarnLeaveType);

            if (existingData != null)
            {
                if (leave.IsHalfToFull)
                {
                    existingData.Obtain += leave.TotalDays;
                    existingData.Balance -= leave.TotalDays * 2;
                }
                else
                {
                    existingData.Obtain += leave.TotalDays;
                    existingData.Balance -= leave.TotalDays;
                }


                leave.IsApproved = true;
                db.EarnLeave.Update(existingData);
                db.LeaveApplications.Update(leave);
                if (db.SaveChanges() > 0)
                {
                    return "আবেদন অনুমোদিত হয়েছে";
                }
            }
            return "আবেদন অনুমোদিত হয়নি";
        }

        public string CasualLeave(LeaveApplication leaveApplication)
        {
            var totalAppliedLeave = leaveApplication.TotalDays;
            var userLeaveStatus = db.UserLeaveQuotas.FirstOrDefault(l => l.LeaveTypeId == leaveApplication.LeaveTypeId && l.EmployeeId == leaveApplication.ApplicantId);
            var balance = userLeaveStatus?.Balance;

            if (balance == 0)
            {
                return "ছুটির সংখ্যা শেষ";
            }

            if (totalAppliedLeave >= balance)
            {
                return "আবেদনকৃত ছুটির সংখ্যা সর্বমোট ছুটির চেয়ে বেশি";
            }

            if (userLeaveStatus != null)
            {
                leaveApplication.IsApproved = true;

                userLeaveStatus.Balance -= totalAppliedLeave;
                userLeaveStatus.LeaveObtain += totalAppliedLeave;

                db.LeaveApplications.Update(leaveApplication);
                db.UserLeaveQuotas.Update(userLeaveStatus);
                if (db.SaveChanges() > 0)
                {
                    return "আবেদন অনুমোদিত হয়েছে";
                }
            }



            return "আবেদন অনুমোদিত হয়নি";
        }




        public string ApproveOptionalLeave(LeaveApplication leaveApplication)
        {
            var userLeaveStatus =
                db.UserLeaveQuotas.FirstOrDefault(l => l.LeaveTypeId == leaveApplication.LeaveTypeId && l.EmployeeId == leaveApplication.ApplicantId && l.Year.Year == leaveApplication.FromDate.Year);

            leaveApplication.IsApproved = true;

            if (userLeaveStatus != null)
            {
                userLeaveStatus.Balance -= leaveApplication.TotalDays;
                userLeaveStatus.LeaveObtain += leaveApplication.TotalDays;

                db.LeaveApplications.Update(leaveApplication);
                db.UserLeaveQuotas.Update(userLeaveStatus);
            }

            if (db.SaveChanges() > 0)
            {
                return "আবেদন অনুমোদিত হয়েছে";
            }

            return "আবেদন অনুমোদিত হয়নি";
        }




        public string ApproveSpecialDisabilityLeave(LeaveApplication leaveApplication)
        {
            var userLeaveStatus = db.SpecialDisabilityLeave.FirstOrDefault(l => l.AppUserId == leaveApplication.ApplicantId);

            leaveApplication.IsApproved = true;
            if (userLeaveStatus != null)
            {
                userLeaveStatus.Obtain += leaveApplication.TotalDays;

                db.LeaveApplications.Update(leaveApplication);
                db.SpecialDisabilityLeave.Update(userLeaveStatus);
            }

            if (db.SaveChanges() > 0)
            {
                return "আবেদন অনুমোদিত হয়েছে";
            }

            return "আবেদন অনুমোদিত হয়নি";
        }




        public string ExtraOrdinaryLeave(LeaveApplication leaveApplication)
        {
            leaveApplication.IsApproved = true;
            db.LeaveApplications.Update(leaveApplication);

            if (db.SaveChanges() > 0)
            {
                return "আবেদন অনুমোদিত হয়েছে";
            }

            return "আবেদন অনুমোদিত হয়নি";
        }



        public string StudyLeave(LeaveApplication leaveApplication)
        {
            var userLeaveStatus = db.StudyLeave.FirstOrDefault(l => l.AppUserId == leaveApplication.ApplicantId);

            leaveApplication.IsApproved = true;
            //leaveApplication.NextApprovedPersonId = null;

            if (userLeaveStatus != null)
            {
                userLeaveStatus.Obtain += leaveApplication.TotalDays;

                db.LeaveApplications.Update(leaveApplication);
                db.StudyLeave.Update(userLeaveStatus);
            }
            else
            {
                StudyLeave studyLeave = new StudyLeave()
                {
                    AppUserId = leaveApplication.ApplicantId,
                    Obtain = leaveApplication.TotalDays
                };
                db.StudyLeave.Add(studyLeave);
            }
            return db.SaveChanges() > 0 ? "আবেদন অনুমোদিত হয়েছে" : "আবেদন অনুমোদিত হয়নি";
        }


        public string QuarantineLeave(LeaveApplication leaveApplication)
        {
            leaveApplication.IsApproved = true;
            db.LeaveApplications.Update(leaveApplication);

            return db.SaveChanges() > 0 ? "আবেদন অনুমোদিত হয়েছে" : "আবেদন অনুমোদিত হয়নি";
        }


        public string MaternityLeave(LeaveApplication leaveApplication)
        {
            var userLeaveStatus = db.MaternityLeave
                .FirstOrDefault(l => l.AppUserId == leaveApplication.ApplicantId);

            leaveApplication.IsApproved = true;

            if (userLeaveStatus != null)
            {
                userLeaveStatus.TakenTime += 1;

                db.LeaveApplications.Update(leaveApplication);
                db.MaternityLeave.Update(userLeaveStatus);
            }
            else
            {
                var maternityLeave = new MaternityLeave
                {
                    AppUserId = leaveApplication.ApplicantId,
                    TakenTime = 1
                };

                db.LeaveApplications.Update(leaveApplication);
                db.MaternityLeave.Add(maternityLeave);
            }
            return db.SaveChanges() > 0 ? "আবেদন অনুমোদিত হয়েছে" : "আবেদন অনুমোদিত হয়নি";
        }



        public string LeaveNotDue(LeaveApplication leaveApplication)
        {
            var userLeaveStatus = db.NotDueLeave.FirstOrDefault(l => l.AppUserId == leaveApplication.ApplicantId);

            leaveApplication.IsApproved = true;
            if (userLeaveStatus != null)
            {
                userLeaveStatus.Obtain += leaveApplication.TotalDays;
                userLeaveStatus.NonPaidAmount += leaveApplication.TotalDays;

                db.LeaveApplications.Update(leaveApplication);
                db.NotDueLeave.Update(userLeaveStatus);
            }

            return db.SaveChanges() > 0 ? "আবেদন অনুমোদিত হয়েছে" : "আবেদন অনুমোদিত হয়নি";
        }



        public string PlrLeave(LeaveApplication leaveApplication)
        {
            leaveApplication.IsApproved = true;
            db.LeaveApplications.Update(leaveApplication);

            return db.SaveChanges() > 0 ? "আবেদন অনুমোদিত হয়েছে" : "আবেদন অনুমোদিত হয়নি";
        }


        public string RAndRLeave(LeaveApplication leaveApplication)
        {
            var userLeaveStatus = db.RestAndRecreationLeave.Where(l => l.AppUserId == leaveApplication.ApplicantId)
                .OrderBy(x => x.Id)
                .FirstOrDefault();

            if (userLeaveStatus != null)
            {
                userLeaveStatus.TakenDate = DateTime.Now;


                var RnRLeave = new RestAndRecreationLeave()
                {
                    AppUserId = leaveApplication.ApplicantId,
                    NextAvailableDate = leaveApplication.FromDate.AddYears(3)
                };

                leaveApplication.IsApproved = true;
                // leaveApplication.NextApprovedPersonId = null;
                //if balance deduct from earn leave
                var earnLeave =
                    db.EarnLeave.FirstOrDefault(x => x.Type == 1 && x.AppUserId == leaveApplication.ApplicantId);
                if (earnLeave != null)
                {
                    earnLeave.Obtain += leaveApplication.TotalDays;
                    earnLeave.Balance -= leaveApplication.TotalDays;
                    db.EarnLeave.Update(earnLeave);
                }


                db.LeaveApplications.Update(leaveApplication);
                db.RestAndRecreationLeave.Update(userLeaveStatus);
                db.RestAndRecreationLeave.Add(RnRLeave);
            }

            return db.SaveChanges() > 0 ? "আবেদন অনুমোদিত হয়েছে" : "আবেদন অনুমোদিত হয়নি";
        }

        public string CompulsoryLeave(LeaveApplication leaveApplication)
        {
            leaveApplication.IsApproved = true;
            db.LeaveApplications.Update(leaveApplication);

            return db.SaveChanges() > 0 ? "আবেদন অনুমোদিত হয়েছে" : "আবেদন অনুমোদিত হয়নি";
        }

        public string WithoutPayLeave(LeaveApplication leaveApplication)
        {
            leaveApplication.IsApproved = true;
            db.LeaveApplications.Update(leaveApplication);

            return db.SaveChanges() > 0 ? "আবেদন অনুমোদিত হয়েছে" : "আবেদন অনুমোদিত হয়নি";
        }


        public string SickLeave(LeaveApplication leaveApplication)
        {
            leaveApplication.IsApproved = true;
            db.LeaveApplications.Update(leaveApplication);

            return db.SaveChanges() > 0 ? "আবেদন অনুমোদিত হয়েছে" : "আবেদন অনুমোদিত হয়নি";
        }









        public string RejectEarnLeave(LeaveApplication leave)
        {
            leave.IsRejected = true;
            //  leave.NextApprovedPersonId = null;
            db.LeaveApplications.Update(leave);
            return db.SaveChanges() > 0 ? "আবেদন বাতিল হয়েছে" : "আবেদন বাতিল হয়নি";
        }

        public string RejectCasualLeave(LeaveApplication leave)
        {
            leave.IsRejected = true;
            // leave.NextApprovedPersonId = null;
            db.LeaveApplications.Update(leave);
            return db.SaveChanges() > 0 ? "আবেদন বাতিল হয়েছে" : "আবেদন বাতিল হয়নি";
        }



        public string RejectOptionalLeave(LeaveApplication leave)
        {
            leave.IsRejected = true;
            //    leave.NextApprovedPersonId = null;
            db.LeaveApplications.Update(leave);
            return db.SaveChanges() > 0 ? "আবেদন বাতিল হয়েছে" : "আবেদন বাতিল হয়নি";
        }



        public string RejectSpecialDisabilityLeave(LeaveApplication leave)
        {
            leave.IsRejected = true;
            // leave.NextApprovedPersonId = null;
            db.LeaveApplications.Update(leave);
            return db.SaveChanges() > 0 ? "আবেদন বাতিল হয়েছে" : "আবেদন বাতিল হয়নি";
        }



        public string RejectExtraOrdinaryLeave(LeaveApplication leave)
        {
            leave.IsRejected = true;
            // leave.NextApprovedPersonId = null;
            db.LeaveApplications.Update(leave);
            return (db.SaveChanges() > 0) ? "আবেদন বাতিল হয়েছে" : "আবেদন বাতিল হয়নি";
        }



        public string RejectStudyLeave(LeaveApplication leave)
        {
            leave.IsRejected = true;
            //  leave.NextApprovedPersonId = null;
            db.LeaveApplications.Update(leave);

            return db.SaveChanges() > 0 ? "আবেদন বাতিল হয়েছে" : "আবেদন বাতিল হয়নি";
        }



        public string RejectQuarantineLeave(LeaveApplication leave)
        {
            leave.IsRejected = true;
            //   leave.NextApprovedPersonId = null;
            db.LeaveApplications.Update(leave);

            return db.SaveChanges() > 0 ? "আবেদন বাতিল হয়েছে" : "আবেদন বাতিল হয়নি";
        }


        public string RejectMaternityLeave(LeaveApplication leave)
        {
            leave.IsRejected = true;
            // leave.NextApprovedPersonId = null;
            db.LeaveApplications.Update(leave);
            return db.SaveChanges() > 0 ? "আবেদন বাতিল হয়েছে" : "আবেদন বাতিল হয়নি";
        }



        public string RejectLeaveNotDue(LeaveApplication leave)
        {
            leave.IsRejected = true;
            // leave.NextApprovedPersonId = null;
            db.LeaveApplications.Update(leave);
            return db.SaveChanges() > 0 ? "আবেদন বাতিল হয়েছে" : "আবেদন বাতিল হয়নি";
        }


        public string RejectPlrLeave(LeaveApplication leave)
        {
            leave.IsRejected = true;
            // leave.NextApprovedPersonId = null;
            db.LeaveApplications.Update(leave);
            return db.SaveChanges() > 0 ? "আবেদন বাতিল হয়েছে" : "আবেদন বাতিল হয়নি";
        }


        public string RejectRAndRLeave(LeaveApplication leave)
        {
            leave.IsRejected = true;
            //leave.NextApprovedPersonId = null;
            db.LeaveApplications.Update(leave);
            return db.SaveChanges() > 0 ? "আবেদন বাতিল হয়েছে" : "আবেদন বাতিল হয়নি";
        }


        public string RejectCompulsoryLeave(LeaveApplication leave)
        {
            leave.IsRejected = true;
            //  leave.NextApprovedPersonId = null;
            db.LeaveApplications.Update(leave);
            return db.SaveChanges() > 0 ? "আবেদন বাতিল হয়েছে" : "আবেদন বাতিল হয়নি";
        }


        public string RejectWithoutPayLeave(LeaveApplication leave)
        {

            leave.IsRejected = true;
            // leave.NextApprovedPersonId = null;
            db.LeaveApplications.Update(leave);
            return db.SaveChanges() > 0 ? "আবেদন বাতিল হয়েছে" : "আবেদন বাতিল হয়নি";
        }

        public string RejectSickLeave(LeaveApplication leave)
        {
            leave.IsRejected = true;
            // leave.NextApprovedPersonId = null;
            db.LeaveApplications.Update(leave);
            return db.SaveChanges() > 0 ? "আবেদন বাতিল হয়েছে" : "আবেদন বাতিল হয়নি";
        }


        public string ForwardSickLeave(LeaveApplication leave, string nextApprovalPerson, DateTime fromDate, DateTime toDate)
        {
            leave.NextApprovedPersonId = nextApprovalPerson;
            leave.TotalDays = (toDate - fromDate).Days + 1;
            if (leave.FromDate != fromDate || leave.ToDate != toDate)
            {
                var leaveHistory = new LeaveHistory
                {
                    LeaveId = leave.Id,
                    FromDate = leave.FromDate,
                    ToDate = leave.ToDate
                };
                db.LeaveHistories.Add(leaveHistory);
            }

            leave.FromDate = fromDate;
            leave.ToDate = toDate;

            db.LeaveApplications.Update(leave);
            if (db.SaveChanges() > 0)
            {
                return "আবেদন ফরওয়ার্ড হয়েছে";
            }

            return "আবেদন ফরওয়ার্ড হয়নি";
        }

        public string ForwardEarnLeave(LeaveApplication leave, string nextApprovalPerson, DateTime fromDate, DateTime toDate)
        {
            leave.NextApprovedPersonId = nextApprovalPerson;
            leave.TotalDays = (toDate - fromDate).Days + 1;
            if (leave.FromDate != fromDate || leave.ToDate != toDate)
            {
                var leaveHistory = new LeaveHistory
                {
                    LeaveId = leave.Id,
                    FromDate = leave.FromDate,
                    ToDate = leave.ToDate
                };
                db.LeaveHistories.Add(leaveHistory);
            }

            leave.FromDate = fromDate;
            leave.ToDate = toDate;
            db.LeaveApplications.Update(leave);
            if (db.SaveChanges() > 0)
            {
                return "আবেদন ফরওয়ার্ড হয়েছে";
            }

            return "আবেদন ফরওয়ার্ড হয়নি";
        }

        public string ForwardExtraOrdinaryLeave(LeaveApplication leave, string nextApprovalPerson, DateTime fromDate, DateTime toDate)
        {
            leave.NextApprovedPersonId = nextApprovalPerson;
            leave.TotalDays = (toDate - fromDate).Days + 1;
            if (leave.FromDate != fromDate || leave.ToDate != toDate)
            {
                var leaveHistory = new LeaveHistory
                {
                    LeaveId = leave.Id,
                    FromDate = leave.FromDate,
                    ToDate = leave.ToDate
                };
                db.LeaveHistories.Add(leaveHistory);
            }

            leave.FromDate = fromDate;
            leave.ToDate = toDate;
            db.LeaveApplications.Update(leave);
            if (db.SaveChanges() > 0)
            {
                return "আবেদন ফরওয়ার্ড হয়েছে";
            }

            return "আবেদন ফরওয়ার্ড হয়নি";
        }

        public string ForwardStudyLeave(LeaveApplication leave, string nextApprovalPerson, DateTime fromDate, DateTime toDate)
        {
            leave.NextApprovedPersonId = nextApprovalPerson;
            leave.TotalDays = (toDate - fromDate).Days + 1;
            if (leave.FromDate != fromDate || leave.ToDate != toDate)
            {
                var leaveHistory = new LeaveHistory
                {
                    LeaveId = leave.Id,
                    FromDate = leave.FromDate,
                    ToDate = leave.ToDate
                };
                db.LeaveHistories.Add(leaveHistory);
            }

            leave.FromDate = fromDate;
            leave.ToDate = toDate;
            db.LeaveApplications.Update(leave);
            if (db.SaveChanges() > 0)
            {
                return "আবেদন ফরওয়ার্ড হয়েছে";
            }

            return "আবেদন ফরওয়ার্ড হয়নি";
        }
        public string ForwardQuarantineLeave(LeaveApplication leave, string nextApprovalPerson, DateTime fromDate, DateTime toDate)
        {
            leave.NextApprovedPersonId = nextApprovalPerson;
            leave.TotalDays = (toDate - fromDate).Days + 1;
            if (leave.FromDate != fromDate || leave.ToDate != toDate)
            {
                var leaveHistory = new LeaveHistory
                {
                    LeaveId = leave.Id,
                    FromDate = leave.FromDate,
                    ToDate = leave.ToDate
                };
                db.LeaveHistories.Add(leaveHistory);
            }

            leave.FromDate = fromDate;
            leave.ToDate = toDate;
            db.LeaveApplications.Update(leave);
            if (db.SaveChanges() > 0)
            {
                return "আবেদন ফরওয়ার্ড হয়েছে";
            }

            return "আবেদন ফরওয়ার্ড হয়নি";
        }

        public string ForwardMaternityLeave(LeaveApplication leave, string nextApprovalPerson, DateTime fromDate, DateTime toDate)
        {
            leave.NextApprovedPersonId = nextApprovalPerson;
            leave.TotalDays = (toDate - fromDate).Days + 1;
            if (leave.FromDate != fromDate || leave.ToDate != toDate)
            {
                var leaveHistory = new LeaveHistory
                {
                    LeaveId = leave.Id,
                    FromDate = leave.FromDate,
                    ToDate = leave.ToDate
                };
                db.LeaveHistories.Add(leaveHistory);
            }

            leave.FromDate = fromDate;
            leave.ToDate = toDate;
            db.LeaveApplications.Update(leave);
            if (db.SaveChanges() > 0)
            {
                return "আবেদন ফরওয়ার্ড হয়েছে";
            }

            return "আবেদন ফরওয়ার্ড হয়নি";
        }

        public string ForwardLeaveNotDue(LeaveApplication leave, string nextApprovalPerson, DateTime fromDate, DateTime toDate)
        {
            leave.NextApprovedPersonId = nextApprovalPerson;
            leave.TotalDays = (toDate - fromDate).Days + 1;
            if (leave.FromDate != fromDate || leave.ToDate != toDate)
            {
                var leaveHistory = new LeaveHistory
                {
                    LeaveId = leave.Id,
                    FromDate = leave.FromDate,
                    ToDate = leave.ToDate
                };
                db.LeaveHistories.Add(leaveHistory);
            }

            leave.FromDate = fromDate;
            leave.ToDate = toDate;
            db.LeaveApplications.Update(leave);
            if (db.SaveChanges() > 0)
            {
                return "আবেদন ফরওয়ার্ড হয়েছে";
            }

            return "আবেদন ফরওয়ার্ড হয়নি";
        }

        public string ForwardPlrLeave(LeaveApplication leave, string nextApprovalPerson, DateTime fromDate, DateTime toDate)
        {
            leave.NextApprovedPersonId = nextApprovalPerson;
            leave.TotalDays = (toDate - fromDate).Days + 1;
            if (leave.FromDate != fromDate || leave.ToDate != toDate)
            {
                var leaveHistory = new LeaveHistory
                {
                    LeaveId = leave.Id,
                    FromDate = leave.FromDate,
                    ToDate = leave.ToDate
                };
                db.LeaveHistories.Add(leaveHistory);
            }

            leave.FromDate = fromDate;
            leave.ToDate = toDate;
            db.LeaveApplications.Update(leave);
            if (db.SaveChanges() > 0)
            {
                return "আবেদন ফরওয়ার্ড হয়েছে";
            }

            return "আবেদন ফরওয়ার্ড হয়নি";
        }

        public string ForwardOptionalLeave(LeaveApplication leave, string nextApprovalPerson, DateTime fromDate, DateTime toDate)
        {
            leave.NextApprovedPersonId = nextApprovalPerson;
            leave.TotalDays = (toDate - fromDate).Days + 1;
            if (leave.FromDate != fromDate || leave.ToDate != toDate)
            {
                var leaveHistory = new LeaveHistory
                {
                    LeaveId = leave.Id,
                    FromDate = leave.FromDate,
                    ToDate = leave.ToDate
                };
                db.LeaveHistories.Add(leaveHistory);
            }

            leave.FromDate = fromDate;
            leave.ToDate = toDate;
            db.LeaveApplications.Update(leave);
            if (db.SaveChanges() > 0)
            {
                return "আবেদন ফরওয়ার্ড হয়েছে";
            }

            return "আবেদন ফরওয়ার্ড হয়নি";
        }
        public string ForwardRAndRLeave(LeaveApplication leave, string nextApprovalPerson, DateTime fromDate, DateTime toDate)
        {
            leave.NextApprovedPersonId = nextApprovalPerson;
            leave.TotalDays = (toDate - fromDate).Days + 1;
            if (leave.FromDate != fromDate || leave.ToDate != toDate)
            {
                var leaveHistory = new LeaveHistory
                {
                    LeaveId = leave.Id,
                    FromDate = leave.FromDate,
                    ToDate = leave.ToDate
                };
                db.LeaveHistories.Add(leaveHistory);
            }

            leave.FromDate = fromDate;
            leave.ToDate = toDate;
            db.LeaveApplications.Update(leave);
            if (db.SaveChanges() > 0)
            {
                return "আবেদন ফরওয়ার্ড হয়েছে";
            }

            return "আবেদন ফরওয়ার্ড হয়নি";
        }

        public string ForwardSpecialDisabilityLeave(LeaveApplication leave, string nextApprovalPerson, DateTime fromDate, DateTime toDate)
        {
            leave.NextApprovedPersonId = nextApprovalPerson;
            leave.TotalDays = (toDate - fromDate).Days + 1;
            if (leave.FromDate != fromDate || leave.ToDate != toDate)
            {
                var leaveHistory = new LeaveHistory
                {
                    LeaveId = leave.Id,
                    FromDate = leave.FromDate,
                    ToDate = leave.ToDate
                };
                db.LeaveHistories.Add(leaveHistory);
            }

            leave.FromDate = fromDate;
            leave.ToDate = toDate;
            db.LeaveApplications.Update(leave);
            if (db.SaveChanges() > 0)
            {
                return "আবেদন ফরওয়ার্ড হয়েছে";
            }

            return "আবেদন ফরওয়ার্ড হয়নি";
        }

        public string ForwardCompulsoryLeave(LeaveApplication leave, string nextApprovalPerson, DateTime fromDate, DateTime toDate)
        {
            leave.NextApprovedPersonId = nextApprovalPerson;
            leave.TotalDays = (toDate - fromDate).Days + 1;

            if (leave.FromDate != fromDate || leave.ToDate != toDate)
            {
                var leaveHistory = new LeaveHistory
                {
                    LeaveId = leave.Id,
                    FromDate = leave.FromDate,
                    ToDate = leave.ToDate
                };
                db.LeaveHistories.Add(leaveHistory);
            }

            leave.FromDate = fromDate;
            leave.ToDate = toDate;
            db.LeaveApplications.Update(leave);
            if (db.SaveChanges() > 0)
            {
                return "আবেদন ফরওয়ার্ড হয়েছে";
            }

            return "আবেদন ফরওয়ার্ড হয়নি";
        }

        public string ForwardWithoutPayLeave(LeaveApplication leave, string nextApprovalPerson, DateTime fromDate, DateTime toDate)
        {
            leave.NextApprovedPersonId = nextApprovalPerson;
            leave.TotalDays = (toDate - fromDate).Days + 1;

            if (leave.FromDate != fromDate || leave.ToDate != toDate)
            {
                var leaveHistory = new LeaveHistory
                {
                    LeaveId = leave.Id,
                    FromDate = leave.FromDate,
                    ToDate = leave.ToDate
                };
                db.LeaveHistories.Add(leaveHistory);
            }

            leave.FromDate = fromDate;
            leave.ToDate = toDate;
            db.LeaveApplications.Update(leave);
            if (db.SaveChanges() > 0)
            {
                return "আবেদন ফরওয়ার্ড হয়েছে";
            }

            return "আবেদন ফরওয়ার্ড হয়নি";
        }
    }

}
