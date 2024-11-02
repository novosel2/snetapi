namespace Core.Exceptions
{
    public class RoleAssignFailedException : Exception
    {
        public RoleAssignFailedException()
        {
        }

        public RoleAssignFailedException(string? message) : base(message)
        {
        }

        public RoleAssignFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
