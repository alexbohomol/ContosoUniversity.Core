namespace ContosoUniversity.Application.Contracts.Repositories.ReadOnly.Projections;

using System;

using Domain;

public record Course(
    int Code,
    string Title,
    int Credits,
    Guid DepartmentId,
    Guid ExternalId) : IIdentifiable<Guid>;