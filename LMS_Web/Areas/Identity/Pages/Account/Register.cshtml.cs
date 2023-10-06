using LMS_Web.Common;
using LMS_Web.Data;
using LMS_Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS_Web.Areas.Salary.Manager;
using LMS_Web.Areas.Salary.Models;

namespace LMS_Web.Areas.Identity.Pages.Account
{
    [Authorize(Roles = "Super Admin")]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly GradeManager _gradeManager;
        private readonly StationManager _stationManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private ApplicationDbContext db;
        private string filePath;
        private readonly IConfiguration _configuration;

        public RegisterModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<RegisterModel> logger,
            ApplicationDbContext _db, IConfiguration configuration,
            IEmailSender emailSender, IWebHostEnvironment webHostEnvironment)
        {
            _gradeManager = new GradeManager(db);
            _stationManager = new StationManager(db);
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            db = _db;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            public int StationId { get; set; }
            public int GradeId { get; set; }
            public int CurrentGradeId { get; set; }
            public int ResidentialStatusId { get; set; }
            public decimal CurrentBasic { get; set; }

            //New Field End

            [Required(ErrorMessage = "মোবাইল নাম্বার দিন")]
            [Phone]
            [StringLength(11, ErrorMessage = "{0} নাম্বার অবশ্যই ১১ ডিজিট হতে হবে")]

            [Display(Name = "মোবাইল")]
            public string Phone { get; set; }


            [Required(ErrorMessage = "পাসওয়ার্ড দিন")]
            [StringLength(20, ErrorMessage = "{0} অবশ্যই ৩-২০ ডিজিট এর মধ্যে হতে হবে", MinimumLength = 3)]
            [DataType(DataType.Password)]
            [Display(Name = "পাসওয়ার্ড")]
            public string Password { get; set; }


            [DataType(DataType.Password)]
            [Display(Name = "আবার লিখুন পাসওয়ার্ড")]
            [Compare("Password", ErrorMessage = "পাসওয়ার্ড ম্যাচ করেনি")]
            public string ConfirmPassword { get; set; }



            [Display(Name = "Name")]
            [Required(ErrorMessage = "Enter Name")]
            public string FullName { get; set; }

            [Display(Name = "নাম বাংলা")]
            [Required(ErrorMessage = "নাম বাংলা দিন")]
            public string FullNameBangla { get; set; }

            [Display(Name = "বিভাগ")]
            [Required(ErrorMessage = "বিভাগ সিলেক্ট করুন")]
            public int DepartmentId { get; set; }

            [Display(Name = "পদবি")]
            [Required(ErrorMessage = "পদবি সিলেক্ট করুন")]
            public int DesignationId { get; set; }

            [Display(Name = "জন্ম তারিখ")]
            [Required(ErrorMessage = "আবশ্যিক")]
            [DataType(DataType.Date, ErrorMessage = "আবশ্যিক")]
            public DateTime BirthDate { get; set; }

            [Display(Name = "যোগদান তারিখ")]
            [Required(ErrorMessage = "আবশ্যিক")]
            [DataType(DataType.Date, ErrorMessage = "আবশ্যিক")]
            public DateTime JoiningDate { get; set; }

            [Display(Name = "লিঙ্গ")]
            public string Gender { get; set; }

            [Display(Name = "এন আই ডি")]
            public string NID { get; set; }

            [Display(Name = "ধর্ম")]
            public string Religion { get; set; }

            [Display(Name = "প্রকার")]
            public string Type { get; set; }


            [Display(Name = "ছবি")]
            public string ImagePath { get; set; }


            [NotMapped]
            [Display(Name = "ছবি")]
            public IFormFile Image { get; set; }
            [Display(Name = "উইং")]
            public int? WingId { get; set; }
            [Display(Name = "শাখা")]
            public int? SectionId { get; set; }
            [Display(Name = "পি এল আর বয়স")]
            public int PlrAge { get; set; }
            public string BankAccountNo { get; set; }
            public string BankAccountNoBangla { get; set; }
            [Display(Name = "Charge Allowance")]
            public bool IsAllowedForChargeAllowance { get; set; }
            [Display(Name = "Live Less than 3 km")]
            public bool IsLiveNear3km { get; set; }
            public string EmployeeCode { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ViewData["Grade"] = new SelectList(db.Grades, "Id", "Name");
            ViewData["Station"] = new SelectList(db.Stations, "Id", "Name");
            ViewData["ResidentialStatus"] = new SelectList(db.ResidentStatus, "Id", "Name");
            ViewData["Designation"] = new SelectList(db.Designation, "Id", "Name");
            ViewData["Department"] = new SelectList(db.Department, "Id", "Name");
            ViewData["Wing"] = new SelectList(db.Wing.Where(c => c.IsActive), "Id", "Name");
            ViewData["Error"] = new SelectList(db.Wing.Where(c => c.IsActive), "Id", "Name");
            //ViewData["Department"] = new SelectList(db.Department, "Id", "Name");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            var users = _userManager.Users;
            if (users.Any(x => x.PhoneNumber == Input.Phone))
            {
                ModelState.AddModelError("Input.Phone", "উপরোক্ত নাম্বার দিয়ে ইতিপূর্বে কর্মকর্তা তৈরী করা হয়েছে");
            }
            if (Input.BirthDate > DateTime.Now.AddYears(-18))
            {
                ViewData["Grade"] = new SelectList(db.Grades, "Id", "Name");
                ViewData["Station"] = new SelectList(db.Stations, "Id", "Name");
                ViewData["ResidentialStatus"] = new SelectList(db.ResidentStatus, "Id", "Name");
                ViewData["Designation"] = new SelectList(db.Designation, "Id", "Name");
                ViewData["Department"] = new SelectList(db.Department, "Id", "Name");
                ViewData["Wing"] = new SelectList(db.Wing.Where(c => c.IsActive), "Id", "Name");
                ModelState.AddModelError("BirthDateError", "বয়স অবশ্যই ১৮ এর বেশি হতে হবে");
            }
            if (ModelState.IsValid)
            {
                //IFormFile TempImage;

                string uniqueFileName = null;
                if (Input.Image != null)
                {
                    string uplaodsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "image/user");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + Input.Image.FileName;
                    filePath = Path.Combine(uplaodsFolder, uniqueFileName);


                    using var stream = new FileStream(filePath, FileMode.Create);
                    Input.Image.CopyTo(stream);
                }


                var user = new AppUser
                {
                    UserName = Input.Phone,
                    PhoneNumber = Input.Phone,
                    EmailConfirmed = true,
                    DepartmentId = Input.DepartmentId,
                    DesignationId = Input.DesignationId,
                    BirthDate = Input.BirthDate,
                    CreatedDateTime = DateTime.Now,
                    CreatedBy = _userManager.GetUserId(User),
                    EmployeeCode = Input.EmployeeCode,
                    EmployeeCodeBangla = string.Concat(Input.EmployeeCode.ToString().Select(c => (char)('\u09E6' + c - '0'))),
                    //EmployeeCode = GenerateEmployeeCode().Item1.ToString(),
                    //EmployeeCodeBangla = GenerateEmployeeCode().Item2,
                    IsActive = true,
                    JoiningDate = Input.JoiningDate,
                    Gender = Input.Gender,
                    NID = Input.NID,
                    Religion = Input.Religion,
                    Type = Input.Type,
                    FullName = Input.FullName,
                    FullNameBangla = Input.FullNameBangla,
                    CurrentGradeId = Input.CurrentGradeId,
                    Image = uniqueFileName,
                    SectionId = Input.SectionId,
                    WingId = Input.WingId,
                    PlrAge = Input.PlrAge,
                    StationId = Input.StationId,
                    GradeId = Input.GradeId,
                    ResidentialStatusId = Input.ResidentialStatusId,
                    CurrentBasic = Input.CurrentBasic,
                    CurrentStationJoiningDate = Input.JoiningDate,
                    BankAccountNo = Input.BankAccountNo,
                    BankAccountNoBangla = string.Concat(Input.BankAccountNo.Select(c => (char)('\u09E6' + c - '0'))),
                    IsLiveNear3Km = Input.IsLiveNear3km,
                    IsAllowedForChargeAllowance = Input.IsAllowedForChargeAllowance

                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    var userRole = new IdentityUserRole<string>()
                    {
                        UserId = user.Id,
                        RoleId = "3"
                    };

                    db.Add(userRole);
                    await db.SaveChangesAsync();

                    TempData["SuccessMessage"] = "নতুন ইউজার তৈরী হয়েছে";

                    Utility utility = new Utility(db, _configuration);
                    utility.CalculateSingleUser(user);
                    return LocalRedirect("/AppUser/Index"); ;
                }
                foreach (var error in result.Errors)
                {
                    ViewData["Grade"] = new SelectList(db.Grades, "Id", "Name");
                    ViewData["Station"] = new SelectList(db.Stations, "Id", "Name");
                    ViewData["ResidentialStatus"] = new SelectList(db.ResidentStatus, "Id", "Name");
                    ViewData["Designation"] = new SelectList(db.Designation, "Id", "Name");
                    ViewData["Department"] = new SelectList(db.Department, "Id", "Name");
                    ViewData["Wing"] = new SelectList(db.Wing.Where(c => c.IsActive), "Id", "Name");
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ViewData["Grade"] = new SelectList(db.Grades, "Id", "Name");
            ViewData["Station"] = new SelectList(db.Stations, "Id", "Name");
            ViewData["ResidentialStatus"] = new SelectList(db.ResidentStatus, "Id", "Name");
            ViewData["Designation"] = new SelectList(db.Designation, "Id", "Name");
            ViewData["Department"] = new SelectList(db.Department, "Id", "Name");
            ViewData["Wing"] = new SelectList(db.Wing.Where(c => c.IsActive), "Id", "Name");
            TempData["ErrorMessage"] = "ইউজার তৈরী হয়নি";
            return Page();
        }

        //public Tuple<int, string> GenerateEmployeeCode()
        //{
        //    int employeeCode = 0;
        //    string employeeCodeBangla = "";
        //    var lastEmployee = _userManager.Users.OrderByDescending(c => c.EmployeeCode).FirstOrDefault();
        //    if (lastEmployee != null)
        //    {
        //        employeeCode = Convert.ToInt32(lastEmployee.EmployeeCode) + 1;
        //        employeeCodeBangla = string.Concat(employeeCode.ToString().Select(c => (char)('\u09E6' + c - '0')));
        //    }

        //    return new Tuple<int, string>(employeeCode, employeeCodeBangla);
        //}
    }
}
