namespace Departments.Core.Projections;

using System;

using ContosoUniversity.SharedKernel;

public record Department(
    string Name,
    decimal Budget,
    DateTime StartDate,
    Guid? AdministratorId,
    string AdministratorLastName,
    string AdministratorFirstName,
    Guid ExternalId) : IIdentifiable<Guid>;
