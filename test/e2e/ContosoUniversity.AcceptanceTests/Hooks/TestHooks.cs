namespace ContosoUniversity.AcceptanceTests.Hooks;

using System.Threading.Tasks;

using BoDi;

using Microsoft.Playwright;

using Pages;

using TechTalk.SpecFlow;

[Binding]
public class TestHooks
{
    [BeforeFeature]
    public static async Task BeforeScenario(IObjectContainer container)
    {
        IPlaywright playwright = await Playwright.CreateAsync();
        IBrowser browser = await playwright.Chromium.LaunchAsync(); //new BrowserTypeLaunchOptions
        // {
        //     Headless = false,
        //     SlowMo = 2000
        // });
        var page = new ContosoUniversityPage(browser);
        container.RegisterInstanceAs(playwright);
        container.RegisterInstanceAs(browser);
        container.RegisterInstanceAs(page);
    }

    [AfterFeature]
    public static async Task AfterScenario(IObjectContainer container)
    {
        var browser = container.Resolve<IBrowser>();
        await browser.CloseAsync();
        var playwright = container.Resolve<IPlaywright>();
        playwright.Dispose();
    }
}