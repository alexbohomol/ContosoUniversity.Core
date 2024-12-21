namespace Departments.Api.IntegrationTests;

using System;

using Microsoft.Extensions.Configuration;

public class TestsConfiguration
{
    private readonly IConfiguration _configuration = new ConfigurationBuilder()
        .AddJsonFile("testsettings.json", optional: false)
        .Build();

    public Uri BaseAddressHttpsUrl => new(_configuration["PageBaseUrl:Https"]!);
}
