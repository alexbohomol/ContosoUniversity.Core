namespace ContosoUniversity.SystemTests.CoursesController;

using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

using Mvc.ViewModels.Courses;

public static class PageMacrosActions
{
    private static readonly SutUrls Urls =
        new(ServiceLocator.GetRequiredService<IConfiguration>());

    public static async Task FillFormWith(this IPage page, CreateCourseRequest request)
    {
        await page.FillAsync("#Request_CourseCode", request.CourseCode.ToString());
        await page.FillAsync("#Request_Title", request.Title);
        await page.FillAsync("#Request_Credits", request.Credits.ToString());

        await page.SelectOptionAsync("#Request_DepartmentId", new[]
        {
            new SelectOptionValue { Value = request.DepartmentId.ToString() }
        });
    }

    public static async Task FillFormWith(this IPage page, EditCourseRequest request)
    {
        await page.FillAsync("#Request_Title", request.Title);
        await page.FillAsync("#Request_Credits", request.Credits.ToString());

        await page.SelectOptionAsync("#Request_DepartmentId", new[]
        {
            new SelectOptionValue { Value = request.DepartmentId.ToString() }
        });
    }

    public static async Task CreateCourse(this IPage page, CreateCourseRequest request)
    {
        await page.GotoAsync(Urls.CoursesCreatePage);
        await page.FillFormWith(request);
        await page.ClickAsync("input[type=submit]");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public static async Task RemoveCourse(this IPage page, string row)
    {
        await page.GotoAsync(Urls.CoursesListPage);
        await page.ClickLinkByRow("Delete", row);
        await page.ClickButton("Delete");
    }
}
