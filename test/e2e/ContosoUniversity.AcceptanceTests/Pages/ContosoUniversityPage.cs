namespace ContosoUniversity.AcceptanceTests.Pages;

using System.Threading.Tasks;

using Microsoft.Playwright;

public class ContosoUniversityPage : PageObject
{
    public ContosoUniversityPage(IBrowser browser) : base(browser)
    {
    }

    protected override string PagePath => "https://localhost:10001";

    public async Task ClickBrandLink()
    {
        await Page.ClickAsync("a.navbar-brand");
    }

    public async Task ClickNavigationHeaderByText(string linkText)
    {
        await Page.ClickAsync($"a.nav-link:text-is('{linkText}')");
    }
}
