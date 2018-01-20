# OWIN-EF-MVC-learning
ASP.NET MVC project with Entity Framework. Learning OWIN authentication.

## Packages Installed
```
Microsoft.AspNet.Identity.EntityFramework
Microsoft.Owin.Security
Microsoft.Owin.Security.Cookies
Microsoft.AspNet.Identity.Owin
Microsoft.Owin.Host.SystemWeb  -- https://stackoverflow.com/questions/20203982/owinstartup-not-firing
```

# Project Breakdown

## Auth Models (identity models)
```csharp
public class AuthUser : IdentityUser<string, AuthUserLogin, AuthUserRole, AuthUserClaim> { }
public class AuthRole : IdentityRole<string, AuthUserRole> { ... }
public class AuthUserLogin : IdentityUserLogin<string> { }
public class AuthUserRole : IdentityUserRole<string> { }
public class AuthUserClaim : IdentityUserClaim<string> { }
```

## Auth Stores

**UserStore**
```csharp

public class AuthUserStore : 
    UserStore<AuthUser, AuthRole, string, AuthUserLogin, AuthUserRole, AuthUserClaim>, 
    IUserStore<AuthUser, string>, 
    IDisposable
{
    public AuthUserStore(DbContext context) : base(context) { }
}
```
**RoleStore**
```csharp
public class AuthRoleStore : 
    RoleStore<AuthRole, string, AuthUserRole>, 
    IQueryableRoleStore<AuthRole, string>, 
    IRoleStore<AuthRole, string>, IDisposable
{
    public AuthRoleStore(DbContext context) : base(context) { }
}
```

## Auth Managers

**AuthUserManager**
```csharp
 public class AuthUserManager : UserManager<AuthUser, string>
{
    public AuthUserManager(IUserStore<AuthUser, string> store) : base(store) { }

    public static AuthUserManager Create(IdentityFactoryOptions<AuthUserManager> options, IOwinContext context)
    {
        var manager = new AuthUserManager(new AuthUserStore(context.Get<AuthDbContext>()));

        return manager;
    }
}
```

**AuthRoleManager**
```csharp
public class AuthRoleManager : RoleManager<AuthRole>
{
    public AuthRoleManager(IRoleStore<AuthRole, string> store) : base(store) { }

    public static AuthRoleManager Create(IdentityFactoryOptions<AuthRoleManager> options, IOwinContext context)
    {
        var manager = new AuthRoleManager(new AuthRoleStore(context.Get<AuthDbContext>()));

        return manager;
    }
}
```

**AuthSignInManager**
```csharp
public class AuthSignInManager : 
    SignInManager<AuthUser, string>
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
```
## AuthDbContext (using Entity Framework)

```csharp
 public class AuthDbContext : 
    IdentityDbContext<AuthUser, AuthRole, string, AuthUserLogin, AuthUserRole, AuthUserClaim>
{
    public AuthDbContext() : base("DefaultConnection") { }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        // db initializer
        Database.SetInitializer(new CreateDatabaseIfNotExists<AuthDbContext>());

        // schema
        modelBuilder.HasDefaultSchema("app"); // change default to app schema e.g. [app].[AspNetUsers]

        // base
        base.OnModelCreating(modelBuilder);

    }
    public static AuthDbContext Create()
    {
        return new AuthDbContext();
    }
}
```
## Startup.cs (App_Start) - make it all work
```csharp
public class Startup
{
    public const string LoginPage = "/Secret/ForGuests"; // login page (e.g. /Login or /Account/Login)

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
```

## IAuthController
not an interface, just an extension of the Controller class.
At the moment its purpose is to give easy access to UserManager, RoleManager, SignInManager
```csharp
public class IAuthController : Controller
{
    public IAuthController() { }
    public IAuthController(AuthSignInManager signInManager, 
                            AuthUserManager userManager, 
                            AuthRoleManager roleManager)
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
```

# Example

standard **SecretController** 
```csharp
public class SecretController : Controller
{ ... }
```

extended **SecretController**
```csharp
public class SecretController : IAuthController
{ ... }
```

**Login in/out methods**  

Login method
```
SignInStatus = base.SignInManager.PasswordSignIn(model.Email, model.Password, model.RememberMe, shouldLockout: false);
```
Logout method
```
base.SignInManager.AuthenticationManager.SignOut();
```

**A bit more info if needed. This should probably be moved to IAuthController class**  

Check if user in role
```
bool result = User.IsInRole("UserRole");
```
Is user logged in
```
bool isauth = User.Identity.IsAuthenticated.ToString();
```
If user is logged in, you can get identity name
```
string = User.Identity.Name;
```
What authentycation type is used
```
string authtype = User.Identity.AuthenticationType.ToString();
```
