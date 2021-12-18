namespace ContosoUniversity.Application.Contracts.ReadModels;

using System;

using Domain;

public record DepartmentReadModel(
    string Name,
    decimal Budget,
    DateTime StartDate,
    Guid? AdministratorId,
    Guid ExternalId) : IIdentifiable<Guid>;