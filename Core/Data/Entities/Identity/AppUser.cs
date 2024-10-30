using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable
namespace Core.Data.Entities.Identity
{
    public class AppUser : IdentityUser<Guid>
    {
        public override string Email { get; set; } = string.Empty;
        public override string UserName { get; set; } = string.Empty;

        public AppUser()
        {
            Id = Guid.NewGuid();
        }
    }
}
