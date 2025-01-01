namespace ContosoUniversity.SystemTests.StudentsController;

using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

public static class PageMacrosActions
{
    private static readonly SutUrls Urls =
        new(ServiceLocator.GetRequiredService<IConfiguration>());

    public static async Task FillFormWith(this IPage page, CreateStudentRequest request)
    {
        await page.FillAsync("#Request_FirstName", request.FirstName);
        await page.FillAsync("#Request_LastName", request.LastName);
        await page.FillAsync("#Request_EnrollmentDate", request.EnrollmentDate.ToString("yyyy-MM-dd"));
    }

    public static async Task FillFormWith(this IPage page, EditStudentRequest request)
    {
        await page.FillAsync("#Request_FirstName", request.FirstName);
        await page.FillAsync("#Request_LastName", request.LastName);
        await page.FillAsync("#Request_EnrollmentDate", request.EnrollmentDate.ToString("yyyy-MM-dd"));
    }

    public static async Task CreateStudent(this IPage page, CreateStudentRequest request)
    {
        await page.GotoAsync(Urls.StudentsCreatePage);
        await page.FillFormWith(request);
        await page.ClickAsync("input[type=submit]");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public static async Task RemoveStudent(this IPage page, string row)
    {
        await page.GotoAsync(Urls.StudentsListPage);
        await page.ClickLinkByRow("Delete", row);
        await page.ClickButton("Delete");
    }
}
