using LMS_Web.Common;
using LMS_Web.Data;
using LMS_Web.Models;
using LMS_Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Web.Controllers
{

    [Authorize]
    public class LeavesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private IConfiguration configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string filePath;

        public LeavesController(ApplicationDbContext context, UserManager<AppUser> userManager,
            IConfiguration _configuration, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            configuration = _configuration;
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<IActionResult> Index()
        {
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            var userId = _userManager.GetUserId(User);

            var user = _userManager.Users
                .Include(x => x.Designation)
                .FirstOrDefault(x => x.Id == userId);

            ViewBag.Designation = user?.Designation.Name;

            var applicationDbContext = _context.LeaveApplications
                .Include(l => l.Applicant)
                .Include(l => l.LeaveType)
                .Include(c => c.NextApprovedPerson)
                .Include(c => c.AttachedFiles)
                .Where(l => l.IsApproved == false && l.IsRejected == false && l.NextApprovedPersonId == userId);


            ViewBag.Message = TempData["ReturnMessage"];
            ViewBag.DesList = _context.ApprovalDesignation.ToList();
            var model = new LeaveApplicationViewModel()
            {
                LeaveApplications = await applicationDbContext.ToListAsync()
            };
            ViewData["Wing"] = new SelectList(_context.Wing.Where(c => c.IsActive), "Id", "Name");
            //ViewData["ApplicantId"] = new SelectList(_context.Users, "Id", "FullName");

            return View(model);
        }


        public async Task<IActionResult> UserApplicationList()
        {
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            var userId = _userManager.GetUserId(User);


            var applicationDbContext = _context.LeaveApplications
                .Include(l => l.Applicant)
                .Include(l => l.LeaveType)
                .Where(x => x.ApplicantId == userId)
                .OrderByDescending(c => c.Id).ToList();


            ViewBag.Message = TempData["ReturnMessage"];

            var model = new LeaveApplicationViewModel()
            {
                LeaveApplications = applicationDbContext
            };

            ViewData["ApplicantId"] = new SelectList(_context.Users, "Id", "FullName");

            return View(model);
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leave = await _context.LeaveApplications
                .Include(l => l.Applicant)
                .Include(l => l.LeaveType)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (leave == null)
            {
                return NotFound();
            }

            return View(leave);
        }




        //For Admin
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        public IActionResult Create(string empCode, int days)
        {
            var user = _context.Users.FirstOrDefault(x => x.EmployeeCode == empCode);
            if (user == null)
            {
                ViewBag.SuccessMessage = "Employee not found. Please enter correct employee code";
                return View();
            }
            var applicantId = user.Id;
            

            var backlogEntry = new BacklogEntry
            {
                LeaveTypeId = 3,
                ApplicantId = applicantId,
                Days = days
            };
            //Leave balance calculation
            var earnLeaveFullPay = _context.EarnLeave.FirstOrDefault(x => x.AppUserId == applicantId && x.Type==1);
            if (earnLeaveFullPay != null && earnLeaveFullPay.Balance >= days)
            {
                earnLeaveFullPay.Obtain += days;
            }
            else
            {
                var cutFromHalfPay = 0;
                   var earnLeaveHalfPay= _context.EarnLeave.FirstOrDefault(x => x.AppUserId == applicantId && x.Type == 2);
                if(earnLeaveFullPay != null)
                {
                    cutFromHalfPay  = days-earnLeaveFullPay.Balance;
                    earnLeaveFullPay.Obtain += earnLeaveFullPay.Balance;
                }
                
                if (earnLeaveHalfPay != null)
                {

                    earnLeaveHalfPay.Obtain += cutFromHalfPay;
                }
            }
            //Leave balance calculation
            
            _context.Add(backlogEntry);
            var save= _context.SaveChanges();
            Utility utility = new Utility(_context, configuration);
            utility.CalculateSingleUser(user);
            if ( save> 0)
            {
                ViewBag.SuccessMessage = "Leave deducted successfully";
            }

            else
            {

                ViewBag.ErrorMessage = "Leave deducted successfully";
            }
            return View();
        }



        //For Admin
        //public IActionResult Create()
        //{
        //    ViewData["VacationTypeId"] = new SelectList(_context.LeaveType, "Id", "Name");
        //    ViewData["ApplicantId"] = new SelectList(_context.Users, "Id", "EmployeeCode");
        //    var wing = _context.Wing.ToList();
        //    ViewData["Wing"] = new SelectList(wing, "Id", "Name");
        //    ViewData["Reason"] = new SelectList(_context.LeaveReason.Where(c => c.IsActive).ToList(), "Name", "Name");
        //    ViewData["Designation"] = new SelectList(_context.Designation, "Id", "Name");
        //    ViewData["EarnLeaveType"] = new SelectList(_context.EarnLeaveTypes, "Id", "Type");
        //    ViewBag.Message = TempData["Message"];
        //    return View();
        //}

        //public IActionResult GetEmployeeId(int id)
        //{
        //    var 
        //    return Json(wing);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(LeaveApplication leave, string convertToFull)
        //{
        //    //if (!ModelState.IsValid)
        //    //{
        //    //    return View();
        //    //}
        //    Utility utility = new Utility(_context, configuration);

        //    //var nextApprovalId = _context.Users.Where(x => x.DesignationId == leave.TempApprovedPersonId);


        //    foreach (var uploadedFile in leave.AttachedFiles)
        //    {
        //        string uniqueFileName = null;
        //        if (uploadedFile.Document != null)
        //        {
        //            string uplaodsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "file/");
        //            uniqueFileName = Guid.NewGuid().ToString() + "_" + uploadedFile.Document.FileName;
        //            filePath = Path.Combine(uplaodsFolder, uniqueFileName);

        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                uploadedFile.Document.CopyTo(stream);
        //            }
        //        }
        //        uploadedFile.DocumentPath = uniqueFileName;
        //    }


        //    var returnMessage = "";
        //    var userId = _userManager.GetUserId(User);
        //    if (leave.ApplicantId == null)
        //    {
        //        leave.ApplicantId = userId;
        //    }
        //    leave.CreatedById = userId;
        //    leave.CreatedDateTime = DateTime.Now;

        //    leave.TotalDays = (leave.ToDate - leave.FromDate).Days + 1;
        //    if (leave.LeaveTypeId != 3)
        //    {
        //        leave.EarnLeaveType = 0;
        //    }

        //    switch (leave.LeaveTypeId)
        //    {
        //        case 1:
        //            returnMessage = utility.CasualLeave(leave, userId);
        //            break;
        //        case 2:
        //            returnMessage = utility.SickLeave(leave);
        //            break;
        //        case 3:
        //            returnMessage = utility.EarnLeave(leave, convertToFull);
        //            break;
        //        case 4:
        //            returnMessage = utility.ExtraOrdinaryLeave(leave, userId);
        //            break;
        //        case 5:
        //            returnMessage = utility.StudyLeave(leave, userId);
        //            break;
        //        case 6:
        //            returnMessage = utility.QuarantineLeave(leave, userId);
        //            break;
        //        case 7:
        //            var user = _userManager.GetUserAsync(User);
        //            if (user.Result.Gender == "মহিলা")
        //            {
        //                returnMessage = utility.MaternityLeave(leave, userId);

        //            }
        //            else
        //            {
        //                returnMessage = "শুধুমাত্র মহিলাদের জন্য প্রযোজ্য";
        //            }
        //            break;
        //        case 8:
        //            returnMessage = utility.NotDueLeave(leave, userId);
        //            break;
        //        case 9:
        //            returnMessage = utility.PlrLeave(leave, userId);
        //            break;
        //        case 10:
        //            returnMessage = utility.OptionalLeave(leave, userId);
        //            break;
        //        case 11:
        //            returnMessage = utility.RAndRLeave(leave, userId);
        //            break;
        //        case 12:
        //            returnMessage = utility.SpecialDisabilityLeave(leave, userId);
        //            break;
        //        case 13:
        //            returnMessage = utility.CompulsoryLeave(leave, userId);
        //            break;
        //        case 14:
        //            returnMessage = utility.WithoutPayLeave(leave, userId);
        //            break;
        //        default:
        //            returnMessage = "ছুটির ধরন নির্ধারণ করুন";
        //            break;

        //    }


        //    TempData["Message"] = returnMessage;
        //    return RedirectToAction("SaveUserApplication");

        //}


        public IActionResult SaveUserApplicationAdmin()
        {
            ViewData["VacationTypeId"] = new SelectList(_context.LeaveType, "Id", "Name");
            var wing = _context.Wing.ToList();
            ViewData["Wing"] = new SelectList(wing, "Id", "Name");
            ViewData["Reason"] = new SelectList(_context.LeaveReason.Where(c => c.IsActive).ToList(), "Name", "Name");
            ViewData["Designation"] = new SelectList(_context.Designation, "Id", "Name");
            ViewData["EarnLeaveType"] = new SelectList(_context.EarnLeaveTypes, "Id", "Type");
            ViewBag.Message = TempData["Message"];
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveUserApplicationAdmin(LeaveApplication leave, string convertToFull,string empCode)
        {

            //if (!ModelState.IsValid)
            //{
            //    return View();
            //}
            Utility utility = new Utility(_context, configuration);

            //var nextApprovalId = _context.Users.Where(x => x.DesignationId == leave.TempApprovedPersonId);

            if (leave.AttachedFiles != null && leave.AttachedFiles.Any())
            {
                foreach (var uploadedFile in leave.AttachedFiles)
                {
                    string uniqueFileName = null;
                    if (uploadedFile.Document != null)
                    {
                        string uplaodsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "file/");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + uploadedFile.Document.FileName;
                        filePath = Path.Combine(uplaodsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            uploadedFile.Document.CopyTo(stream);
                        }
                    }
                    uploadedFile.DocumentPath = uniqueFileName;
                }
            }

            var returnMessage = "";


            var _user = _context.Users.FirstOrDefault(x=>x.EmployeeCode == empCode);

            if (_user == null)
            {
                returnMessage = "User not found with the specific Employee Code";
                return View();
            }

            var userId = _user.Id;
            leave.CreatedById = _userManager.GetUserId(User);
            leave.CreatedDateTime = DateTime.Now;
            //leave.IsApproved = true;
            leave.NextApprovedPersonId = _userManager.GetUserId(User);
            leave.ApplicantId = userId;


            leave.TotalDays = (leave.ToDate - leave.FromDate).Days + 1;
            if (leave.LeaveTypeId != 3)
            {
                leave.EarnLeaveType = 0;
            }

            switch (leave.LeaveTypeId)
            {
                case 1:
                    returnMessage = utility.CasualLeave(leave, userId);
                    break;
                case 2:
                    returnMessage = utility.SickLeave(leave);
                    break;
                case 3:
                    returnMessage = utility.EarnLeave(leave, convertToFull);
                    break;
                case 4:
                    returnMessage = utility.ExtraOrdinaryLeave(leave, userId);
                    break;
                case 5:
                    returnMessage = utility.StudyLeave(leave, userId);
                    break;
                case 6:
                    returnMessage = utility.QuarantineLeave(leave, userId);
                    break;
                case 7:
                    var user = _userManager.GetUserAsync(User);
                    if (user.Result.Gender == "মহিলা")
                    {
                        returnMessage = utility.MaternityLeave(leave, userId);

                    }
                    else
                    {
                        returnMessage = "শুধুমাত্র মহিলাদের জন্য প্রযোজ্য";
                    }
                    break;
                case 8:
                    returnMessage = utility.NotDueLeave(leave, userId);
                    break;
                case 9:
                    returnMessage = utility.PlrLeave(leave, userId);
                    break;
                case 10:
                    returnMessage = utility.OptionalLeave(leave, userId);
                    break;
                case 11:
                    returnMessage = utility.RAndRLeave(leave, userId);
                    break;
                case 12:
                    returnMessage = utility.SpecialDisabilityLeave(leave, userId);
                    break;
                case 13:
                    returnMessage = utility.CompulsoryLeave(leave, userId);
                    break;
                case 14:
                    returnMessage = utility.WithoutPayLeave(leave, userId);
                    break;
                default:
                    returnMessage = "ছুটির ধরন নির্ধারণ করুন";
                    break;

            }

            LeaveApprovalController approveController = new LeaveApprovalController(_context, _userManager,configuration);
            approveController.ApproveApplication(leave.Id,userId);

            TempData["Message"] = returnMessage;
            return RedirectToAction("SaveUserApplicationAdmin");
            }
       
        public IActionResult SaveUserApplication()
        {
            ViewData["VacationTypeId"] = new SelectList(_context.LeaveType, "Id", "Name");
            var wing = _context.Wing.ToList();
            ViewData["Wing"] = new SelectList(wing, "Id", "Name");
            ViewData["Reason"] = new SelectList(_context.LeaveReason.Where(c => c.IsActive).ToList(), "Name", "Name");
            ViewData["Designation"] = new SelectList(_context.Designation, "Id", "Name");
            ViewData["EarnLeaveType"] = new SelectList(_context.EarnLeaveTypes, "Id", "Type");
            ViewBag.Message = TempData["Message"];
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveUserApplication(LeaveApplication leave, string convertToFull)
        {

            //if (!ModelState.IsValid)
            //{
            //    return View();
            //}
            Utility utility = new Utility(_context, configuration);

            //var nextApprovalId = _context.Users.Where(x => x.DesignationId == leave.TempApprovedPersonId);

            if (leave.AttachedFiles !=null && leave.AttachedFiles.Any())
            {
                foreach (var uploadedFile in leave.AttachedFiles)
                {
                    string uniqueFileName = null;
                    if (uploadedFile.Document != null)
                    {
                        string uplaodsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "file/");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + uploadedFile.Document.FileName;
                        filePath = Path.Combine(uplaodsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            uploadedFile.Document.CopyTo(stream);
                        }
                    }
                    uploadedFile.DocumentPath = uniqueFileName;
                }
            }

            var returnMessage = "";
            var userId = _userManager.GetUserId(User);
            if (leave.ApplicantId == null)
            {
                leave.ApplicantId = userId;
            }
            leave.CreatedById = userId;
            leave.CreatedDateTime = DateTime.Now;

            leave.TotalDays = (leave.ToDate - leave.FromDate).Days + 1;
            if (leave.LeaveTypeId != 3)
            {
                leave.EarnLeaveType = 0;
            }

            switch (leave.LeaveTypeId)
            {
                case 1:
                    returnMessage = utility.CasualLeave(leave, userId);
                    break;
                case 2:
                    returnMessage = utility.SickLeave(leave);
                    break;
                case 3:
                    returnMessage = utility.EarnLeave(leave, convertToFull);
                    break;
                case 4:
                    returnMessage = utility.ExtraOrdinaryLeave(leave, userId);
                    break;
                case 5:
                    returnMessage = utility.StudyLeave(leave, userId);
                    break;
                case 6:
                    returnMessage = utility.QuarantineLeave(leave, userId);
                    break;
                case 7:
                    var user = _userManager.GetUserAsync(User);
                    if (user.Result.Gender == "মহিলা")
                    {
                        returnMessage = utility.MaternityLeave(leave, userId);

                    }
                    else
                    {
                        returnMessage = "শুধুমাত্র মহিলাদের জন্য প্রযোজ্য";
                    }
                    break;
                case 8:
                    returnMessage = utility.NotDueLeave(leave, userId);
                    break;
                case 9:
                    returnMessage = utility.PlrLeave(leave, userId);
                    break;
                case 10:
                    returnMessage = utility.OptionalLeave(leave, userId);
                    break;
                case 11:
                    returnMessage = utility.RAndRLeave(leave, userId);
                    break;
                case 12:
                    returnMessage = utility.SpecialDisabilityLeave(leave, userId);
                    break;
                case 13:
                    returnMessage = utility.CompulsoryLeave(leave, userId);
                    break;
                case 14:
                    returnMessage = utility.WithoutPayLeave(leave, userId);
                    break;
                default:
                    returnMessage = "ছুটির ধরন নির্ধারণ করুন";
                    break;

            }


            TempData["Message"] = returnMessage;
            return RedirectToAction("SaveUserApplication");




        }

        private bool LeaveExists(long id)
        {
            return _context.LeaveApplications.Any(e => e.Id == id);
        }

        public IActionResult TotalLeave()
        {
            return View();
        }

        public IActionResult Balance()
        {

            ViewData["Employee"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["LeaveType"] = new SelectList(_context.LeaveType, "Id", "Name");
            return View();
        }

        public IActionResult ReadBalance(int year, int leaveType, string employee)
        {
            string message = "";
            List<LeaveBalanceVm> balanceVm = new List<LeaveBalanceVm>();
            switch (leaveType)
            {
                case 1:
                    var balance = _context.UserLeaveQuotas.Include(c => c.LeaveType).FirstOrDefault(x =>
                        x.Year.Year == year && x.LeaveTypeId == leaveType && x.EmployeeId == employee);
                    if (balance != null)
                    {
                        LeaveBalanceVm caseual = new LeaveBalanceVm()
                        {
                            Balance = balance.Balance,
                            Obtain = balance.LeaveObtain,
                            Type = balance.LeaveType.Name

                        };
                        balanceVm.Add(caseual);
                    }

                    break;
                case 2:
                    message = "প্রযোজ্য না";
                    break;
                case 3:
                    var earnLeave = _context.EarnLeave.Where(x => x.AppUserId == employee).Include(c => c.EarnLeaveType)
                        .ToList();
                    foreach (var e in earnLeave)
                    {
                        LeaveBalanceVm earn = new LeaveBalanceVm()
                        {
                            Balance = e.Balance,
                            Obtain = e.Obtain,
                            Type = e.EarnLeaveType.Type

                        };

                        balanceVm.Add(earn);
                        //var totalEarnLeave = 0;
                        //foreach (var item in balanceVm)
                        //{
                        //    totalEarnLeave += item.Obtain;
                        //}

                        //ViewBag.TotalEarnLeave = totalEarnLeave;


                    }
                    break;
                case 4:
                    message = "প্রযোজ্য না";

                    break;
                case 5:

                    int max = Convert.ToInt32(configuration.GetSection("MyKey").GetSection("StudyLeave").Value);
                    var studyLeave = _context.StudyLeave.FirstOrDefault(x => x.AppUserId == employee);
                    var balnce = max;
                    var sObtain = 0;
                    if (studyLeave != null)
                    {
                        balnce = max - studyLeave.Obtain;
                        sObtain = studyLeave.Obtain;
                    }
                    LeaveBalanceVm extra = new LeaveBalanceVm()
                    {
                        Balance = balnce,
                        Obtain = sObtain,
                        Type = "অধ্যয়ন"

                    };
                    balanceVm.Add(extra);
                    break;
                case 6:
                    message = "প্রযোজ্য না";
                    break;
                case 7:
                    var user = _context.Users.FirstOrDefault(c => c.Id == employee);
                    if (user != null && user.Gender == "মহিলা")
                    {
                        int mMax = Convert.ToInt32(configuration.GetSection("MyKey").GetSection("MaternityLeave").Value);
                        var mBalance = mMax;
                        var mObtain = 0;
                        var cMaternity = _context.MaternityLeave.FirstOrDefault(c => c.AppUserId == employee);
                        if (cMaternity != null)
                        {
                            mObtain = cMaternity.TakenTime;
                            mBalance = mMax - cMaternity.TakenTime;
                        }
                        LeaveBalanceVm maternity = new LeaveBalanceVm()
                        {
                            Balance = mBalance,
                            Obtain = mObtain,
                            Type = "প্রসূতি"

                        };
                        balanceVm.Add(maternity);
                    }
                    else
                    {
                        message = "শুধুমাত্র মহিলাদের জন্য প্রযোজ্য";
                    }
                    break;
                case 8:
                    message = "প্রযোজ্য না";
                    break;
                case 9:
                    message = "প্রযোজ্য না";
                    break;
                case 10:
                    var optional = _context.UserLeaveQuotas.Include(c => c.LeaveType).FirstOrDefault(x =>
                        x.Year.Year == year && x.LeaveTypeId == leaveType && x.EmployeeId == employee);
                    if (optional != null)
                    {
                        LeaveBalanceVm op = new LeaveBalanceVm()
                        {
                            Balance = optional.Balance,
                            Obtain = optional.LeaveObtain,
                            Type = optional.LeaveType.Name

                        };
                        balanceVm.Add(op);
                    }
                    break;
                case 11:
                    message = "প্রযোজ্য না";
                    break;
                case 12:

                    int sdMax = Convert.ToInt32(configuration.GetSection("MyKey").GetSection("SpecialDisabilityLeave").Value);
                    var sDisability = _context.StudyLeave.FirstOrDefault(x => x.AppUserId == employee);
                    var sDbalance = sdMax;
                    var sdObtain = 0;
                    if (sDisability != null)
                    {
                        sDbalance = sdMax - sDisability.Obtain;
                        sdObtain = sDisability.Obtain;
                    }
                    LeaveBalanceVm sDisablility = new LeaveBalanceVm()
                    {
                        Balance = sDbalance,
                        Obtain = sdObtain,
                        Type = "বিশেষ অক্ষমতাজনিত"

                    };
                    balanceVm.Add(sDisablility);
                    break;
                case 13:
                    message = "প্রযোজ্য না";
                    break;
                case 14:
                    message = "প্রযোজ্য না";
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;

            }

            ViewBag.Message = message;
            //var balance = _context.UserLeaveQuotas.Where(x => x.Year.Year == year).Include(c => c.Employee).Include(c => c.LeaveType).ToList();
            return PartialView("_LoadBalance", balanceVm);

        }

        public IActionResult PersonalBalance()
        {
            ViewBag.LeaveList = _context.LeaveType.ToList();
            return View();

        }

        public IActionResult LoadLeaveBalance(int leaveId)
        {
            var currentYear = DateTime.Now.Year;
            var userId = _userManager.GetUserId(User);
            // var user = _userManager.GetUserAsync(User);

            return RedirectToAction("ReadBalance", new { year = currentYear, leaveType = leaveId, employee = userId });
            //List<LeaveBalanceVm> leaveList = new List<LeaveBalanceVm>();
            ////Casual and optional
            //var getLeave = _context.UserLeaveQuotas.Where(x => x.EmployeeId == userId && x.Year.Year == currentYear)
            //    .Include(c => c.Employee).Include(c => c.LeaveType).ToList();
            //foreach (var l in getLeave)
            //{
            //    LeaveBalanceVm m = new LeaveBalanceVm()
            //    {
            //        Type = l.LeaveType.Name,
            //        Balance = l.Balance,
            //        Obtain = l.LeaveObtain
            //    };
            //    leaveList.Add(m);
            //}
            ////earn leave
            //var earnLeave = _context.EarnLeave.Include(c => c.EarnLeaveType).Where(x => x.AppUserId == userId);
            //foreach (var earn in earnLeave)
            //{

            //    LeaveBalanceVm m = new LeaveBalanceVm()
            //    {
            //        Type = earn.EarnLeaveType.Type,
            //        Balance = earn.Balance,
            //        Obtain = earn.Obtain
            //    };
            //    leaveList.Add(m);

            //}
            ////study leave
            //var studyLeave = _context.StudyLeave.FirstOrDefault(x => x.AppUserId == userId);
            //int max = Convert.ToInt32(configuration.GetSection("MyKey").GetSection("StudyLeave").Value);

            //var studyBalance = max;
            //var studyObtain = 0;
            //if (studyLeave != null)
            //{
            //    studyBalance = max - studyLeave.Obtain;
            //    studyObtain = studyLeave.Obtain;
            //}
            //LeaveBalanceVm study = new LeaveBalanceVm()
            //{
            //    Type = "অধ্যয়ন",
            //    Balance = studyBalance,
            //    Obtain = studyObtain
            //};
            //leaveList.Add(study);
            ////maternity
            //if (user.Result.Gender == "Female")
            //{
            //    var mLeave = _context.MaternityLeave.FirstOrDefault(x => x.AppUserId == userId);
            //    int mMaternity = Convert.ToInt32(configuration.GetSection("MyKey").GetSection("MaternityLeave").Value);

            //    var mBalance = mMaternity;
            //    var mobtain = 0;
            //    if (mLeave != null)
            //    {
            //        mBalance = mMaternity - mLeave.TakenTime;
            //        mobtain = mLeave.TakenTime;
            //    }
            //    LeaveBalanceVm maternity = new LeaveBalanceVm()
            //    {
            //        Type = "প্রসূতি",
            //        Balance = mBalance,
            //        Obtain = mobtain
            //    };
            //    leaveList.Add(maternity);
            //}
            ////Special Disability
            //int sdMax = Convert.ToInt32(configuration.GetSection("MyKey").GetSection("SpecialDisabilityLeave").Value);
            //var sDisability = _context.StudyLeave.FirstOrDefault(x => x.AppUserId == userId);
            //var sDbalance = sdMax;
            //var sdObtain = 0;
            //if (sDisability != null)
            //{
            //    sDbalance = sdMax - sDisability.Obtain;
            //    sdObtain = sDisability.Obtain;
            //}
            //LeaveBalanceVm sDisablility = new LeaveBalanceVm()
            //{
            //    Balance = sDbalance,
            //    Obtain = sdObtain,
            //    Type = "বিশেষ অক্ষমতাজনিত"

            //};
            //leaveList.Add(sDisablility);

            //// Rest and recreation
            //var restRecreation = _context.RestAndRecreationLeave.Where(x => x.AppUserId == userId)
            //    .OrderByDescending(c => c.Id).FirstOrDefault();
            //if (restRecreation != null)
            //{
            //    var currentDate = DateTime.Today;
            //    if (currentDate >= restRecreation.NextAvailableDate && restRecreation.TakenDate == null)
            //    {

            //        LeaveBalanceVm rr = new LeaveBalanceVm()
            //        {
            //            Balance = 15,
            //            Obtain = 0,
            //            Type = "শ্রান্তি ও বিনোদন"

            //        };
            //        leaveList.Add(rr);
            //    }
            //}
            //return View(leaveList);
            //return PartialView("_LoadLeaveBalance");
        }
        public IActionResult ApplicationHistory()
        {
            var userId = _userManager.GetUserId(User);
            var leaveList = _context.LeaveApplications.Include(c => c.LeaveType).Where(x => x.ApplicantId == userId)
                .ToList();

            return View(leaveList.OrderByDescending(c => c.Id));
        }


        public IActionResult CalculateLeave()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CalculateLeaveBalance()
        {
            Utility utility = new Utility(_context, configuration);
            utility.CalculateLeave();
            return RedirectToAction("CalculateLeave");
        }
        public IActionResult LoadDepartmentByWingId(int wingId)
        {
            var wing = _context.Department.Where(c => c.IsActive && c.WingId == wingId).ToList();
            return Json(wing);
        }
        public IActionResult LoadSectionByDepartmentId(int departmentId)
        {
            var section = _context.Section.Where(c => c.IsActive && c.DepartmentId == departmentId).ToList();
            return Json(section);
        }
        public IActionResult LoadUserBySectionId(int sectionId)
        {
            var user = _context.Users.Where(c => c.IsActive && c.SectionId == sectionId).ToList();
            return Json(user);
        }
    }
}