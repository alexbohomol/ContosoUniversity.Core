namespace Departments.Api.Models;

public record CreateDepartmentRequest(
    string Name,
    decimal Budget,
    DateTime StartDate,
    Guid? AdministratorId);
