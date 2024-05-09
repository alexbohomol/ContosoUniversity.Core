namespace ContosoUniversity.AcceptanceTests.Hooks;

using System.Threading.Tasks;

using BoDi;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

using TechTalk.SpecFlow;

[Binding]
public class PlaywrightHooks
{
    [BeforeTestRun]
    public static async Task BeforeTestRun(
        IObjectContainer container,
        IConfiguration configuration)
    {
        IPlaywright playwright = await Playwright.CreateAsync();

        var launchOptions = configuration
            .GetSection(nameof(BrowserTypeLaunchOptions))
            .Get<BrowserTypeLaunchOptions>();

        IBrowser browser = await playwright.Chromium.LaunchAsync(launchOptions);

        container.RegisterInstanceAs(playwright);
        container.RegisterInstanceAs(browser);
    }

    [AfterTestRun]
    public static async Task AfterTestRun(IObjectContainer container)
    {
        var browser = container.Resolve<IBrowser>();
        await browser.CloseAsync();
        var playwright = container.Resolve<IPlaywright>();
        playwright.Dispose();
    }
}
