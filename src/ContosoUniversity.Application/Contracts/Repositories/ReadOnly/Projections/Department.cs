namespace ContosoUniversity.Application.Contracts.Repositories.ReadOnly.Projections;

using System;

using Domain;

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