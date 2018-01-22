using Microsoft.AspNet.Identity.EntityFramework;
using OwinAuthWithMVC.AuthModels;
using System.Data.Entity;

namespace OwinAuthWithMVC.AuthContext
{
    public class AuthDbContext : IdentityDbContext<AuthUser, AuthRole, string, AuthUserLogin, AuthUserRole, AuthUserClaim>
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
}