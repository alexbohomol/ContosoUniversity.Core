namespace ContosoUniversity.SystemTests.Hooks;

using System;
using System.IO;

using BoDi;

using Microsoft.Extensions.Configuration;

using TechTalk.SpecFlow;

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
