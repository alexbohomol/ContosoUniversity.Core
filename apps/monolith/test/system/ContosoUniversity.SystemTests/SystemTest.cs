namespace ContosoUniversity.SystemTests;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright.NUnit;

public abstract class SystemTest : PageTest
{
    protected static readonly IConfiguration Configuration = new ConfigurationBuilder()
        .AddJsonFile("testsettings.json", optional: false)
        .Build();
}
