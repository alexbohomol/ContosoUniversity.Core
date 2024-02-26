namespace ContosoUniversity.AcceptanceTests.Hooks;

using Microsoft.Extensions.Hosting;

using Mvc;

using TechTalk.SpecFlow;

[Binding]
public class TestHostHooks
{
    private static IHost _host;
    private const string ApplicationUrls = "https://localhost:10001;http://localhost:10000";

    [BeforeTestRun]
    public static void BeforeTestRun()
    {
        _host = Program.CreateHostBuilder(null, ApplicationUrls.Split(';'))
            //.UseContentRoot(Path.Combine(Environment.CurrentDirectory, "../../../../../../src/ContosoUniversity.Mvc"))
            .Build();

        _host.Start();
    }

    [AfterTestRun]
    public static void AfterTestRun()
    {
        _host.StopAsync().Wait();
    }
}
