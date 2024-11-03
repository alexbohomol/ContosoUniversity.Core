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
}
