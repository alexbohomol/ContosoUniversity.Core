namespace ContosoUniversity.SystemTests;

using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceLocator
{
    static ServiceLocator() => _serviceProvider = BuildServiceProvider();

    private static readonly IServiceProvider _serviceProvider;

    private static ServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();

        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("testsettings.json", optional: false)
            .Build();

        services.AddSingleton(configuration);

        return services.BuildServiceProvider();
    }

    public static T GetRequiredService<T>() => _serviceProvider.GetRequiredService<T>();
}
