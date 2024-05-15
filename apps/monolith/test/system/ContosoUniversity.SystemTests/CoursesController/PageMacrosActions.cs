namespace ContosoUniversity.SystemTests.CoursesController;

using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

using Mvc.ViewModels.Courses;

public static class PageMacrosActions
{
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
        IConfiguration configuration =
            ServiceLocator.GetRequiredService<IConfiguration>();

        await page.GotoAsync($"{configuration["PageBaseUrl:Http"]}/Courses/Create");
        await page.FillFormWith(request);
        await page.ClickAsync("input[type=submit]");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Ensure we are on the list page
        // await Expect(Page).ToHaveURLAsync($"{configuration["PageBaseUrl:Http"]}/Courses");
    }

    public static async Task RemoveCourseByRowDescription(this IPage page, string rowDescription)
    {
        IConfiguration configuration =
            ServiceLocator.GetRequiredService<IConfiguration>();

        // Goto the list page
        await page.GotoAsync($"{configuration["PageBaseUrl:Http"]}/Courses");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Ensure we are on the list page and the course is present
        // await Expect(page).ToHaveURLAsync($"{configuration["PageBaseUrl:Http"]}/Courses");
        // await Expect(page.GetByRole(AriaRole.Row, new() { Name = rowDescription })).ToBeVisibleAsync();

        // Goto the delete page
        await page
            .GetByRole(AriaRole.Row, new() { Name = rowDescription })
            .GetByRole(AriaRole.Link, new() { Name = "Delete" })
            .ClickAsync();
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        page.Url.Should().StartWith($"{configuration["PageBaseUrl:Http"]}/Courses/Delete");

        // Delete the course
        await page
            .GetByRole(AriaRole.Button, new() { Name = "Delete" })
            .ClickAsync();
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Ensure we are on the list page and the course is not present
        // await Expect(Page).ToHaveURLAsync($"{configuration["PageBaseUrl:Http"]}/Courses");
        // await Expect(Page.GetByRole(AriaRole.Row, new() { Name = rowDescription })).ToBeHiddenAsync();
    }

    public static async Task ClickEditLinkByRowDescription(this IPage page, string rowDescription)
    {
        IConfiguration configuration =
            ServiceLocator.GetRequiredService<IConfiguration>();

        // Ensure we are on the list page and the course is present
        // await Expect(page).ToHaveURLAsync($"{configuration["PageBaseUrl:Http"]}/Courses");
        // await Expect(page.GetByRole(AriaRole.Row, new() { Name = rowDescription })).ToBeVisibleAsync();

        // Goto the edit page
        await page
            .GetByRole(AriaRole.Row, new() { Name = rowDescription })
            .GetByRole(AriaRole.Link, new() { Name = "Edit" })
            .ClickAsync();
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        page.Url.Should().StartWith($"{configuration["PageBaseUrl:Http"]}/Courses/Edit");
    }
}
