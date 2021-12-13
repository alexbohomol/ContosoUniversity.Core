namespace ContosoUniversity.Domain.Department;

using System;

public record DepartmentReadModel(
    string Name,
    decimal Budget,
    DateTime StartDate,
    Guid? AdministratorId,
    Guid ExternalId) : IIdentifiable<Guid>;