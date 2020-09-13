using System;

namespace API.Test.Domain.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public int EntityId { get; }

        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(int entityId) : base($"Entity {entityId} does not exist.")
        {
            EntityId = entityId;
        }

        public EntityNotFoundException(string message) : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}