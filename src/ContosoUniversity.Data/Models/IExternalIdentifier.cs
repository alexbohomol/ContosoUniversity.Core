namespace ContosoUniversity.Data.Models
{
    using System;

    public interface IExternalIdentifier
    {
        Guid ExternalId { get; set; }
    }
}