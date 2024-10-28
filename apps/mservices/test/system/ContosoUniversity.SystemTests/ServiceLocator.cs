namespace ContosoUniversity.SystemTests;

using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceLocator
{
    static ServiceLocator()
    {
        ServiceProvider = BuildServiceProvider();
    }

    private static readonly IServiceProvider ServiceProvider;

    private static IServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();

        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("testsettings.json", optional: false)
            .Build();

        services.AddSingleton(configuration);

        return services.BuildServiceProvider();
    }

    public static T GetRequiredService<T>()
    {
        return ServiceProvider.GetRequiredService<T>();
    }
}
