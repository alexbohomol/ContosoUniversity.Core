namespace ContosoUniversity.Domain.Contracts.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class FindException : Exception
    {
        public FindException()
        {
        }

        protected FindException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public FindException(string message) : base(message)
        {
        }

        public FindException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}