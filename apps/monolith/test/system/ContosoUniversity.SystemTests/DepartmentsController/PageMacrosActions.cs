namespace ContosoUniversity.SystemTests.DepartmentsController;

using System.Globalization;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

public static class PageMacrosActions
{
    private static readonly SutUrls Urls =
        new(ServiceLocator.GetRequiredService<IConfiguration>());

    public static ILocator DepartmentRow(this IPage page, string name) =>
        page.Locator("table > tbody > tr").Filter(new()
        {
            Has = page.GetByRole(AriaRole.Cell, new() { Name = name, Exact = true })
        });

    public static async Task FillFormWith(this IPage page, CreateDepartmentRequest request)
    {
        await page.FillAsync("#Request_Name", request.Name);
        await page.FillAsync("#Request_Budget", request.Budget.ToString("0.00", CultureInfo.InvariantCulture));
        await page.FillAsync("#Request_StartDate", request.StartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        await SelectAdministrator(page, request.AdministratorName);
    }

    public static async Task FillFormWith(this IPage page, EditDepartmentRequest request)
    {
        await page.FillAsync("#Request_Name", request.Name);
        await page.FillAsync("#Request_Budget", request.Budget.ToString("0.00", CultureInfo.InvariantCulture));
        await page.FillAsync("#Request_StartDate", request.StartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        await SelectAdministrator(page, request.AdministratorName);
    }

    public static async Task CreateDepartment(this IPage page, CreateDepartmentRequest request)
    {
        await page.GotoAsync(Urls.DepartmentsCreatePage);
        await page.FillFormWith(request);
        await page.ClickAsync("input[type=submit]");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public static async Task RemoveDepartment(this IPage page, string name)
    {
        await page.GotoAsync(Urls.DepartmentsListPage);
        ILocator row = page.DepartmentRow(name);
        if (await row.CountAsync() == 0)
        {
            return;
        }

        await page.ClickLinkByRow("Delete", name);
        await page.ClickButton("Delete");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    private static async Task SelectAdministrator(IPage page, string administratorName)
    {
        if (administratorName is null)
        {
            await page.SelectOptionAsync("#Request_AdministratorId", new SelectOptionValue { Value = string.Empty });
            return;
        }

        await page.SelectOptionAsync("#Request_AdministratorId", new SelectOptionValue { Label = administratorName });
    }
}
