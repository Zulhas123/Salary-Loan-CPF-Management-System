using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace LMS_Web.SecurityExtension
{
    public static class IdentityExtension
    {
        public static IdentityUser GetUser(this IIdentity identity, UserManager<IdentityUser> userManager)
        {
            string userId = GetUserId(identity);
            var user = userManager.Users.FirstOrDefault(c => c.Id == userId);
            return user;
        }

        public static string GetUserId(this IIdentity identity)
        {
            var userId = new HttpContextAccessor().HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId;
        }
        public static string GetUserEmail(this IIdentity identity)
        {
            var userId = new HttpContextAccessor().HttpContext.User.FindFirstValue(ClaimTypes.Name);
            return userId;
        }
    }
}
