using System;
using System.Security.Claims;
using System.Web.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

using OwinAuthWithMVC.AuthManagers;
using OwinAuthWithMVC.AuthContext;

[assembly: OwinStartup(typeof(OwinAuthWithMVC.Startup))]

namespace OwinAuthWithMVC
{
    public class Startup
    {
        public const string LoginPage = "/Secret/ForGuests";

        public void Configuration(IAppBuilder app)
        {
            // inject db context and auth managers
            app.CreatePerOwinContext(AuthDbContext.Create);
            app.CreatePerOwinContext<AuthUserManager>(AuthUserManager.Create);
            app.CreatePerOwinContext<AuthRoleManager>(AuthRoleManager.Create);
            app.CreatePerOwinContext<AuthSignInManager>(AuthSignInManager.Create);

            // enable cookie auth
            app.UseCookieAuthentication(
                new CookieAuthenticationOptions
                {
                    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                    LoginPath = new PathString(LoginPage)
                }
            );

        }
    }
}
