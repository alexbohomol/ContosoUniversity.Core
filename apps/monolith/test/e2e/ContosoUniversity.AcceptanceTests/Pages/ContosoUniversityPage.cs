namespace ContosoUniversity.AcceptanceTests.Pages;

using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

public class ContosoUniversityPage(IBrowser browser, IConfiguration configuration)
    : PageObject(browser, configuration)
{
    protected override string PagePath => PageBaseUrl;

    public async Task ClickBrandLink()
    {
        await Page.ClickAsync("a.navbar-brand");
    }

    public async Task ClickNavigationHeaderByText(string linkText)
    {
        await Page.ClickAsync($"a.nav-link:text-is('{linkText}')");
    }
}
