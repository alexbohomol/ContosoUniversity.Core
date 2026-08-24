namespace ContosoUniversity.Data;

using Microsoft.Extensions.DependencyInjection;

using Options;

public static class StartupExtensions
{
    public static void AddDataInfrastructure(this IServiceCollection services)
    {
        services.ConfigureOptions<SqlConnectionStringBuilderSetup>();
    }
}
