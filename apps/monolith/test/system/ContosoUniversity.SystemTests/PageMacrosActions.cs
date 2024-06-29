namespace ContosoUniversity.SystemTests;

using System.Threading.Tasks;

using Microsoft.Playwright;

public static class PageMacrosActions
{
    public static async Task ClickLinkByRow(this IPage page, string link, string row)
        => await page
            .GetByRole(AriaRole.Row, new() { Name = row })
            .GetByRole(AriaRole.Link, new() { Name = link })
            .ClickAsync();

    public static async Task ClickButton(this IPage page, string button)
        => await page
            .GetByRole(AriaRole.Button, new() { Name = button })
            .ClickAsync();
}
