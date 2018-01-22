using Microsoft.AspNet.Identity.EntityFramework;


namespace OwinAuthWithMVC.AuthModels
{
   
    // extended identity 
    public class AuthUser : IdentityUser<string, AuthUserLogin, AuthUserRole, AuthUserClaim> { }
    public class AuthRole : IdentityRole<string, AuthUserRole>
    {
        public AuthRole() : base() { }
        public AuthRole(string name) : this()
        {
            this.Name = name;
        }

    }
    public class AuthUserLogin : IdentityUserLogin<string> { }
    public class AuthUserRole : IdentityUserRole<string> { }
    public class AuthUserClaim : IdentityUserClaim<string> { }
}