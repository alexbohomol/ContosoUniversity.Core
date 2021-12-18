namespace ContosoUniversity.Application.Contracts.Repositories.ReadOnly.Projections;

using System;

using Domain;

public record Department(
    string Name,
    decimal Budget,
    DateTime StartDate,
    Guid? AdministratorId,
    Guid ExternalId) : IIdentifiable<Guid>;