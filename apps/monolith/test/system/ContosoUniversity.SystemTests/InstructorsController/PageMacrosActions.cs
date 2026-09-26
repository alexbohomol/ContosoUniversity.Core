namespace ContosoUniversity.SystemTests.InstructorsController;

using System;
using System.Globalization;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

using static Microsoft.Playwright.Assertions;

public static class PageMacrosActions
{
    private static readonly SutUrls Urls =
        new(ServiceLocator.GetRequiredService<IConfiguration>());

    public static ILocator InstructorRow(this IPage page, string name) =>
        page.Locator("table > tbody > tr").Filter(new()
        {
            Has = page.GetByRole(AriaRole.Cell, new() { Name = name, Exact = true })
        });

    public static async Task FillFormWith(this IPage page, CreateInstructorRequest request)
    {
        await page.FillAsync("#Request_LastName", request.LastName);
        await page.FillAsync("#Request_FirstName", request.FirstName);
        await page.FillAsync("#Request_HireDate", request.HireDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        await page.FillAsync("#Request_Location", request.Location);
    }

    public static async Task FillFormWith(this IPage page, EditInstructorRequest request)
    {
        await page.FillAsync("#Request_LastName", request.LastName);
        await page.FillAsync("#Request_FirstName", request.FirstName);
        await page.FillAsync("#Request_HireDate", request.HireDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        await page.FillAsync("#Request_Location", request.Location);
    }

    public static async Task CreateInstructor(this IPage page, CreateInstructorRequest request)
    {
        await page.GotoAsync(Urls.InstructorsCreatePage);
        await page.FillFormWith(request);
        await page.ClickAsync("input[type=submit]");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public static async Task RemoveInstructor(this IPage page, string name)
    {
        await page.GotoAsync(Urls.InstructorsListPage);
        await page.ClickLinkByRow("Delete", name);
        await page.ClickButton("Delete");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public static async Task AssertInstructorRow(this IPage page,
        string lastName,
        string firstName,
        DateTime hireDate,
        string location)
    {
        ILocator row = page.InstructorRow(lastName);
        await Expect(row).ToHaveCountAsync(1);
        await Expect(row.Locator("td").Nth(0)).ToHaveTextAsync(lastName);
        await Expect(row.Locator("td").Nth(1)).ToHaveTextAsync(firstName);
        await Expect(row.Locator("td").Nth(2)).ToHaveTextAsync(hireDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        await Expect(row.Locator("td").Nth(3)).ToHaveTextAsync(location ?? string.Empty);
    }

    public static async Task AssertEditForm(this IPage page, CreateInstructorRequest request)
    {
        await Expect(page.Locator("#Request_LastName")).ToHaveValueAsync(request.LastName);
        await Expect(page.Locator("#Request_FirstName")).ToHaveValueAsync(request.FirstName);
        await Expect(page.Locator("#Request_HireDate")).ToHaveValueAsync(request.HireDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        await Expect(page.Locator("#Request_Location")).ToHaveValueAsync(request.Location);
    }
}
