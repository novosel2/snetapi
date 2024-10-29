using Core.Data.Dto.Account;

namespace Core.IServices
{
    public interface IAccountService
    {
        /// <summary>
        /// Registers user to the database and makes a token
        /// </summary>
        /// <param name="registerUserDto">Register information</param>
        /// <returns>User response with user information and token</returns>
        public Task<UserResponseDto> RegisterUserAsync(RegisterUserDto registerUserDto);

        /// <summary>
        /// Creates a token for the user and returns the whole user
        /// </summary>
        /// <param name="loginUserDto">User information</param>
        /// <returns>User response with user information and token</returns>
        public Task<UserResponseDto> LoginUserAsync(LoginUserDto loginUserDto);
    }
}
