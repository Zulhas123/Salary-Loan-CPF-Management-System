using System.Linq;
using System.Threading.Tasks;
using LMS_Web.Data;
using LMS_Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;

namespace LMS_Web.SecurityExtension
{
    public class CustomAuthentication : AuthorizeAttribute
    {
        private readonly RequestDelegate next;
        //private IConfiguration configuration;
        //IHttpContextAccessor httpContextAccessor;

        public CustomAuthentication(RequestDelegate next)
        {
            this.next = next;


        }

        public async Task Invoke(HttpContext context, IHttpContextAccessor httpContextAccessor, ApplicationDbContext db, UserManager<AppUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            var controllerName = context.GetRouteValue("controller").ToString();
            var actionName = context.GetRouteValue("action").ToString();
            var session = new SessionData(httpContextAccessor);
            var userEmail = session.LogCurrentUserEmail();
            if (userEmail == null)
            {
                context.Response.Redirect("/Identity/Account/Login");
            }
            else
            {
                var userData = _userManager.FindByNameAsync(userEmail);
                var roles = _userManager.GetRolesAsync(userData.Result);
                var roleName = roles.Result.FirstOrDefault();
                var role = _roleManager.FindByNameAsync(roleName);
                var roleId = role.Result.Id;
                var hasAccess = (from m in db.RoleSubMenus
                    join s in db.SubMenus on m.SubMenuId equals s.Id
                    where m.RoleId == roleId && s.ControllerName == controllerName && s.ActionName == actionName
                    select s).FirstOrDefault();

                if (hasAccess != null)
                {
                    await this.next.Invoke(context);
                }
                else
                {
                    context.Response.Redirect("/Identity/Account/Login");
                }
                //await this.next.Invoke(context);

            }
            
        }
    }
    public static class MyCustomAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyCustomAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomAuthentication>();
        }
    }
}