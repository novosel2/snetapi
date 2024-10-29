namespace Core.Exceptions
{
    public class RegisterFailedException : Exception
    {
        public RegisterFailedException()
        {
        }

        public RegisterFailedException(string? message) : base(message)
        {
        }

        public RegisterFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
