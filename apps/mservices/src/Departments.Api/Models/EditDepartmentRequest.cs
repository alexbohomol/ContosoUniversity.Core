namespace Departments.Api.Models;

internal record EditDepartmentRequest(
    string Name,
    decimal Budget,
    DateTime StartDate,
    Guid? AdministratorId,
    Guid ExternalId,
    byte[] RowVersion);
