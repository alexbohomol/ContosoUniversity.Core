namespace ContosoUniversity.ApiClients;

using System.Net.Http.Json;

using Application.ApiClients;

using SharedKernel.Paging;

internal class StudentsApiClient(HttpClient client) : IStudentsApiClient
{
    private const string ApiRoot = "api/students";

    public async Task<Student[]> GetStudentsEnrolledForCourses(Guid[] courseIds, CancellationToken cancellationToken)
    {
        var queryString = string.Join("&", courseIds.Select(x => $"courseIds={x}"));

        var dtos = await client.GetFromJsonAsync<StudentDto[]>($"{ApiRoot}/enrolled?{queryString}", cancellationToken);

        return dtos.Select(x => x.ToDomain()).ToArray();
    }

    public async Task<EnrollmentDateGroup[]> GetEnrollmentDateGroups(CancellationToken cancellationToken)
    {
        var dtos = await client.GetFromJsonAsync<EnrollmentDateGroupDto[]>($"{ApiRoot}/enrolled/groups", cancellationToken);

        return dtos.Select(x => x.ToDomain()).ToArray();
    }

    public async Task<PagedResult<Student>> Search(
        SearchRequest searchRequest,
        OrderRequest orderRequest,
        PageRequest pageRequest,
        CancellationToken cancellationToken)
    {
        var message = await client.PostAsJsonAsync($"{ApiRoot}/search", new
        {
            searchRequest,
            orderRequest,
            pageRequest
        }, cancellationToken);

        (StudentDto[] dtos, PageInfo info) = await message.Content.ReadFromJsonAsync<PagedResult<StudentDto>>(cancellationToken);

        return new(
            dtos.Select(x => x.ToDomain()).ToArray(),
            info);
    }

    public async Task<Student> GetById(Guid externalId, CancellationToken cancellationToken)
    {
        var dto = await client.GetFromJsonAsync<StudentDto>($"{ApiRoot}/{externalId}", cancellationToken);

        return dto.ToDomain();
    }
}

file enum GradeDto
{
    A,
    B,
    C,
    D,
    F
}

file record EnrollmentDto(
    Guid StudentId,
    Guid CourseId,
    GradeDto? Grade);

file record StudentDto(
    string LastName,
    string FirstName,
    DateTime EnrollmentDate,
    EnrollmentDto[] Enrollments,
    Guid ExternalId);

file record EnrollmentDateGroupDto(DateTime EnrollmentDate, int StudentCount);

static file class Extensions
{
    internal static Student ToDomain(this StudentDto dto) => new(
        dto.LastName,
        dto.FirstName,
        dto.EnrollmentDate,
        dto.ExternalId)
    {
        Enrollments = dto.Enrollments.Select(x => x.ToDomain()).ToArray()
    };

    private static Enrollment ToDomain(this EnrollmentDto dto) => new(
        dto.StudentId,
        dto.CourseId,
        dto.Grade.HasValue ? (Grade?)dto.Grade.Value : null);

    internal static EnrollmentDateGroup ToDomain(this EnrollmentDateGroupDto dto) => new(
        dto.EnrollmentDate,
        dto.StudentCount);
}
