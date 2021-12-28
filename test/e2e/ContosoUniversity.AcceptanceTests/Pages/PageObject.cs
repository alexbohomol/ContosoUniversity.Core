namespace ContosoUniversity.AcceptanceTests.Pages;

using System.Threading.Tasks;

using Microsoft.Playwright;

public abstract class PageObject
{
    protected PageObject(IBrowser browser)
    {
        Browser = browser;
    }

    protected abstract string PagePath { get; }
    protected IPage Page { get; set; }
    private IBrowser Browser { get; }

    public async Task NavigateAsync()
    {
        Page = await Browser.NewPageAsync();
        await Page.GotoAsync(PagePath);
    }
}