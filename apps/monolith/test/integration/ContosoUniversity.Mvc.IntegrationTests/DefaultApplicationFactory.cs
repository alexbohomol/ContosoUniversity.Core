namespace ContosoUniversity.Mvc.IntegrationTests;

using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public class DefaultApplicationFactory : WebApplicationFactory<Program>
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

            services.RemoveAll<IAntiforgery>();
            services.AddTransient<IAntiforgery, NoOpAntiforgery>();
        });
    }
}

file class NoOpAntiforgery : IAntiforgery
{
    public AntiforgeryTokenSet GetAndStoreTokens(HttpContext httpContext) =>
        new("test", "test", "test", "test");

    public AntiforgeryTokenSet GetTokens(HttpContext httpContext) =>
        new("test", "test", "test", "test");

    public Task<bool> IsRequestValidAsync(HttpContext httpContext) =>
        Task.FromResult(true);

    public void SetCookieTokenAndHeader(HttpContext httpContext) { }

    public Task ValidateRequestAsync(HttpContext httpContext) =>
        Task.CompletedTask;
}
