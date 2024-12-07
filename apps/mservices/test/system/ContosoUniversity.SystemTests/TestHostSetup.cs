namespace ContosoUniversity.SystemTests;

using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

// using NUnit.Framework;

using Program = Mvc.Program;

// [SetUpFixture]
public class TestHostSetup
{
    private static readonly IConfiguration Configuration = new ConfigurationBuilder()
        .AddJsonFile("testsettings.json", optional: false)
        .Build();

    private static IHost _host;

    // [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        var hostUrls = Configuration["ApplicationUrls"].Split(';');

        _host = Program.CreateHostBuilder(null, hostUrls)
            .UseContentRoot(
                Path.Combine(
                    Environment.CurrentDirectory,
                    "../../../../../../src/ContosoUniversity.Mvc"))
            .Build();

        await _host.StartAsync();
    }

    // [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _host.StopAsync();
    }
}
