namespace ContosoUniversity.ApiClients.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

internal class CoursesApiSettingsSetup(IConfiguration configuration) : IConfigureOptions<CoursesApiSettings>
{
    public void Configure(CoursesApiSettings options)
    {
        configuration
            .GetSection("CoursesApiSettings")
            .Bind(options);
    }
}
