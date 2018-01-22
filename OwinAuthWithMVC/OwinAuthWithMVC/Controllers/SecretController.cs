using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

using OwinAuthWithMVC.AuthManagers;

namespace OwinAuthWithMVC.Controllers
{
    public class SecretController : IAuthController
    {

        // GET: Secret
        [Authorize(Roles = "Admin")]
        public ActionResult ForAdmins()
        {
            return View();
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult ForSuperAdmins()
        {
            return View();
        }

        [Authorize]
        public ActionResult ForUsers()
        {
            return View();
        }

        // login
        [AllowAnonymous]
        public ActionResult ForGuests(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ForGuests(Models.LoginViewModel model, string returnUrl)
        {
            // validate model
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // try to auth the user
            var result = base.SignInManager.PasswordSignIn(model.Email, model.Password, model.RememberMe, shouldLockout: false);
          
            // process auth result
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    throw new NotImplementedException();

                case SignInStatus.RequiresVerification:
                    throw new NotImplementedException();

                case SignInStatus.Failure:

                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }

        }

        public ActionResult Logout()
        {
            base.SignInManager.AuthenticationManager.SignOut();

            return RedirectToAction("Index", "Home");
        }
    }
}