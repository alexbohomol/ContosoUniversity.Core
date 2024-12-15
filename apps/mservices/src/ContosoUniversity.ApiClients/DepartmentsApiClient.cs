namespace ContosoUniversity.ApiClients;

using System.Net.Http.Json;

using Application.ApiClients;

internal class DepartmentsApiClient(HttpClient client) : IDepartmentsApiClient
{
    // Read-Only

    public async Task<Dictionary<Guid, string>> GetDepartmentNamesReference(CancellationToken cancellationToken)
        => await client.GetFromJsonAsync<Dictionary<Guid, string>>("/api/departments/names", cancellationToken);

    public Task<Department> GetById(Guid entityId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Department[]> GetAll(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    // Read-Write
}
