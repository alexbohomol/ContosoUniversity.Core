namespace ContosoUniversity.Application.Exceptions;

using System;
using System.Runtime.Serialization;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException()
    {
    }

    protected EntityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public EntityNotFoundException(string message) : base(message)
    {
    }

    public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public EntityNotFoundException(string entityName, Guid id)
        : this($"Could not find {entityName} with id:{id}")
    {
    }
}
