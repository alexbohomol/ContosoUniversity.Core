namespace ContosoUniversity.Domain
{
    using System;

    public interface IAggregateRoot
    {
        Guid EntityId { get; }
    }
}