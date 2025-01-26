namespace Departments.Api.IntegrationTests;

using Models;

static class MacrosActionsExtensions
{
    public static async Task<(CreateDepartmentResponse created, Uri location)> CreateDepartment(
        this HttpClient httpClient,
        CreateDepartmentRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("/api/departments", request, default);
        response.EnsureSuccessStatusCode();
        var created = await response.Content.ReadFromJsonAsync<CreateDepartmentResponse>();
        return (created, response.Headers.Location);
    }

    public static async Task<(CreateInstructorResponse created, Uri location)> CreateInstructor(
        this HttpClient httpClient,
        CreateInstructorRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("/api/instructors", request, default);
        response.EnsureSuccessStatusCode();
        var created = await response.Content.ReadFromJsonAsync<CreateInstructorResponse>();
        return (created, response.Headers.Location);
    }
}
