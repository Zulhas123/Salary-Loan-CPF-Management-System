using AspNetCore.Reporting;
using LMS_Web.Data;
using LMS_Web.Models;
using LMS_Web.SecurityExtension;
using LMS_Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LMS_Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IWebHostEnvironment webHostEnvironment;
        private IConfiguration configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration _configuration, IWebHostEnvironment _environment, ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            webHostEnvironment = _environment;
            configuration = _configuration;
            _roleManager = roleManager;
        }



        public IActionResult PrintApplication(long? id)
        {
            var applications = _context.LeaveApplications
                .Include(x => x.Applicant)
                .Include(x => x.LeaveType)
                .Where(x => x.Id == id);
            var firstApplication = applications.FirstOrDefault();
            if (firstApplication != null)
            {
                firstApplication.IsRead = true;
                _context.Update(firstApplication);
                _context.SaveChanges();
            };

            var employee = _context.Users.Include(c => c.Designation).FirstOrDefault(x => x.Id == firstApplication.ApplicantId);
            var approveBy = _context.ApprovedHistory.Include(x => x.CreatedBy).ThenInclude(x => x.Designation)
                .FirstOrDefault(c => c.LeaveApplicationId == id);
            ViewBag.Designation = approveBy?.CreatedBy.FullName + "(" + approveBy?.CreatedBy.Designation.Name + ")";
            ViewBag.EmpCode = employee?.EmployeeCode;
            ViewBag.Des = employee?.Designation.Name;
            ViewBag.Name = employee?.FullName;
            ViewBag.BirthDate = employee?.BirthDate.ToString("dd-MMM-yyyy");
            ViewBag.JoiningDate = employee?.JoiningDate.ToString("dd-MMM-yyyy");
            ViewBag.PlrDate = employee?.BirthDate.AddYears(employee.PlrAge).ToString("dd-MMM-yyyy");
            return View(applications);
        }

        public IActionResult PrintFullReport(string userId, int? leaveId)
        {
            List<LeaveApplication> leaves = new List<LeaveApplication>();
            if (leaveId == 3)
            {
                leaves = _context.LeaveApplications
                    .Where(l => l.ApplicantId == userId && l.IsApproved && (l.LeaveTypeId == 11 || l.LeaveTypeId == leaveId)).Include(c => c.LeaveType).ToList();

            }
            else
            {
                leaves = _context.LeaveApplications.Where(l => l.ApplicantId == userId && (leaveId == null || l.LeaveTypeId == leaveId) && l.IsApproved).Include(c => c.LeaveType).ToList();
            }


            var employee = _context.Users.Include(c => c.Designation).FirstOrDefault(x => x.Id == userId);




            var orderedList = leaves.OrderByDescending(x => x.CreatedDateTime).ToList();


            var studyLeaveBalance = 0;
            var maternityLeaveBalance = 0;
            var extraOrdinaryLeave = 0;
            var specialDisabilityLeaveBalance = 0;
            var optionalLeaveBalance = 0;
            var casualLeaveBalance = 0;
            var earnLeaveFull = 0;
            var earnLeaveHalf = 0;
            var halfEarnLeaveObtained = 0;
            var fullEarnLeaveObtained = 0;




            var earnLeave = _context.EarnLeave.Where(x => x.AppUserId == userId).Include(c => c.EarnLeaveType).ToList();

            if (earnLeave.Any())
            {
                var full = earnLeave.FirstOrDefault(c => c.Type == 1);
                if (full != null)
                {
                    earnLeaveFull = full.Balance;
                    halfEarnLeaveObtained = full.Obtain;
                }

                var half = earnLeave.FirstOrDefault(c => c.Type == 2);
                if (half != null)
                {
                    earnLeaveHalf = half.Balance;
                    fullEarnLeaveObtained = half.Balance;
                }

            }

            if (leaveId != 3)
            {
                var casualLeave = _context.UserLeaveQuotas.Include(c => c.LeaveType).FirstOrDefault(x =>
                    x.Year.Year == DateTime.Now.Year && x.LeaveTypeId == 1 && x.EmployeeId == userId);
                if (casualLeave != null)
                {
                    casualLeaveBalance = casualLeave.Balance;
                }

                ViewBag.CasualObtained = casualLeave?.LeaveObtain;
                ViewBag.CasualTotal = casualLeave?.LeaveObtain + casualLeaveBalance;

                int max = Convert.ToInt32(configuration.GetSection("MyKey").GetSection("StudyLeave").Value);
                var studyLeave = _context.StudyLeave.FirstOrDefault(x => x.AppUserId == userId);


                ViewBag.TotalStudyLeave = max;
                if (studyLeave != null)
                {
                    studyLeaveBalance = max - studyLeave.Obtain;
                    ViewBag.StudyLeaveObtained = studyLeave.Obtain;
                }
                else
                {
                    studyLeaveBalance = max;
                    ViewBag.StudyLeaveObtained = 0;
                }



                var user = _context.Users.FirstOrDefault(c => c.Id == userId);
                if (user != null && user.Gender == "মহিলা")
                {
                    int mMax = Convert.ToInt32(configuration.GetSection("MyKey").GetSection("MaternityLeave").Value);
                    var mBalance = mMax;
                    var cMaternity = _context.MaternityLeave.FirstOrDefault(c => c.AppUserId == userId);
                    if (cMaternity != null)
                    {
                        mBalance = mMax - cMaternity.TakenTime;
                    }

                    maternityLeaveBalance = mBalance * 180;
                    ViewBag.MatertityObtained = cMaternity?.TakenTime * 180;
                }



                var optional = _context.UserLeaveQuotas.Include(c => c.LeaveType).FirstOrDefault(x =>
                    x.Year.Year == DateTime.Now.Year && x.LeaveTypeId == 10 && x.EmployeeId == userId);
                if (optional != null)
                {
                    optionalLeaveBalance = optional.Balance;
                    ViewBag.OptionalObtained = optional.LeaveObtain;
                    ViewBag.TotalOptional = optional.LeaveObtain + optionalLeaveBalance;
                }


                int sdMax = Convert.ToInt32(configuration.GetSection("MyKey").GetSection("SpecialDisabilityLeave").Value);
                var sDisability = _context.StudyLeave.FirstOrDefault(x => x.AppUserId == userId);
                var sDbalance = sdMax;

                if (sDisability != null)
                {
                    sDbalance = sdMax - sDisability.Obtain;
                    ViewBag.SdObtained = sDisability.Obtain;
                    ViewBag.TotalSd = sDbalance + sDisability.Obtain;
                }

                specialDisabilityLeaveBalance = sDbalance;
                ViewBag.SdObtained = 0;
                ViewBag.TotalSd = sdMax;


            }

            ViewBag.leaveId = leaveId;
            if (employee != null)
            {

                if (employee.Gender == "মহিলা")
                {
                    ViewBag.MaternityLeaveBalance = maternityLeaveBalance;
                }

                else
                {

                    ViewBag.MaternityLeaveBalance = 0;
                }

                ViewBag.TotalFullEarned = earnLeaveFull + fullEarnLeaveObtained;
                ViewBag.EarnLeaveFullBalance = earnLeaveFull;
                ViewBag.FullEarnLeaveObtained = fullEarnLeaveObtained;

                ViewBag.TotalHalfEarned = earnLeaveHalf + halfEarnLeaveObtained;
                ViewBag.EarnLeaveHalfBalance = earnLeaveHalf;
                ViewBag.HalfEarnLeaveObtained = halfEarnLeaveObtained;

                ViewBag.TotalEarnLeave = earnLeaveFull + fullEarnLeaveObtained + (earnLeaveHalf / 2) +
                                         (halfEarnLeaveObtained / 2);
                ViewBag.TotalEarnLeaveObtained = fullEarnLeaveObtained + (halfEarnLeaveObtained / 2);


                ViewBag.CasualLeaveBalance = casualLeaveBalance;
                ViewBag.StudyLeaveBalance = studyLeaveBalance;
                ViewBag.OptionalLeaveBalance = optionalLeaveBalance;
                ViewBag.ExtraOrdinaryLeave = extraOrdinaryLeave;
                ViewBag.specialDisabilityLeaveBalance = specialDisabilityLeaveBalance;

                ViewBag.EmployeeInfo = employee.EmployeeCode + ", " + employee.FullName + ", " + employee.Designation.Name;

                ViewBag.Birthdate = employee.BirthDate.ToString("dd-MMM-yyyy");
                ViewBag.JoiningDate = employee.JoiningDate.ToString("dd-MMM-yyyy");
                ViewBag.PlrDate = employee.BirthDate.AddYears(employee.PlrAge).ToString("dd-MMM-yyyy");

            }

            return View(orderedList);
        }



        //public IActionResult PrintApplication(long? id)
        //{
        //    var applications = _context.LeaveApplications
        //        .Include(x => x.Applicant)
        //        .Include(x => x.LeaveType)
        //        .Where(x => x.Id == id);
        //    var firstApplication = applications.FirstOrDefault();


        //    var employee = _context.Users.Include(c => c.Designation).FirstOrDefault(x => x.Id == firstApplication.ApplicantId);



        //    DataTable table = new DataTable();
        //    table.Columns.Add("Id", typeof(string));
        //    table.Columns.Add("ApplicantId", typeof(string));
        //    table.Columns.Add("LeaveTypeId", typeof(string));
        //    table.Columns.Add("Reason", typeof(string));
        //    table.Columns.Add("TotalDays", typeof(int));
        //    table.Columns.Add("FromDate", typeof(DateTime));
        //    table.Columns.Add("ToDate", typeof(DateTime));
        //    table.Columns.Add("LeaveType", typeof(string));


        //    var path = $"{webHostEnvironment.WebRootPath}\\Reports\\LeaveApplication.rdlc";




        //    foreach (var application in applications)
        //    {
        //        DataRow row = table.NewRow();
        //        row["Id"] = application.Id;
        //        row["ApplicantId"] = application.Applicant.FullName;
        //        row["LeaveTypeId"] = application.LeaveType.Name;
        //        row["FromDate"] = application.FromDate;
        //        row["ToDate"] = application.ToDate;
        //        row["Reason"] = application.Reason;
        //        row["TotalDays"] = application.TotalDays;
        //        table.Rows.Add(row);

        //    }




        //    string mimtype = "";
        //    var extenstion = 1;

        //    Dictionary<string, string> parameters = new Dictionary<string, string>();

        //    if (employee != null)
        //    {
        //        parameters.Add("name", employee.EmployeeCode + ", " + employee.FullName + ", " + employee.Designation.Name);
        //        parameters.Add("birthdate", employee.BirthDate.ToString());
        //        parameters.Add("joiningdate", employee.JoiningDate.ToString());
        //        parameters.Add("plrDate", employee.BirthDate.AddYears(employee.PlrAge).ToString("D"));

        //    }

        //    if (firstApplication != null)
        //    {
        //        firstApplication.IsRead = true;
        //        _context.Update(firstApplication);
        //        _context.SaveChanges();
        //    };

        //    LocalReport localReport = new LocalReport(path);
        //    localReport.AddDataSource("LeaveApplication", table);
        //    var result = localReport.Execute(RenderType.Pdf, extenstion, parameters, mimtype);
        //    return File(result.MainStream, "application/pdf");
        //}

        public IActionResult Notification()
        {
            var approvedApplications = _context.LeaveApplications
                .Include(l => l.Applicant)
                .Include(l => l.LeaveType)
                .Include(c => c.NextApprovedPerson)
                .Include(c => c.AttachedFiles)
                .Where(l => l.IsApproved == true);

            var applications = approvedApplications.OrderByDescending(x => x.Id);
            return View(applications);
        }

        public IActionResult Index()
        {
            var userId = User.Identity.GetUserEmail();

            if (userId != null)
            {
                var userData = _userManager.FindByNameAsync(userId);
                var roles = _userManager.GetRolesAsync(userData.Result);
                var roleName = roles.Result.FirstOrDefault();
                var role = _roleManager.FindByNameAsync(roleName);
                var roleId = role.Result.Id;
                var data = (from sr in _context.RoleSubMenus
                            join sub in _context.SubMenus on sr.SubMenuId equals sub.Id
                            where sr.RoleId == roleId

                            select new ModuleVm
                            {
                                Name = sub.AreaName

                            }).Distinct().ToList();

                return View(data);
            }

            return Redirect("/Account/Identity/Login");
        }

        [Authorize(Roles = "User, Admin, Super Admin")]
        public IActionResult LMSDashboard()
        {
            var user = _userManager.GetUserId(User);

            var approvedApplications = _context.LeaveApplications
                .Include(l => l.Applicant)
                .Include(l => l.LeaveType)
                .Include(c => c.NextApprovedPerson)
                .Include(c => c.AttachedFiles)
                .Where(l => l.IsApproved == true && l.IsRead == false);

            ViewBag.ApprovedListForNotification = approvedApplications.Count();

            //UserSignInHistory userSignInHistory = new UserSignInHistory()
            //{
            //    LoginDateTime = DateTime.Now,
            //    UserId = user
            //};
            //_context.UserSignInHistory.Add(userSignInHistory);
            //_context.SaveChanges();
            var today = DateTime.Today;
            ViewBag.TotalUser = _context.Users.ToList().Count;
            ViewBag.EmployeeOnLeaveCount = _context.LeaveApplications
                .Count(l => l.IsApproved == true && today >= l.FromDate.Date && today <= l.ToDate.Date);
            if (User.IsInRole("User"))
            {
                return RedirectToAction("UserDashboard");
            }
            else
                return View();

        }


        [Authorize(Roles = "User, Admin, Super Admin")]
        public IActionResult Report()
        {
            ViewBag.LeaveList = _context.LeaveType.ToList();
            return View();
        }

        public IActionResult LeaveReport(int leaveId)
        {
            List<LeaveApplication> leaveApplications = new List<LeaveApplication>();

            var userId = _userManager.GetUserId(User);

            leaveApplications = _context.LeaveApplications
                .Where(x => x.IsApproved && x.ApplicantId == userId && x.LeaveTypeId == leaveId)
                .Include(c => c.LeaveType).ToList();

            LeaveBalance(leaveId, userId);
            ViewBag.LeaveId = leaveId;
            ViewBag.TotalFullEarnLeaveBalance = TotalFullEarnLeaveBalance;
            ViewBag.TotalHalfEarnLeaveBalance = TotalHalfEarnLeaveBalance;
            if (!isValid)
            {
                ViewBag.LeaveBalance = "প্রযোজ্য নয়";
            }
            else
            {
                ViewBag.LeaveBalance = balance;
            }
            return PartialView("_LeaveReport", leaveApplications);
        }
        bool isValid = true;
        int balance = 0;
        int TotalFullEarnLeaveBalance = 0;
        int TotalHalfEarnLeaveBalance = 0;
        public void LeaveBalance(int leaveId, string userId)
        {

            var leaveQuotas = _context.UserLeaveQuotas.Include(c => c.LeaveType).Where(x =>
                x.Year.Year == DateTime.Now.Year && x.EmployeeId == userId);
            switch (leaveId)
            {
                case 1:
                    var i = leaveQuotas.FirstOrDefault(x => x.LeaveTypeId == 1)?.Balance;
                    if (i != null)
                        balance = (int)i;
                    break;
                case 2:
                    isValid = false;
                    break;
                case 3:
                    var earnLeave = _context.EarnLeave.Where(x => x.AppUserId == userId).ToList();
                    TotalFullEarnLeaveBalance = (int)earnLeave.FirstOrDefault(c => c.Type == 1)?.Balance;
                    TotalHalfEarnLeaveBalance = (int)earnLeave.FirstOrDefault(c => c.Type == 2)?.Balance;
                    break;

                case 4:
                    isValid = false;

                    break;
                case 5:

                    int max = Convert.ToInt32(configuration.GetSection("MyKey").GetSection("StudyLeave").Value);
                    var studyLeave = _context.StudyLeave.FirstOrDefault(x => x.AppUserId == userId);
                    if (studyLeave != null) balance = max - studyLeave.Obtain;


                    break;
                case 6:
                    isValid = false;
                    break;
                //case 7:
                //    var user = _context.Users.FirstOrDefault(c => c.Id == userId);
                //    if (user != null && user.Gender == "Female")
                //    {
                //        int mMax = Convert.ToInt32(configuration.GetSection("MyKey").GetSection("MaternityLeave").Value);

                //        var cMaternity = _context.MaternityLeave.FirstOrDefault(c => c.AppUserId == userId);
                //        if (cMaternity != null)
                //        {
                //            balance = (mMax - cMaternity.TakenTime);
                //        }
                //        else
                //        {

                //        }

                //    }
                //    else
                //    {
                //        message = "শুধুমাত্র মহিলাদের জন্য প্রযোজ্য";
                //    }
                //    break;
                case 8:
                    isValid = false;
                    break;
                case 9:
                    isValid = false;
                    break;
                case 10:
                    var o = leaveQuotas.FirstOrDefault(x => x.LeaveTypeId == 10)?.Balance;
                    if (o != null)
                        balance = (int)o;
                    break;
                case 11:
                    isValid = false;
                    break;
                case 12:

                    int sdMax = Convert.ToInt32(configuration.GetSection("MyKey").GetSection("SpecialDisabilityLeave").Value);
                    var sDisability = _context.StudyLeave.FirstOrDefault(x => x.AppUserId == userId);
                    balance = (int)(sdMax - sDisability?.Obtain);

                    break;
                case 13:
                    isValid = false;
                    break;
                case 14:
                    isValid = false;
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }

        }
        public IActionResult PrintReport(int id)
        {
            var path = "";
            var userId = _userManager.GetUserId(User);
            var list = _context.LeaveApplications.Where(x => x.IsApproved && x.ApplicantId == userId && x.LeaveTypeId == id).Include(c => c.LeaveType).ToList();
            var employee = _context.Users.Include(c => c.Designation).FirstOrDefault(x => x.Id == userId);

            DataTable table = new DataTable();
            table.Columns.Add("Reason", typeof(string));
            table.Columns.Add("TotalDays", typeof(int));
            table.Columns.Add("FromDate", typeof(DateTime));
            table.Columns.Add("ToDate", typeof(DateTime));
            if (id == 3)
            {
                path = $"{webHostEnvironment.WebRootPath}\\Reports\\EarnLeaveReport.rdlc";
                table.Columns.Add("EarnLeaveId", typeof(int));
                table.Columns.Add("EarnLeaveType", typeof(string));
                table.Columns.Add("Balance", typeof(string));
            }
            else
            {
                path = $"{webHostEnvironment.WebRootPath}\\Reports\\LeaveReport.rdlc";
            }
            LeaveBalance(id, userId);

            var type = "";
            var orderedList = list.OrderByDescending(x => x.CreatedDateTime).ToList();
            foreach (var str in orderedList)
            {

                type = str.LeaveType.Name;
                DataRow row = table.NewRow();
                row["Reason"] = str.Reason;
                row["TotalDays"] = str.TotalDays;
                row["FromDate"] = str.FromDate;
                row["ToDate"] = str.ToDate;
                if (id == 3)
                {

                    row["EarnLeaveId"] = str.EarnLeaveType;

                    if (str.EarnLeaveType == 1)
                    {
                        row["EarnLeaveType"] = "গড় বেতনে";
                        row["Balance"] = TotalFullEarnLeaveBalance;
                    }
                    else if (str.EarnLeaveType == 2)
                    {
                        row["EarnLeaveType"] = "অর্ধ গড় বেতনে";
                        row["Balance"] = TotalHalfEarnLeaveBalance;
                    }

                }

                table.Rows.Add(row);
            }

            string mimtype = "";
            var extenstion = 1;

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            if (employee != null)
            {
                parameters.Add("name", employee.EmployeeCode + ". " + employee.FullName + ", " + employee.Designation.Name);
                parameters.Add("leaveType", type);
                parameters.Add("birthdate", employee.BirthDate.ToString());
                parameters.Add("joiningdate", employee.JoiningDate.ToString());
                parameters.Add("plrDate", employee.BirthDate.AddYears(employee.PlrAge).ToString("D"));

            }

            LocalReport localReport = new LocalReport(path);
            if (id == 3)
            {
                localReport.AddDataSource("EarnLeaveType", table);
            }
            else
            {
                parameters.Add("balance", balance.ToString());
                localReport.AddDataSource("leaveList", table);
            }

            var result = localReport.Execute(RenderType.Pdf, extenstion, parameters, mimtype);
            return File(result.MainStream, "application/pdf");

        }
        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }


        [Authorize]
        public IActionResult UserLeaveHistory(string id)
        {
            ViewData["LeaveType"] = new SelectList(_context.LeaveType, "Id", "Name");
            ViewBag.Wing = _context.Wing.Where(c => c.IsActive).ToList();
            ViewData["Designation"] = new SelectList(_context.Designation, "Id", "Name");


            //var leaves = _context.LeaveApplications
            //    .Where(l => l.ApplicantId == id && l.IsApproved && l.FromDate.Year == DateTime.Now.Year && l.ToDate.Year == DateTime.Now.Year).ToList();
            var user = _context.Users.FirstOrDefault(c => c.Id == id);
            if (user != null)
            {
                ViewBag.UserId = id;
                ViewBag.WingId = user.WingId;
                ViewBag.DepartmentId = user.DepartmentId;
                ViewBag.SectionId = user.SectionId;

            }

            ViewBag.Message = "";
            return View();
        }




        public IActionResult UserLeaveDetailsHistory(string userId, int? leaveTypeId)
        {
            ViewData["Wing"] = new SelectList(_context.Wing.Where(c => c.IsActive), "Id", "Name");
            ViewData["Designation"] = new SelectList(_context.Designation, "Id", "Name");
            ViewData["LeaveType"] = new SelectList(_context.LeaveType, "Id", "Name");
            ViewBag.UserId = userId;
            ViewBag.LeaveId = leaveTypeId;
            List<LeaveApplication> leaves = new List<LeaveApplication>();

            if (leaveTypeId == 3)
            {
                leaves = _context.LeaveApplications
                    .Where(l => l.ApplicantId == userId && l.IsApproved && (l.LeaveTypeId == 11 || l.LeaveTypeId == leaveTypeId)).Include(c => c.LeaveType).ToList();
            }
            else
            {
                leaves = _context.LeaveApplications
                    .Where(l => l.ApplicantId == userId && l.IsApproved && (leaveTypeId == null || l.LeaveTypeId == leaveTypeId)).Include(c => c.LeaveType).ToList();

            }
            if (leaves.Any())
            {
                ViewBag.Message = "কোন তথ্য পাওয়া যায় নাই";
            }
            return PartialView("_UserLeaveDetailsHistory", leaves);

        }

        //public IActionResult LeaveApplicationReport()
        //{

        //}


        //public IActionResult PrintFullReport(string userId, int? leaveId)
        //{
        //    List<LeaveApplication> leaves = new List<LeaveApplication>();
        //    if (leaveId == 3)
        //    {
        //        leaves = _context.LeaveApplications
        //            .Where(l => l.ApplicantId == userId && l.IsApproved && (l.LeaveTypeId == 11 || l.LeaveTypeId == leaveId)).Include(c => c.LeaveType).ToList();

        //    }
        //    else
        //    {
        //        leaves = _context.LeaveApplications.Where(l => l.ApplicantId == userId && (leaveId == null || l.LeaveTypeId == leaveId) && l.IsApproved).Include(c => c.LeaveType).ToList();
        //    }


        //    var employee = _context.Users.Include(c => c.Designation).FirstOrDefault(x => x.Id == userId);

        //    DataTable table = new DataTable();
        //    table.Columns.Add("Reason", typeof(string));
        //    table.Columns.Add("TotalDays", typeof(int));
        //    table.Columns.Add("FromDate", typeof(DateTime));
        //    table.Columns.Add("ToDate", typeof(DateTime));
        //    table.Columns.Add("LeaveType", typeof(string));


        //    var path = $"{webHostEnvironment.WebRootPath}\\Reports\\FullReport.rdlc";
        //    var pathEarnLeave = $"{webHostEnvironment.WebRootPath}\\Reports\\FullEarnLeaveReport.rdlc";
        //    var type = "";
        //    var orderedList = leaves.OrderByDescending(x => x.CreatedDateTime).ToList();
        //    foreach (var str in orderedList)
        //    {

        //        type = str.LeaveType.Name;
        //        DataRow row = table.NewRow();
        //        row["Reason"] = str.Reason;
        //        row["TotalDays"] = str.TotalDays;
        //        row["FromDate"] = str.FromDate;
        //        row["ToDate"] = str.ToDate;
        //        row["LeaveType"] = type;

        //        table.Rows.Add(row);
        //    }

        //    string mimtype = "";
        //    var extenstion = 1;

        //    var studyLeaveBalance = 0;
        //    var maternityLeaveBalance = 0;
        //    var extraOrdinaryLeave = 0;
        //    var specialDisabilityLeaveBalance = 0;
        //    var optionalLeaveBalance = 0;
        //    var casualLeaveBalance = 0;
        //    var earnLeaveFull = 0;
        //    var earnLeaveHalf = 0;



        //    var casualLeave = _context.UserLeaveQuotas.Include(c => c.LeaveType).FirstOrDefault(x =>
        //                 x.Year.Year == DateTime.Now.Year && x.LeaveTypeId == 1 && x.EmployeeId == userId);
        //    if (casualLeave != null)
        //    {
        //        casualLeaveBalance = casualLeave.Balance;
        //    }


        //    var earnLeave = _context.EarnLeave.Where(x => x.AppUserId == userId).Include(c => c.EarnLeaveType).ToList();

        //    if (earnLeave.Any())
        //    {
        //        var full = earnLeave.FirstOrDefault(c => c.Type == 1);
        //        if (full != null)
        //        {
        //            earnLeaveFull = full.Balance;
        //        }

        //        var half = earnLeave.FirstOrDefault(c => c.Type == 2);
        //        if (half != null)
        //        {
        //            earnLeaveHalf = half.Balance;
        //        }

        //    }


        //    int max = Convert.ToInt32(configuration.GetSection("MyKey").GetSection("StudyLeave").Value);
        //    var studyLeave = _context.StudyLeave.FirstOrDefault(x => x.AppUserId == userId);

        //    if (studyLeave != null)
        //    {
        //        studyLeaveBalance = max - studyLeave.Obtain;

        //    }
        //    else
        //    {
        //        studyLeaveBalance = max;
        //    }

        //    var user = _context.Users.FirstOrDefault(c => c.Id == userId);
        //    if (user != null && user.Gender == "মহিলা")
        //    {
        //        int mMax = Convert.ToInt32(configuration.GetSection("MyKey").GetSection("MaternityLeave").Value);
        //        var mBalance = mMax;
        //        var cMaternity = _context.MaternityLeave.FirstOrDefault(c => c.AppUserId == userId);
        //        if (cMaternity != null)
        //        {
        //            mBalance = mMax - cMaternity.TakenTime;
        //        }

        //        maternityLeaveBalance = mBalance * 180;
        //    }



        //    var optional = _context.UserLeaveQuotas.Include(c => c.LeaveType).FirstOrDefault(x =>
        //        x.Year.Year == DateTime.Now.Year && x.LeaveTypeId == 10 && x.EmployeeId == userId);
        //    if (optional != null)
        //    {
        //        optionalLeaveBalance = optional.Balance;
        //    }


        //    int sdMax = Convert.ToInt32(configuration.GetSection("MyKey").GetSection("SpecialDisabilityLeave").Value);
        //    var sDisability = _context.StudyLeave.FirstOrDefault(x => x.AppUserId == userId);
        //    var sDbalance = sdMax;

        //    if (sDisability != null)
        //    {
        //        sDbalance = sdMax - sDisability.Obtain;
        //    }

        //    specialDisabilityLeaveBalance = sDbalance;
        //    var allBalance = "";



        //    Dictionary<string, string> parameters = new Dictionary<string, string>();

        //    if (employee != null)
        //    {
        //        parameters.Add("plrDate", employee.BirthDate.AddYears(employee.PlrAge).ToString("D"));
        //        if (employee.Gender == "মহিলা")
        //        {
        //            parameters.Add("earnLeaveFull", earnLeaveFull.ToString());
        //            //parameters.Add("earnLeaveHalf", earnLeaveHalf.ToString() + " (গড়" + Math.Round((decimal)(earnLeaveHalf / 2)) + ")");
        //            parameters.Add("earnLeaveHalf", earnLeaveHalf.ToString());
        //            parameters.Add("casualLeaveBalance", casualLeaveBalance.ToString());
        //            parameters.Add("studyLeaveBalance", studyLeaveBalance.ToString());
        //            parameters.Add("optionalLeaveBalance", optionalLeaveBalance.ToString());
        //            parameters.Add("extraOrdinaryLeave", extraOrdinaryLeave.ToString());
        //            parameters.Add("specialDisabilityLeaveBalance", specialDisabilityLeaveBalance.ToString());

        //            parameters.Add("maternityLeaveBalance", maternityLeaveBalance.ToString());

        //            //allBalance = "পূর্ণ গড়ঃ " + earnLeaveFull + " দিন, অর্ধ গড়ঃ " + earnLeaveHalf + " (গড়" + Math.Round((decimal)(earnLeaveHalf / 2)) + ")" + " দিন, নৈমিত্তিকঃ " +
        //            //             casualLeaveBalance + " দিন, অধ্যয়নঃ " + studyLeaveBalance + " দিন, ঐচ্ছিকঃ " + optionalLeaveBalance +
        //            //             " দিন, অসাধারণঃ " + optionalLeaveBalance + " দিন, প্রসূতিঃ " + maternityLeaveBalance +
        //            //             " দিন, অক্ষমতাজনিত বিশেষঃ " + maternityLeaveBalance + " দিন";
        //        }

        //        //if (leaveId == 3)
        //        //{

        //        //}

        //        else
        //        {
        //            parameters.Add("earnLeaveFull", earnLeaveFull.ToString());
        //            parameters.Add("earnLeaveHalf", earnLeaveHalf.ToString());
        //            parameters.Add("casualLeaveBalance", casualLeaveBalance.ToString());
        //            parameters.Add("studyLeaveBalance", studyLeaveBalance.ToString());
        //            parameters.Add("optionalLeaveBalance", optionalLeaveBalance.ToString());
        //            parameters.Add("extraOrdinaryLeave", extraOrdinaryLeave.ToString());
        //            parameters.Add("specialDisabilityLeaveBalance", specialDisabilityLeaveBalance.ToString());

        //            parameters.Add("maternityLeaveBalance", "প্রযোজ্য নয়");
        //        }
        //        parameters.Add("name", employee.EmployeeCode + ", " + employee.FullName + ", " + employee.Designation.Name);

        //        parameters.Add("birthdate", employee.BirthDate.ToString());
        //        parameters.Add("joiningdate", employee.JoiningDate.ToString());
        //        //parameters.Add("allBalance", allBalance);
        //        //var prlDate = employee.BirthDate.Year+ employee.PlrAge;
        //        //parameters.Add("plrDate", new DateTime(prlDate,employee.BirthDate.Month,employee.BirthDate.Day).ToString());

        //    }

        //    if (leaveId == 3)
        //    {
        //        LocalReport localReport = new LocalReport(pathEarnLeave);
        //        localReport.AddDataSource("FullReport", table);
        //        var result = localReport.Execute(RenderType.Pdf, extenstion, parameters, mimtype);
        //        return File(result.MainStream, "application/pdf");
        //    }
        //    else
        //    {
        //        LocalReport localReport = new LocalReport(path);
        //        localReport.AddDataSource("FullReport", table);
        //        var result = localReport.Execute(RenderType.Pdf, extenstion, parameters, mimtype);
        //        return File(result.MainStream, "application/pdf");
        //    }
        //}

        [Authorize]
        public IActionResult UserDashboard()
        {
            var userId = _userManager.GetUserId(User);
            var leaves = _context.LeaveApplications
                .Where(l => l.ApplicantId == userId && l.IsApproved && l.FromDate.Year == DateTime.Now.Year && l.ToDate.Year == DateTime.Now.Year).ToList();


            var totalLeaveCount = 0;

            foreach (var leave in leaves)
            {
                if (leave.FromDate.Year != DateTime.Now.Year)
                {
                    var daysCount = leave.ToDate.Day;

                    totalLeaveCount += daysCount;
                }

                if (leave.FromDate.Year == DateTime.Now.Year)
                {
                    var days = leave.ToDate - leave.FromDate;
                    var daysCount = days.Days;
                    totalLeaveCount += daysCount;
                }
            }



            var leaveQuotas = _context.UserLeaveQuotas
                .Where(l => l.EmployeeId == userId && l.Year.Year == DateTime.Now.Year)
                .Include(l => l.LeaveType);


            ViewBag.EarnLeave = _context.EarnLeave.FirstOrDefault(x => x.AppUserId == userId && x.Type == 1)?.Balance;
            ViewBag.EarnHalfLeave = _context.EarnLeave.FirstOrDefault(x => x.AppUserId == userId && x.Type == 2)?.Balance;
            ViewBag.MaternityLeave = _context.MaternityLeave.FirstOrDefault(x => x.AppUserId == userId)?.TakenTime;
            ViewBag.NotDueLeave = _context.NotDueLeave.FirstOrDefault(x => x.AppUserId == userId)?.Obtain;
            ViewBag.RestAndRecreationLeave = _context.RestAndRecreationLeave
                .Where(x => x.AppUserId == userId)
                .OrderByDescending(x => x.Id)
                .FirstOrDefault()?.NextAvailableDate;
            ViewBag.SpecialDisabilityLeave = _context.SpecialDisabilityLeave.FirstOrDefault(x => x.AppUserId == userId)?.Obtain;
            ViewBag.StudyLeave = _context.StudyLeave.FirstOrDefault(x => x.AppUserId == userId)?.Obtain;

            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);

            if (user?.Gender == "মহিলা")
            {
                //var paternityLeave = _context.LeaveApplications
                //    .Where(x => x.LeaveTypeId == 7);
                var maternityLeave = _context.MaternityLeave.FirstOrDefault(X => X.AppUserId == userId);
                ViewBag.MaternityLeave = maternityLeave?.TakenTime * 180;


            }

            var model = new UserDashboardDataViewModel
            {
                TotalLeaveCount = totalLeaveCount,
                LeaveQuotas = leaveQuotas.ToList()
            };

            return View(model);
        }


        //[Authorize]
        //[HttpPost]
        //public IActionResult UserDashboard(DateTime fromDate, DateTime toDate)
        //{
        //    var userId = _userManager.GetUserId(User);

        //    var leaves = _context.LeaveApplications
        //        .Where(l => l.ApplicantId == userId && l.FromDate.Year == DateTime.Now.Year && l.ToDate.Year == DateTime.Now.Year)
        //        .Include(l => l.Applicant);

        //    return View();
        //}


        public IActionResult Test()
        {
            return View();
        }

        public IActionResult GetUserImage(string phoneNumber)
        {
            var userImage = _context.Users.FirstOrDefault(x => x.PhoneNumber == phoneNumber)?.Image;
            return Json(userImage);
        }


    }
}
