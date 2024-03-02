namespace ContosoUniversity.AcceptanceTests.Hooks;

using System;
using System.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Mvc;

using TechTalk.SpecFlow;

[Binding]
public class TestHostHooks
{
    private static IHost _host;

    [BeforeTestRun(Order = 1)]
    public static void BeforeTestRun(IConfiguration configuration)
    {
        _host = Program.CreateHostBuilder(null, configuration["ApplicationUrls"].Split(';'))
            .UseContentRoot(Path.Combine(Environment.CurrentDirectory, "../../../../../../src/ContosoUniversity.Mvc"))
            .Build();

        _host.Start();
    }

    [AfterTestRun]
    public static void AfterTestRun()
    {
        _host.StopAsync().Wait();
    }
}
