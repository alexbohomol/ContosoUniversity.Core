namespace ContosoUniversity.ApiClients;

using System.Net.Http.Json;

using Application.ApiClients;

internal class DepartmentsApiClient(HttpClient client) : IDepartmentsApiClient
{
    private const string ApiRoot = "api/departments";

    // Read-Only

    public async Task<Dictionary<Guid, string>> GetDepartmentNamesReference(CancellationToken cancellationToken)
        => await client.GetFromJsonAsync<Dictionary<Guid, string>>($"{ApiRoot}/names", cancellationToken);

    public async Task<Department> GetById(Guid externalId, CancellationToken cancellationToken)
    {
        var dto = await client.GetFromJsonAsync<DepartmentDto>($"{ApiRoot}/{externalId}", cancellationToken);

        return dto.ToDomain();
    }

    public async Task<Department[]> GetAll(CancellationToken cancellationToken)
    {
        var dtos = await client.GetFromJsonAsync<DepartmentDto[]>(ApiRoot, cancellationToken);

        return dtos.Select(x => x.ToDomain()).ToArray();
    }

    // Read-Write

    public async Task Create(DepartmentCreateModel model, CancellationToken cancellationToken)
        => await client.PostAsJsonAsync(ApiRoot, model, cancellationToken);

    public async Task Update(DepartmentEditModel model, CancellationToken cancellationToken)
        => await client.PutAsJsonAsync($"{ApiRoot}/{model.ExternalId}", model, cancellationToken);

    public async Task Delete(DepartmentDeleteModel model, CancellationToken cancellationToken)
        => await client.DeleteAsync($"{ApiRoot}/{model.Id}", cancellationToken);
}

file record DepartmentDto(
    string Name,
    decimal Budget,
    DateTime StartDate,
    Guid? AdministratorId,
    string AdministratorLastName,
    string AdministratorFirstName,
    Guid ExternalId);

static file class Extensions
{
    internal static Department ToDomain(this DepartmentDto dto) => new(
        dto.Name,
        dto.Budget,
        dto.StartDate,
        dto.AdministratorId,
        dto.AdministratorLastName,
        dto.AdministratorFirstName,
        dto.ExternalId);
}
