using LMS_Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System;
using LMS_Web.Data;

namespace LMS_Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly ApplicationDbContext  _context;

        public LoginModel(SignInManager<AppUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<AppUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context= context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Display(Name = "মোবাইল")]
            [Required(ErrorMessage = "মোবাইল নাম্বার দিন")]
            [StringLength(11)]
            [Phone]
            public string Phone { get; set; }


            [Display(Name = "পাসওয়ার্ড")]
            [Required(ErrorMessage = "পাসওয়ার্ড দিন")]
            [DataType(DataType.Password)]
            public string Password { get; set; }


            [Display(Name = "মনে রাখুন")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {

                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == Input.Phone);

                if (user == null)
                {
                    ModelState.AddModelError("NoUserError", "এই ফোন নাম্বার দিয়ে কোনো ইউজার পাওয়া যায়নি");
                    return Page();
                }
                if(!user.IsActive)
                {
                    ModelState.AddModelError("NoUserError", "এই ফোন নাম্বার দিয়ে কোনো ইউজার পাওয়া যায়নি");

                }
                var result = await _signInManager.PasswordSignInAsync(user, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    UserSignInHistory userSignInHistory = new UserSignInHistory()
                    {
                        LoginDateTime = DateTime.Now,
                        UserId = user.Id
                    };
                    _context.UserSignInHistory.Add(userSignInHistory);
                    _context.SaveChanges();
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                //if (result.RequiresTwoFactor)
                //{
                //    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                //}
                //if (result.IsLockedOut)
                //{
                //    _logger.LogWarning("User account locked out.");
                //    return RedirectToPage("./Lockout");
                //}
                else
                {
                    ModelState.AddModelError("PasswordError", "সঠিক পাসওয়ার্ড দিন");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
