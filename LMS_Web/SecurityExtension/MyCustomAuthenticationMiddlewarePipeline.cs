using Microsoft.AspNetCore.Builder;

namespace LMS_Web.SecurityExtension
{
    public class MyCustomAuthenticationMiddlewarePipeline
    {
       
        public void Configure(IApplicationBuilder app)
        {
            app.UseMyCustomAuthentication();
        }
    }
}
