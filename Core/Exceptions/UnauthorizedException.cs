using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException()
        {
        }

        public UnauthorizedException(string? message) : base(message)
        {
        }

        public UnauthorizedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
