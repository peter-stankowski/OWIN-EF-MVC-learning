using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OwinAuthWithMVC.AuthModels;
using System;
using System.Data.Entity;

namespace OwinAuthWithMVC.AuthStores 
{
    public class AuthRoleStore : RoleStore<AuthRole, string, AuthUserRole>, IQueryableRoleStore<AuthRole, string>, IRoleStore<AuthRole, string>, IDisposable
    {
        public AuthRoleStore(DbContext context) : base(context) { }
    }
}