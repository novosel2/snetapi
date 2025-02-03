using Core.Data.Entities;
using Core.Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data
{
    public class AuthDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        private readonly IConfiguration _config;

        public AuthDbContext(DbContextOptions<AuthDbContext> options, IConfiguration config) : base(options) 
        { 
            _config = config;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasIndex(u => u.UserName);

            builder.Entity<AppUser>()
                .HasIndex(u => u.Email);

            // Static list of roles
            List<AppRole> roles = new List<AppRole>
            {
                new AppRole { Name = "admin", NormalizedName = "ADMIN" },
                new AppRole { Name = "user", NormalizedName = "USER" }
            };

            builder.Entity<AppRole>().HasData(roles);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }
    }
}
