using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OwinAuthWithMVC.AuthModels;
using System;
using System.Data.Entity;


namespace OwinAuthWithMVC.AuthStores
{
    public class AuthUserStore : UserStore<AuthUser, AuthRole, string, AuthUserLogin, AuthUserRole, AuthUserClaim>, IUserStore<AuthUser, string>, IDisposable
    {
        public AuthUserStore(DbContext context) : base(context) { }
    }
}