namespace ContosoUniversity.Application.Contracts.Repositories.Reads.Projections;

using System;

using SharedKernel;

public record Course(
    int Code,
    string Title,
    int Credits,
    Guid DepartmentId,
    Guid ExternalId) : IIdentifiable<Guid>;
