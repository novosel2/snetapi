using Core.IServices;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Core.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public Guid? UserId { get; }
        public string? Role { get; }

        public CurrentUserService(IHttpContextAccessor httpContextAccessor) 
        {
            UserId = Guid.TryParse(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId) ? userId : null;
            Role = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
        }
    }
}
