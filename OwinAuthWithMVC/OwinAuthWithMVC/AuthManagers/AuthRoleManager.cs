using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using OwinAuthWithMVC.AuthContext;
using OwinAuthWithMVC.AuthModels;
using OwinAuthWithMVC.AuthStores;

namespace OwinAuthWithMVC.AuthManagers
{
    public class AuthRoleManager : RoleManager<AuthRole>
    {
        public AuthRoleManager(IRoleStore<AuthRole, string> store) : base(store) { }

        public static AuthRoleManager Create(IdentityFactoryOptions<AuthRoleManager> options, IOwinContext context)
        {
            var manager = new AuthRoleManager(new AuthRoleStore(context.Get<AuthDbContext>()));

            return manager;
        }

    }
}