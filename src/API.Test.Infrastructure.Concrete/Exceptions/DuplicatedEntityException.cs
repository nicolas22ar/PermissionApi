using System;

namespace API.Test.Infrastructure.Concrete.Exceptions
{
    public class DuplicatedEntityException : Exception
    {
        public DuplicatedEntityException()
        {
        }

        public DuplicatedEntityException(string message) : base(message)
        {
        }

        public DuplicatedEntityException(string message, Exception innerException) : base(message, innerException)
        {
        }

       
    }
}
