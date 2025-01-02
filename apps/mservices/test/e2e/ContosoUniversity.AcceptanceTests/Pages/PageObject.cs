namespace ContosoUniversity.AcceptanceTests.Pages;

using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

public abstract class PageObject(IBrowser browser, IConfiguration configuration)
{
    protected string PageBaseUrl => configuration["PageBaseUrl:Http"];
    protected abstract string PagePath { get; }
    protected IPage Page { get; set; }

    public async Task NavigateAsync()
    {
        Page ??= await browser.NewPageAsync();
        await Page.GotoAsync(PagePath);
    }

    public async Task NavigateToRouteAsync(string route)
    {
        Page ??= await browser.NewPageAsync();
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
