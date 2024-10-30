using Core.Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Core.IServices
{
    public interface ITokenService
    {
        /// <summary>
        /// Creates a JWT for specified user
        /// </summary>
        /// <param name="user">User that requested JWT</param>
        /// <returns>Json Web Token as a string</returns>
        public string CreateToken(AppUser appUser);
    }
}
