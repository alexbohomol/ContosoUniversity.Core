namespace ContosoUniversity.Models
{
    using System;

    public interface IExternalIdentifier
    {
        Guid ExternalId { get; set; }
    }
}