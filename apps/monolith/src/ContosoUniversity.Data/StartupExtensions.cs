namespace ContosoUniversity.Data;

using Connection;

using Microsoft.Extensions.DependencyInjection;

public static class StartupExtensions
{
    public static void AddDataInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionResolver, DefaultConnectionResolver>();
    }
}
