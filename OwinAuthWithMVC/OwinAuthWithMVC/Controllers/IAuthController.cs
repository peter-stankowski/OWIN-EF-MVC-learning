using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

using OwinAuthWithMVC.AuthManagers;

namespace OwinAuthWithMVC.Controllers
{
    public class IAuthController : Controller
    {
        public IAuthController() { }
        public IAuthController(AuthSignInManager signInManager, AuthUserManager userManager, AuthRoleManager roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleMananger = roleManager;
        }

        private AuthSignInManager _signInManager;
        private AuthUserManager _userManager;
        private AuthRoleManager _roleMananger;

        public AuthSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<AuthSignInManager>();
            }
            set
            {
                _signInManager = value;
            }
        }
        public AuthUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<AuthUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }
        public AuthRoleManager RoleManager
        {
            get
            {
                return _roleMananger ?? HttpContext.GetOwinContext().Get<AuthRoleManager>();
            }
            set
            {
                _roleMananger = value;
            }
        }

        // helpers
        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }


    }
}