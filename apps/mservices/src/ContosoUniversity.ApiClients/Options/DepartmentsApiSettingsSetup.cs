namespace ContosoUniversity.ApiClients.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

internal class DepartmentsApiSettingsSetup(IConfiguration configuration) : IConfigureOptions<DepartmentsApiSettings>
{
    public void Configure(DepartmentsApiSettings options)
    {
        configuration
            .GetSection("DepartmentsApiSettings")
            .Bind(options);
    }
}
