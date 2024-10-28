using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data
{
    public class AuthDbContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
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

            foreach (var role in _config.GetSection("IdentityRoles").GetChildren())
            {
                roles.Add(new IdentityRole
                {
                    Name = role.ToString(),
                    NormalizedName = role.ToString()!.ToUpper()
                });
            }

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
