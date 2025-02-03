using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class JsonDeserializeFailedException : Exception
    {
        public JsonDeserializeFailedException()
        {
        }

        public JsonDeserializeFailedException(string? message) : base(message)
        {
        }

        public JsonDeserializeFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
