namespace Departments.Api.Models;

public record EditDepartmentResponse(
    Guid ExternalId,
    string Name,
    decimal Budget,
    DateTime StartDate,
    Guid? AdministratorId);
