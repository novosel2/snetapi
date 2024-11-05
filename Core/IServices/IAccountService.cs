using Core.Data.Dto.AccountDto;

namespace Core.IServices
{
    public interface IAccountService
    {
        /// <summary>
        /// Registers user to the database and makes a token
        /// </summary>
        /// <param name="registerUserDto">Register information</param>
        /// <returns>User response with user information and token</returns>
        public Task<UserResponse> RegisterUserAsync(RegisterUserDto registerUserDto);

        /// <summary>
        /// Creates a token for the user and returns the whole user
        /// </summary>
        /// <param name="loginUserDto">User information</param>
        /// <returns>User response with user information and token</returns>
        public Task<UserResponse> LoginUserAsync(LoginUserDto loginUserDto);

        /// <summary>
        /// Deletes a user from the database
        /// </summary>
        public Task DeleteUserAsync();
    }
}
