namespace Departments.Api.Models;

internal record CreateDepartmentRequest(
    string Name,
    decimal Budget,
    DateTime StartDate,
    Guid? AdministratorId);
