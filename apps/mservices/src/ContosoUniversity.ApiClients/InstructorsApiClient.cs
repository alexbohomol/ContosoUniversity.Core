namespace ContosoUniversity.ApiClients;

using System.Net.Http.Json;

using Application.ApiClients;

internal class InstructorsApiClient(HttpClient client) : IInstructorsApiClient
{
    private const string ApiRoot = "api/instructors";

    // Read-Only

    public async Task<Dictionary<Guid, string>> GetInstructorNamesReference(CancellationToken cancellationToken)
        => await client.GetFromJsonAsync<Dictionary<Guid, string>>($"{ApiRoot}/names", cancellationToken);

    public async Task<Instructor[]> GetAll(CancellationToken cancellationToken)
    {
        var dtos = await client.GetFromJsonAsync<InstructorDto[]>(ApiRoot, cancellationToken);

        return dtos.Select(x => x.ToDomain()).ToArray();
    }

    public async Task<Instructor> GetById(Guid externalId, CancellationToken cancellationToken)
    {
        var dto = await client.GetFromJsonAsync<InstructorDto>($"{ApiRoot}/{externalId}", cancellationToken);

        return dto.ToDomain();
    }

    // Read-Write
}

file record InstructorDto(
    string FirstName,
    string LastName,
    DateTime HireDate,
    Guid[] Courses,
    string Office,
    Guid ExternalId);

static file class Extensions
{
    internal static Instructor ToDomain(this InstructorDto dto) => new(
        dto.FirstName,
        dto.LastName,
        dto.HireDate,
        dto.Courses,
        dto.Office,
        dto.ExternalId);
}
