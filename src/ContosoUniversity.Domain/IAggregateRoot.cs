namespace ContosoUniversity.Domain
{
    using System;

    /// <summary>
    /// TODO: consider better naming for it.
    /// This is less about aggregating, more about entity identification.
    /// </summary>
    public interface IAggregateRoot
    {
        Guid EntityId { get; }
    }
}