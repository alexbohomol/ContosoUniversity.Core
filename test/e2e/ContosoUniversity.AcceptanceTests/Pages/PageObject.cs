namespace ContosoUniversity.AcceptanceTests.Pages;

using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

public abstract class PageObject
{
    protected PageObject(IBrowser browser, IConfiguration configuration)
    {
        Browser = browser;
        Configuration = configuration;
    }

    protected string PageBaseUrl => Configuration["PageBaseUrl:Https"];
    protected abstract string PagePath { get; }
    protected IPage Page { get; set; }
    private IBrowser Browser { get; }
    private IConfiguration Configuration { get; }

    public async Task NavigateAsync()
    {
        Page ??= await Browser.NewPageAsync();
        await Page.GotoAsync(PagePath);
    }

    public async Task NavigateToRouteAsync(string route)
    {
        Page ??= await Browser.NewPageAsync();
        await Page.GotoAsync($"{PagePath}{route}");
    }

    public bool IsAtRoute(string route)
    {
        return Page.Url == $"{PagePath}{route}";
    }

    public async Task<bool> HasTitle(string title)
    {
        return await Page.TitleAsync() == title;
    }

    public bool IsAtRouteWithGuidIdentifier(string route)
    {
        string pathWithoutId = $"{PagePath}{route}";
        string idString = Page.Url[(pathWithoutId.Length + 1)..];
        return Guid.TryParse(idString, out _);
    }

    public async Task ClickLinkWithText(string linkText)
    {
        await Page.ClickAsync($"text={linkText}");
    }

    public async Task ClickSubmitButton()
    {
        await Page.ClickAsync("input[type=submit]");
    }
}
