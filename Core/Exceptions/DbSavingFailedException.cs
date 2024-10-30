using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class DbSavingFailedException : Exception
    {
        public DbSavingFailedException()
        {
        }

        public DbSavingFailedException(string? message) : base(message)
        {
        }

        public DbSavingFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
