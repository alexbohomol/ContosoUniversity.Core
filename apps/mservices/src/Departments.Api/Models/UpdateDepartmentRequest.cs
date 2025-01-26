namespace Departments.Api.Models;

public record UpdateDepartmentRequest(
    string Name,
    decimal Budget,
    DateTime StartDate,
    Guid? AdministratorId);
