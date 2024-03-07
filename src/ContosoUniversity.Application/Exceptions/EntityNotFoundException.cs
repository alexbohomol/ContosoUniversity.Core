namespace ContosoUniversity.Application.Exceptions;

using System;
using System.Runtime.Serialization;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException() { }

    public EntityNotFoundException(string message) : base(message) { }

    public EntityNotFoundException(string entityName, Guid id)
        : this($"Could not find {entityName} with id:{id}") { }
}
