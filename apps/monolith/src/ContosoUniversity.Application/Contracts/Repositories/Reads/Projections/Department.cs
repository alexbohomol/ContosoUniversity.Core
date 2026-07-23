namespace ContosoUniversity.Application.Contracts.Repositories.Reads.Projections;

using System;

using SharedKernel;

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
