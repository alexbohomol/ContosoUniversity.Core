namespace Departments.Api.Models;

public record UpdateDepartmentResponse(
    Guid ExternalId,
    string Name,
    decimal Budget,
    DateTime StartDate,
    Guid? AdministratorId);
