namespace Core.Exceptions
{
    public class AddToRoleFailedException : Exception
    {
        public AddToRoleFailedException()
        {
        }

        public AddToRoleFailedException(string? message) : base(message)
        {
        }

        public AddToRoleFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
