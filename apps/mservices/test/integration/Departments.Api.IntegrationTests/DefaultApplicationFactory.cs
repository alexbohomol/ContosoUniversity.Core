namespace Departments.Api.IntegrationTests;

using System;

using Api;

using MassTransit;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

public class DefaultApplicationFactory : WebApplicationFactory<IAssemblyMarker>
{
    public Func<string> DataSourceSetterFunction = () => string.Empty;
    public Func<string> RabbitMqConnectionSetterFunction;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.Configure<SqlConnectionStringBuilder>(options =>
            {
                options.DataSource = DataSourceSetterFunction();
            });

            if (RabbitMqConnectionSetterFunction is not null)
            {
                services.Configure<RabbitMqTransportOptions>(options =>
                {
                    var rabbitUri = new Uri(RabbitMqConnectionSetterFunction());
                    options.Port = (ushort)rabbitUri.Port;
                });
            }
        });
    }
}
