using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

using OwinAuthWithMVC.AuthModels;

namespace OwinAuthWithMVC.AuthManagers
{
    public class AuthSignInManager : SignInManager<AuthUser, string>
    {
        public AuthSignInManager(AuthUserManager userManager, IAuthenticationManager authenticationManager)
                : base(userManager, authenticationManager)
        {
        }

        public static AuthSignInManager Create(IdentityFactoryOptions<AuthSignInManager> options, IOwinContext context)
        {
            return new AuthSignInManager(context.GetUserManager<AuthUserManager>(), context.Authentication);
        }
    }
}