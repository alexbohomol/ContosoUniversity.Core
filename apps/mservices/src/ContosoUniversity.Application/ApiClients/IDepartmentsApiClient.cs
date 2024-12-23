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

    Task Create(DepartmentCreateModel model, CancellationToken cancellationToken);
    Task Update(DepartmentEditModel model, CancellationToken cancellationToken);
    Task Delete(DepartmentDeleteModel model, CancellationToken cancellationToken);

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

public record DepartmentCreateModel(
    string Name,
    decimal Budget,
    DateTime StartDate,
    Guid? AdministratorId);

public record DepartmentEditModel(
    string Name,
    decimal Budget,
    DateTime StartDate,
    Guid? AdministratorId,
    Guid ExternalId,
    byte[] RowVersion);

public record DepartmentDeleteModel(Guid Id);
