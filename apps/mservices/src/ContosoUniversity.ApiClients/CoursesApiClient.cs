namespace ContosoUniversity.ApiClients;

using System.Net.Http.Json;

using Application.ApiClients;

internal class CoursesApiClient(HttpClient client) : ICoursesApiClient
{
    //Read-Only

    public async Task<Course> GetById(Guid externalId, CancellationToken cancellationToken)
    {
        var dto = await client.GetFromJsonAsync<CourseDto>($"api/courses/{externalId}", cancellationToken);

        return dto.ToDomain();
    }

    public async Task<Course[]> GetByDepartmentId(Guid departmentExternalId, CancellationToken cancellationToken)
    {
        var dtos = await client.GetFromJsonAsync<CourseDto[]>($"api/courses/department/{departmentExternalId}", cancellationToken);

        return dtos.Select(x => x.ToDomain()).ToArray();
    }

    public async Task<Course[]> GetAll(CancellationToken cancellationToken)
    {
        var dtos = await client.GetFromJsonAsync<CourseDto[]>("api/courses", cancellationToken);

        return dtos.Select(x => x.ToDomain()).ToArray();
    }

    public async Task<bool> Exists(Guid externalId, CancellationToken cancellationToken)
        => await GetById(externalId, cancellationToken) != null;

    public async Task<bool> ExistsCourseCode(int courseCode, CancellationToken cancellationToken)
        => await client.GetFromJsonAsync<bool>($"/api/courses/existsByCourseCode/{courseCode}", cancellationToken);

    public async Task<Dictionary<Guid, string>> GetCourseTitlesReference(Guid[] entityIds, CancellationToken cancellationToken)
    {
        var queryString = string.Join("&", entityIds.Select(x => $"entityIds={x}"));

        return await client.GetFromJsonAsync<Dictionary<Guid, string>>($"/api/courses/title?{queryString}", cancellationToken);
    }

    //Read-Write

    public async Task Create(CourseCreateModel model, CancellationToken cancellationToken)
        => await client.PostAsJsonAsync("/api/courses", model, cancellationToken);

    public async Task Update(CourseEditModel model, CancellationToken cancellationToken)
        => await client.PutAsJsonAsync($"/api/courses/{model.Id}", model, cancellationToken);

    public async Task Delete(CourseDeleteModel model, CancellationToken cancellationToken)
        => await client.DeleteAsync($"/api/courses/{model.Id}", cancellationToken);

    public async Task<int> UpdateCoursesCredits(int multiplier, CancellationToken cancellationToken)
    {
        var responseMessage = await client.PutAsJsonAsync("/api/courses/credits/update",
            new UpdateCoursesCreditsModel(multiplier),
            cancellationToken);
        var body = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
        return int.Parse(body);
    }
}

file record CourseDto(
    int Code,
    string Title,
    int Credits,
    Guid DepartmentId,
    Guid ExternalId);

static file class Extensions
{
    public static Course ToDomain(this CourseDto dto)
        => new(dto.Code, dto.Title, dto.Credits, dto.DepartmentId, dto.ExternalId);
}

public record UpdateCoursesCreditsModel(int Multiplier);
