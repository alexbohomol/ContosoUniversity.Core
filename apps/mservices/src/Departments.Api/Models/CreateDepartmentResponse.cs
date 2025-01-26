namespace Departments.Api.Models;

public record CreateDepartmentResponse(
    Guid ExternalId,
    string Name,
    decimal Budget,
    DateTime StartDate,
    Guid? AdministratorId);
