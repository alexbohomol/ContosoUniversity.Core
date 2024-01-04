namespace ContosoUniversity.AcceptanceTests.Hooks;

using System.Threading.Tasks;

using BoDi;

using Microsoft.Playwright;

using TechTalk.SpecFlow;

[Binding]
public class TestHooks
{
    [BeforeTestRun]
    public static async Task BeforeTestRun(IObjectContainer container)
    {
        IPlaywright playwright = await Playwright.CreateAsync();
        IBrowser browser = await playwright.Chromium.LaunchAsync(); //new BrowserTypeLaunchOptions
        // {
        //     Headless = false,
        //     SlowMo = 2000
        // });
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
