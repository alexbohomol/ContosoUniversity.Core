namespace ContosoUniversity.Models
{
    using System;

    public interface IUniqueEntity
    {
        Guid UniqueId { get; set; }
    }
}