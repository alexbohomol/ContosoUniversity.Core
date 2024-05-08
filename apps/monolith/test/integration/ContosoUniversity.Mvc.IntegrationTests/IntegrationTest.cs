namespace ContosoUniversity.Mvc.IntegrationTests;

using Microsoft.Extensions.Configuration;

public class IntegrationTest
{
    protected static readonly IConfiguration Configuration = new ConfigurationBuilder()
        .AddJsonFile("testsettings.json", optional: false)
        .Build();
}
