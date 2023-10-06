using LMS_Web.Data;
using LMS_Web.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCore.Reporting;


namespace LMS_Web.Common
{
    public class Utility
    {
        private static ApplicationDbContext db;
      

        private IConfiguration configuration;
        public Utility(ApplicationDbContext _db, IConfiguration _configuration)
        {
            db = _db;
            configuration = _configuration;
           
        }


        public static string EarnLeave(decimal days)
        {
            var totalYears = Math.Truncate(days / 365);
            var totalMonths = Math.Truncate((days % 365) / 30);
            var remainingDays = Math.Truncate((days % 365) % 30);

            return totalYears + " বছর " + totalMonths + " মাস " + remainingDays + " দিন";
        }



        public string EarnLeave(LeaveApplication leave, string convertToFull)
        {
            try
            {
                var earnLeave = db.EarnLeave.FirstOrDefault(l => l.AppUserId == leave.ApplicantId);
                if (earnLeave != null)
                {
                    var halfToFullTotalDay = leave.TotalDays;
                    if (convertToFull == "ConvertedToFull")
                    {
                        halfToFullTotalDay = leave.TotalDays * 2;
                        leave.IsHalfToFull = true;
                    }
                    if (halfToFullTotalDay > earnLeave.Balance)
                    {
                        return "আবেদনকৃত ছুটির পরিমান অবশিষ্ট ছুটির চেয়ে বেশি";
                    }
                    db.LeaveApplications.Add(leave);

                    if (db.SaveChanges() > 0)
                    {
                        return "আবেদন সম্পূর্ণ হয়েছে";
                    }

                }

                return "আবেদন সম্পূর্ণ হয় নাই";
            }
            catch (Exception e)
            {
                return "আবেদন সম্পূর্ণ হয় নাই";
            }

        }
        public string SickLeave(LeaveApplication leave)
        {
            db.LeaveApplications.Add(leave);

            if (db.SaveChanges() > 0)
            {
                return "আবেদন সম্পূর্ণ হয়েছে";
            }
            return "আবেদন সম্পূর্ণ হয় নাই";
        }

        public string ExtraOrdinaryLeave(LeaveApplication leaveApplication, string userId)
        {
            try
            {
                if (leaveApplication.ApplicantId == null)
                {
                    leaveApplication.ApplicantId = userId;

                }
                leaveApplication.CreatedById = userId;
                db.LeaveApplications.Add(leaveApplication);
                if (db.SaveChanges() > 0)
                {
                    return "আবেদন সম্পূর্ণ হয়েছে";
                }
                return "আবেদন সম্পূর্ণ হয় নাই";
            }
            catch (Exception e)
            {
                return "আবেদন সম্পূর্ণ হয় নাই";
            }


        }

        public string StudyLeave(LeaveApplication leaveApplication, string userId)
        {
            try
            {
                string max = configuration.GetSection("MyKey").GetSection("StudyLeave").Value;
                var data = db.StudyLeave.FirstOrDefault(x => x.AppUserId == userId);
                var obtain = 0;
                if (data != null)
                {
                    obtain = data.Obtain;

                }

                var balance = Convert.ToInt32(max) - obtain;
                var differ = leaveApplication.ToDate - leaveApplication.FromDate;
                var dateCount = differ.Days;
                if (dateCount > balance)
                {
                    return "আবেদনকৃত ছুটির পরিমান অবশিষ্ট ছুটির চেয়ে বেশি";
                }
                db.LeaveApplications.Add(leaveApplication);
                if (db.SaveChanges() > 0)
                {
                    return "আবেদন সম্পূর্ণ হয়েছে";
                }


                return "আবেদন সম্পূর্ণ হয় নাই";

            }
            catch (Exception e)
            {
                return "আবেদন সম্পূর্ণ হয় নাই";
            }

        }

        public string QuarantineLeave(LeaveApplication leaveApplication, string userId)
        {

            try
            {
                int max = Convert.ToInt32(configuration.GetSection("MyKey").GetSection("ExtraOrdinaryLeaveLimit").Value);

                var dateDifference = leaveApplication.ToDate - leaveApplication.FromDate;
                var dateCount = dateDifference.Days;

                if (dateCount > max)
                {
                    return "আপনার আবেদনকৃত ছুটির সংখ্যা সংগনিরোধ ছুটির নির্দিষ্ট পরিমানের চেয়ে বেশি।";
                }
                db.LeaveApplications.Add(leaveApplication);
                if (db.SaveChanges() > 0)
                {
                    return "আপনার আবেদন সফল ভাবে সম্পন্ন হয়েছে।";
                }

                return "আপনার আবেদন সম্পন্ন হয় নাই";
            }
            catch (Exception e)
            {
                return "আপনার আবেদন সম্পন্ন হয় নাই";
            }

        }

        public string MaternityLeave(LeaveApplication leaveApplication, string userId)
        {
            try
            {
                var getLeave = db.MaternityLeave.FirstOrDefault(x => x.AppUserId == userId);
                int takenCount = 0;
                if (getLeave != null)
                {
                    takenCount = getLeave.TakenTime;
                }
                string max = configuration.GetSection("MyKey").GetSection("MaternityLeave").Value;
                if (takenCount >= Convert.ToInt32(max))
                {
                    return "আপনি প্রসূতি কালীন ছুটির সীমা শেষ করেছেন";
                }
                db.LeaveApplications.Add(leaveApplication);
                if (db.SaveChanges() > 0)
                {
                    return "আপনার আবেদন সফল ভাবে সম্পন্ন হয়েছে।";
                }

                return "আপনার আবেদন সম্পন্ন হয় নাই";

            }
            catch (Exception e)
            {
                return "আপনার আবেদন সম্পন্ন হয় নাই";
            }
        }
        public string NotDueLeave(LeaveApplication leaveApplication, string userId)
        {
            try
            {

                int max = Convert.ToInt32(configuration.GetSection("MyKey").GetSection("LeaveNotDue").Value);

                var leaveDetail = db.NotDueLeave
                    .FirstOrDefault(x => x.AppUserId == userId);
                var obtain = 0;

                if (leaveDetail != null)
                {
                    obtain = leaveDetail.Obtain;
                }

                var balance = max - obtain;
                var differ = leaveApplication.ToDate - leaveApplication.FromDate;
                var dateCount = differ.Days;
                if (dateCount > balance)
                {
                    return "আবেদনকৃত ছুটির পরিমান অবশিষ্ট ছুটির চেয়ে বেশি";
                }
                db.LeaveApplications.Add(leaveApplication);
                if (db.SaveChanges() > 0)
                {
                    return "আবেদন সম্পূর্ণ হয়েছে";
                }


                return "আবেদন সম্পূর্ণ হয় নাই";

            }
            catch (Exception e)
            {
                return "আবেদন সম্পূর্ণ হয় নাই";
            }

        }
        public string PlrLeave(LeaveApplication leaveApplication, string userId)
        {
            try
            {

                int max = Convert.ToInt32(configuration.GetSection("MyKey").GetSection("PlrLeave").Value);

                var differ = leaveApplication.ToDate - leaveApplication.FromDate;
                var dateCount = differ.Days;
                if (dateCount > max)
                {
                    return "আবেদনকৃত ছুটির পরিমান অবশিষ্ট ছুটির চেয়ে বেশি";
                }
                db.LeaveApplications.Add(leaveApplication);
                if (db.SaveChanges() > 0)
                {
                    return "আবেদন সম্পূর্ণ হয়েছে";
                }


                return "আবেদন সম্পূর্ণ হয় নাই";

            }
            catch (Exception e)
            {
                return "আবেদন সম্পূর্ণ হয় নাই";
            }
        }

        public string CasualLeave(LeaveApplication leave, string userId)
        {
            try
            {
                var year = leave.FromDate.Year;
                var balance = 0;
                var checkBalance = db.UserLeaveQuotas.FirstOrDefault(x =>
                    x.EmployeeId == userId && x.LeaveTypeId == leave.LeaveTypeId && x.Year.Year == year);
                if (checkBalance != null)
                {
                    balance = checkBalance.Balance;
                }
                var dateDifference = leave.ToDate - leave.FromDate;
                var dateCount = dateDifference.Days;
                if (balance < dateCount)
                {
                    return "আবেদনকৃত ছুটির পরিমান অবশিষ্ট ছুটির চেয়ে বেশি";
                }
                db.LeaveApplications.Add(leave);
                if (db.SaveChanges() > 0)
                {
                    return "আপনার আবেদন সফল ভাবে সম্পন্ন হয়েছে।";
                }

                return "আপনার আবেদন সম্পন্ন হয় নাই";

            }
            catch (Exception e)
            {
                return "আপনার আবেদন সম্পন্ন হয় নাই";
            }
        }
        public string OptionalLeave(LeaveApplication leave, string userId)
        {
            return CasualLeave(leave, userId);
        }
        public string RAndRLeave(LeaveApplication leave, string userId)
        {
            try
            {
                var date = leave.FromDate;
                var checkData = db.RestAndRecreationLeave.Where(x => x.AppUserId == userId).ToList();
                if (checkData.Any())
                {
                    var lastData = checkData.OrderByDescending(x => x.Id).FirstOrDefault();
                    if (lastData != null && date < lastData.NextAvailableDate)
                    {
                        return "আপনার তিন বছর সময় পার হয় নাই";
                    }

                    db.LeaveApplications.Add(leave);
                    if (db.SaveChanges() > 0)
                    {
                        return "আপনার আবেদন সফল ভাবে সম্পন্ন হয়েছে।";
                    }
                    return "আপনার আবেদন সম্পন্ন হয় নাই";
                }

                var user = db.Users.Find(userId);
                var availableDate = user.JoiningDate.AddYears(3);
                if (date >= availableDate)
                {
                    db.LeaveApplications.Add(leave);
                    if (db.SaveChanges() > 0)
                    {
                        return "আপনার আবেদন সফল ভাবে সম্পন্ন হয়েছে।";
                    }
                    return "আপনার আবেদন সম্পন্ন হয় নাই";
                }
                return "আপনার তিন বছর সময় পার হয় নাই";


            }
            catch (Exception e)
            {
                return "আপনার আবেদন সম্পন্ন হয় নাই";
            }
        }
        public string SpecialDisabilityLeave(LeaveApplication leaveApplication, string userId)
        {
            try
            {
                var currentUserId = "";
                int max = Convert.ToInt32(configuration.GetSection("MyKey").GetSection("SpecialDisabilityLeave").Value);
                var leaveDetail = db.SpecialDisabilityLeave
                    .FirstOrDefault(x => x.AppUserId == currentUserId);
                var obtain = 0;

                if (leaveDetail != null)
                {
                    obtain = leaveDetail.Obtain;

                }

                var balance = max - obtain;
                var differ = leaveApplication.ToDate - leaveApplication.FromDate;
                var dateCount = differ.Days;
                if (dateCount > balance)
                {
                    return "আবেদনকৃত ছুটির পরিমান অবশিষ্ট ছুটির চেয়ে বেশি";
                }
                db.LeaveApplications.Add(leaveApplication);
                if (db.SaveChanges() > 0)
                {
                    return "আবেদন সম্পূর্ণ হয়েছে";
                }


                return "আবেদন সম্পূর্ণ হয় নাই";

            }
            catch (Exception e)
            {
                return "আবেদন সম্পূর্ণ হয় নাই";
            }
        }
        public string CompulsoryLeave(LeaveApplication leaveApplication, string userId)
        {
            try
            {
                db.LeaveApplications.Add(leaveApplication);
                if (db.SaveChanges() > 0)
                {
                    return "আবেদন সম্পূর্ণ হয়েছে";
                }


                return "আবেদন সম্পূর্ণ হয় নাই";

            }
            catch (Exception e)
            {
                return "আবেদন সম্পূর্ণ হয় নাই";
            }
        }

        public string WithoutPayLeave(LeaveApplication leaveApplication, string userId)
        {
            try
            {

                db.LeaveApplications.Add(leaveApplication);
                if (db.SaveChanges() > 0)
                {
                    return "আবেদন সম্পূর্ণ হয়েছে";
                }


                return "আবেদন সম্পূর্ণ হয় নাই";

            }
            catch (Exception e)
            {
                return "আবেদন সম্পূর্ণ হয় নাই";
            }
        }




        public void CalculateLeave()
        {
            try
            {
                var userList = db.Users.Where(x => x.IsActive).ToList();


                foreach (var user in userList)
                {
                    CalculateSingleUser(user);
                }
            }
            catch (Exception e)
            {

            }
        }

        public void CalculateSingleUser(AppUser user)
        {
            var leaveList = (from la in db.LeaveApplications
                             join u in db.Users on la.ApplicantId equals u.Id
                             where u.IsActive && la.IsApproved
                             select la).ToList();

            var notDueLeaveList = (from nl in db.NotDueLeave
                                   join u in db.Users on nl.AppUserId equals u.Id
                                   where u.IsActive
                                   select nl).ToList();
            var today = DateTime.Today;

            var approvedLeave = leaveList.Where(x => x.ApplicantId == user.Id).ToList();
            var distance = today - user.JoiningDate;
            var daysTillJoin = distance.Days + 1;
            var earnLeaveFullPayTaken = approvedLeave.Where(x => x.LeaveTypeId == 3 && x.EarnLeaveType == 1).Sum(c => c.TotalDays);
            var earnLeaveHalfPayTaken = approvedLeave.Where(x => x.LeaveTypeId == 3 && x.EarnLeaveType == 2).Sum(c => c.TotalDays);
            var total = approvedLeave.Where(x => x.LeaveTypeId == 3 || x.LeaveTypeId == 4 || x.LeaveTypeId == 5 || x.LeaveTypeId == 7 || x.LeaveTypeId == 11).Sum(c => c.TotalDays);
            var rrTaken = approvedLeave.Where(x => x.LeaveTypeId == 11).Sum(c => c.TotalDays);


            //var runningDayLeave = leaveList.Where(x => x.FromDate <= today && x.ToDate > today).ToList();
            //var ongoing = 0;
            //foreach (var cLeave in runningDayLeave)
            //{
            //    var ff = -(today - cLeave.FromDate.Date).TotalDays;
            //    ongoing += (today - cLeave.FromDate.Date).Days + 1;
            //}


            var backlogData = db.BacklogEntries.Where(x => x.ApplicantId == user.Id).ToList();
            var backlog = 0;
            foreach (var b in backlogData)
            {
                backlog += b.Days;
            }

           
            var totalOfficeDay = daysTillJoin - total-backlog;

            decimal totalAverageSalaryLeave = Math.Round((decimal)totalOfficeDay / 11);
            int balanceFullPay = Convert.ToInt32(totalAverageSalaryLeave - earnLeaveFullPayTaken - rrTaken);

           
            //Half average salary leave
            decimal totalHalfAverageSalaryLeaveBeforeNotDue = Math.Round((decimal)totalOfficeDay / 12);

            int finalBalanceHalfPay = Convert.ToInt32(totalHalfAverageSalaryLeaveBeforeNotDue - earnLeaveHalfPayTaken);

            var userNotDueLeave = notDueLeaveList.FirstOrDefault(x => x.AppUserId == user.Id);
            if (userNotDueLeave != null)
            {
                int nonPaidAmount = userNotDueLeave.NonPaidAmount;
                if (nonPaidAmount > 0)
                {



                    //if (totalHalfAverageSalaryLeaveBeforeNotDue <= nonPaidAmount)
                    //{
                    //    userNotDueLeave.NonPaidAmount -= Convert.ToInt32(totalHalfAverageSalaryLeaveBeforeNotDue);

                    //}
                    //else
                    //{
                    //    userNotDueLeave.NonPaidAmount = 0;
                    //    finalBalanceHalfPay -= nonPaidAmount;
                    //}
                    if (finalBalanceHalfPay <= nonPaidAmount)
                    {
                        userNotDueLeave.NonPaidAmount -= Convert.ToInt32(nonPaidAmount - finalBalanceHalfPay);
                        finalBalanceHalfPay = 0;
                    }
                    else
                    {
                        userNotDueLeave.NonPaidAmount = 0;
                        finalBalanceHalfPay -= nonPaidAmount;
                    }
                }
            }

            if (balanceFullPay >= backlog)
            {
                balanceFullPay = balanceFullPay - backlog;
            }
            else
            {
                balanceFullPay = 0;
                var cutFromHalfPay = backlog - balanceFullPay;
                finalBalanceHalfPay = finalBalanceHalfPay - cutFromHalfPay;
            }

            

            var userFullPayEarnLeave = db.EarnLeave.FirstOrDefault(x => x.AppUserId == user.Id && x.Type == 1);

            if (userFullPayEarnLeave == null)
            {
                EarnLeave earnLeave = new EarnLeave()
                {
                    AppUserId = user.Id,
                    Balance = balanceFullPay,
                    LastCalculationDate = today,
                    Obtain = 0,
                    Type = 1

                };
                db.EarnLeave.Add(earnLeave);
            }
            else
            {
                userFullPayEarnLeave.Balance = balanceFullPay;

            }


            var userHalfPayEarnLeave = db.EarnLeave.FirstOrDefault(x => x.AppUserId == user.Id && x.Type == 2);


            if (userHalfPayEarnLeave == null)
            {
                EarnLeave earnLeave = new EarnLeave()
                {
                    AppUserId = user.Id,
                    Balance = finalBalanceHalfPay,
                    LastCalculationDate = today,
                    Obtain = 0,
                    Type = 2

                };
                db.EarnLeave.Add(earnLeave);
            }
            else
            {
                userHalfPayEarnLeave.Balance = finalBalanceHalfPay;
            }


            var restLeaveList = db.RestAndRecreationLeave.Where(x => x.AppUserId == user.Id);
            if (!restLeaveList.Any())
            {

                RestAndRecreationLeave rr = new RestAndRecreationLeave()
                {
                    AppUserId = user.Id,
                    NextAvailableDate = user.JoiningDate.AddYears(3),
                    TakenDate = null
                };
                db.RestAndRecreationLeave.Add(rr);
            }


            //optional leave
            var optionLeaveTypeId = 10;
            int optionLeaveQuota = Convert.ToInt32(configuration.GetSection("MyKey").GetSection("OptionalLeave").Value);
            var optionalLeave = db.UserLeaveQuotas
                .Where(x => x.EmployeeId == user.Id && x.LeaveType.Id == optionLeaveTypeId);
            if (!optionalLeave.Any())
            {

                var userLeaveQuotas = new UserLeaveQuota()
                {
                    EmployeeId = user.Id,
                    LeaveTypeId = optionLeaveTypeId,
                    Balance = optionLeaveQuota,
                    LeaveObtain = 0,
                    Year = DateTime.Now
                };
                db.UserLeaveQuotas.Add(userLeaveQuotas);
            }




            //noimittik

            var casualLeaveTypeId = 1;
            int casualLeaveQuota = Convert.ToInt32(configuration.GetSection("MyKey").GetSection("CasualLeave").Value);
            var casualLeave = db.UserLeaveQuotas
                .Where(x => x.EmployeeId == user.Id && x.LeaveType.Id == casualLeaveTypeId);
            if (!casualLeave.Any())
            {

                var userLeaveQuotas = new UserLeaveQuota()
                {
                    EmployeeId = user.Id,
                    LeaveTypeId = casualLeaveTypeId,
                    Balance = casualLeaveQuota,
                    LeaveObtain = 0,
                    Year = DateTime.Now
                };
                db.UserLeaveQuotas.Add(userLeaveQuotas);
            }

            //plr
            if (user.BirthDate <= DateTime.Today.AddYears(-59))
            {
                var plLeaveTypeId = 9;
                int plLeaveQuota =
                    Convert.ToInt32(configuration.GetSection("MyKey").GetSection("PlLeave").Value);
                var plLeave = db.UserLeaveQuotas
                    .Where(x => x.EmployeeId == user.Id && x.LeaveType.Id == plLeaveTypeId);
                if (!plLeave.Any())
                {

                    var userLeaveQuotas = new UserLeaveQuota()
                    {
                        EmployeeId = user.Id,
                        LeaveTypeId = plLeaveTypeId,
                        Balance = plLeaveQuota,
                        LeaveObtain = 0,
                        Year = DateTime.Now
                    };
                    db.UserLeaveQuotas.Add(userLeaveQuotas);
                }
            }

            db.SaveChanges();
        }
    }
}
