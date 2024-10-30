using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Entities.Identity
{
    public class AppRole : IdentityRole<Guid>
    {
        public AppRole()
        {
            Id = Guid.NewGuid();
        }
    }
}
