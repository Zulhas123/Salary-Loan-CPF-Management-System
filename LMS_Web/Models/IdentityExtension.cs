using System.Security.Claims;
using System.Security.Principal;

namespace LMS_Web.Models
{
    public static class IdentityExtension
    {

        public static string UserOwnName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(ClaimTypes.Surname);
            return (claim != null) ? claim.Value : string.Empty;
        }
        public static string UserMobile(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(ClaimTypes.Name);
            return (claim != null) ? claim.Value : string.Empty;
        }

    }
}
