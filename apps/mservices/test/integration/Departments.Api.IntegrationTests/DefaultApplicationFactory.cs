namespace Departments.Api.IntegrationTests;

using System;

using Departments.Api;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

public class DefaultApplicationFactory : WebApplicationFactory<IAssemblyMarker>
{
    public Func<string> DataSourceSetterFunction = () => string.Empty;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.Configure<SqlConnectionStringBuilder>(options =>
            {
                options.DataSource = DataSourceSetterFunction();
            });
        });
    }
}
