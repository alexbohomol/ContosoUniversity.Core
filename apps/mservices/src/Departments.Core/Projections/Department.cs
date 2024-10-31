namespace Departments.Core.Projections;

using System;

using ContosoUniversity.Domain;

public record Department(
    string Name,
    decimal Budget,
    DateTime StartDate,
    Guid? AdministratorId,
    string AdministratorLastName,
    string AdministratorFirstName,
    Guid ExternalId) : IIdentifiable<Guid>
{
    public string AdministratorFullName => AdministratorId.HasValue
        ? $"{AdministratorLastName}, {AdministratorFirstName}"
        : string.Empty;
}
