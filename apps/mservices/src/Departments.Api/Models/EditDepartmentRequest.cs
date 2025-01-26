namespace Departments.Api.Models;

public record EditDepartmentRequest(
    string Name,
    decimal Budget,
    DateTime StartDate,
    Guid? AdministratorId,
    Guid ExternalId,
    byte[] RowVersion);
