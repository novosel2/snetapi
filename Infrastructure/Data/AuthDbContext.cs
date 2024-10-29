using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data
{
    public class AuthDbContext : IdentityDbContext<IdentityUser>
    {
        private readonly IConfiguration _config;

        public AuthDbContext(DbContextOptions<AuthDbContext> options, IConfiguration config) : base(options) 
        { 
            _config = config;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            List<IdentityRole> roles = new List<IdentityRole>();
            List<string>? roleNames = _config.GetSection("IdentityRoles").Get<List<string>>();

            if (roleNames != null)
            {
                foreach (var roleName in roleNames)
                {
                    roles.Add(new IdentityRole
                    {
                        Name = roleName,
                        NormalizedName = roleName.ToUpper()
                    });
                }
            }

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
