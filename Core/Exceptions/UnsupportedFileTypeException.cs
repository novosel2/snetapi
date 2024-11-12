using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class UnsupportedFileTypeException : Exception
    {
        public UnsupportedFileTypeException()
        {
        }

        public UnsupportedFileTypeException(string? message) : base(message)
        {
        }

        public UnsupportedFileTypeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
