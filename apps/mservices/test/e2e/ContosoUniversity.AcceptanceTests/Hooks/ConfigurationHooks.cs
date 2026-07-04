namespace ContosoUniversity.AcceptanceTests.Hooks;

using System;
using System.IO;

using Microsoft.Extensions.Configuration;

using Reqnroll;
using Reqnroll.BoDi;

[Binding]
public class ConfigurationHooks
{
    private const string TestSettingsFile = "testsettings.json";

    [BeforeTestRun(Order = 0)]
    public static void RegisterConfigurationFromFile(IObjectContainer container)
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile(
                Path.Combine(Environment.CurrentDirectory, TestSettingsFile),
                optional: true,
                reloadOnChange: true)
            .Build();

        container.RegisterInstanceAs(config);
    }
}
