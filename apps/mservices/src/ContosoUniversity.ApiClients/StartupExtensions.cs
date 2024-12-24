namespace ContosoUniversity.ApiClients;

using Application.ApiClients;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Options;

public static class StartupExtensions
{
    public static void AddCoursesApiClient(this IServiceCollection services)
    {
        services.ConfigureOptions<CoursesApiSettingsSetup>();

        services.AddHttpClient<ICoursesApiClient, CoursesApiClient>((svc, client) =>
        {
            var settings = svc.GetRequiredService<IOptions<CoursesApiSettings>>().Value;

            client.BaseAddress = new Uri(settings.BaseUrl);
        });
    }

    public static void AddDepartmentsApiClients(this IServiceCollection services)
    {
        services.ConfigureOptions<DepartmentsApiSettingsSetup>();

        services.AddHttpClient<IDepartmentsApiClient, DepartmentsApiClient>((svc, client) =>
        {
            var settings = svc.GetRequiredService<IOptions<DepartmentsApiSettings>>().Value;

            client.BaseAddress = new Uri(settings.BaseUrl);
        });

        services.AddHttpClient<IInstructorsApiClient, InstructorsApiClient>((svc, client) =>
        {
            var settings = svc.GetRequiredService<IOptions<DepartmentsApiSettings>>().Value;

            client.BaseAddress = new Uri(settings.BaseUrl);
        });
    }

    public static void AddStudentsApiClient(this IServiceCollection services)
    {
        services.ConfigureOptions<StudentsApiSettingsSetup>();

        services.AddHttpClient<IStudentsApiClient, StudentsApiClient>((svc, client) =>
        {
            var settings = svc.GetRequiredService<IOptions<StudentsApiSettings>>().Value;

            client.BaseAddress = new Uri(settings.BaseUrl);
        });
    }
}
