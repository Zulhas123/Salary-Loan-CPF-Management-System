using Microsoft.AspNetCore.Http;

namespace LMS_Web.SecurityExtension
{
    public class SessionData
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionData(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string LogCurrentUserEmail()
        {
            var email = _httpContextAccessor.HttpContext.User.Identity.GetUserEmail();
            return email;

        }
        public string LogCurrentUserRole()
        {
            var roleId = _httpContextAccessor.HttpContext.Session.GetString("roleId");
            return roleId;

        }
    }
}
