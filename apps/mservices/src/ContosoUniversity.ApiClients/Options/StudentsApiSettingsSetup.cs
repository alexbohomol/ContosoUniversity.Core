namespace ContosoUniversity.ApiClients.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

internal class StudentsApiSettingsSetup(IConfiguration configuration) : IConfigureOptions<StudentsApiSettings>
{
    public void Configure(StudentsApiSettings options)
    {
        configuration
            .GetSection("StudentsApiSettings")
            .Bind(options);
    }
}
