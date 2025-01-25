namespace Students.Api.IntegrationTests;

using Models;

static class MacrosActionsExtensions
{
    public static async Task<(CreateStudentResponse created, Uri location)> CreateStudent(
        this HttpClient httpClient,
        CreateStudentRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("/api/students", request, default);
        response.EnsureSuccessStatusCode();
        var created = await response.Content.ReadFromJsonAsync<CreateStudentResponse>();
        return (created, response.Headers.Location);
    }
}
