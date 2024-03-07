namespace ContosoUniversity.Application.Exceptions;

using System;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException() { }

    public EntityNotFoundException(string message) : base(message) { }

    public EntityNotFoundException(string entityName, Guid id)
        : this($"Could not find {entityName} with id:{id}") { }
}
