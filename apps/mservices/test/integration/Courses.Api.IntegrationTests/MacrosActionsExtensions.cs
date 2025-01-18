namespace Courses.Api.IntegrationTests;

using Models;

static class MacrosActionsExtensions
{
    public static async Task<(CreateCourseResponse created, Uri Location)> CreateCourse(
        this HttpClient httpClient,
        CreateCourseRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("/api/courses", request, default);
        response.EnsureSuccessStatusCode();
        var created = await response.Content.ReadFromJsonAsync<CreateCourseResponse>();
        return (created, response.Headers.Location);
    }
}
