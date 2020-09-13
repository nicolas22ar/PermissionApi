using System;
using System.Collections.Generic;
using System.Text;

namespace API.Test.Infrastructure.Concrete.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message) : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public EntityNotFoundException()
        {
        }
    }
}
