namespace ContosoUniversity.Domain.Contracts.Exceptions;

using System;
using System.Runtime.Serialization;

public class PersistenceException : Exception
{
    public PersistenceException()
    {
    }

    protected PersistenceException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public PersistenceException(string message) : base(message)
    {
    }

    public PersistenceException(string message, Exception innerException) : base(message, innerException)
    {
    }
}