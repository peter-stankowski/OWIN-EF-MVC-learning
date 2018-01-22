using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using OwinAuthWithMVC.AuthContext;
using OwinAuthWithMVC.AuthModels;
using OwinAuthWithMVC.AuthStores;

namespace OwinAuthWithMVC.AuthManagers
{
    public class AuthUserManager : UserManager<AuthUser, string>
    {
        public AuthUserManager(IUserStore<AuthUser, string> store) : base(store) { }

        public static AuthUserManager Create(IdentityFactoryOptions<AuthUserManager> options, IOwinContext context)
        {
            var manager = new AuthUserManager(new AuthUserStore(context.Get<AuthDbContext>()));

            return manager;
        }

    }
}