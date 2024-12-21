namespace ContosoUniversity.Application.ApiClients;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public interface IDepartmentsApiClient
{
    // Read-Only

    Task<Dictionary<Guid, string>> GetDepartmentNamesReference(CancellationToken cancellationToken);
    Task<Department> GetById(Guid externalId, CancellationToken cancellationToken);
    Task<Department[]> GetAll(CancellationToken cancellationToken);

    // Read-Write
}

public record Department(
    string Name,
    decimal Budget,
    DateTime StartDate,
    Guid? AdministratorId,
    string AdministratorLastName,
    string AdministratorFirstName,
    Guid ExternalId)
{
    public string AdministratorFullName => AdministratorId.HasValue
        ? $"{AdministratorLastName}, {AdministratorFirstName}"
        : string.Empty;
}
