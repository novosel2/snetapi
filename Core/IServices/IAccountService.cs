using Core.Data.Dto.Account;

namespace Core.IServices
{
    public interface IAccountService
    {
        /// <summary>
        /// Registers user to the database
        /// </summary>
        /// <param name="registerUserDto">Register information</param>
        public Task<UserResponseDto> RegisterUserAsync(RegisterUserDto registerUserDto);
    }
}
