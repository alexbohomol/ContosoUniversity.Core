namespace ContosoUniversity.SystemTests;

using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

using Mvc.ViewModels.Courses;

public abstract class SystemTest : PageTest
{
    protected static readonly IConfiguration Configuration = new ConfigurationBuilder()
        .AddJsonFile("testsettings.json", optional: false)
        .Build();

    protected async Task FillFormWith(CreateCourseRequest request)
    {
        await Page.FillAsync("#Request_CourseCode", request.CourseCode.ToString());
        await Page.FillAsync("#Request_Title", request.Title);
        await Page.FillAsync("#Request_Credits", request.Credits.ToString());

        await Page.SelectOptionAsync("#Request_DepartmentId", new[]
        {
            new SelectOptionValue { Value = request.DepartmentId.ToString() }
        });
    }

    protected async Task FillFormWith(EditCourseRequest request)
    {
        await Page.FillAsync("#Request_Title", request.Title);
        await Page.FillAsync("#Request_Credits", request.Credits.ToString());

        await Page.SelectOptionAsync("#Request_DepartmentId", new[]
        {
            new SelectOptionValue { Value = request.DepartmentId.ToString() }
        });
    }

    protected async Task RemoveCourseByRowDescription(string rowDescription)
    {
        // Ensure we are on the list page and the course is present
        await Expect(Page).ToHaveURLAsync($"{Configuration["PageBaseUrl:Http"]}/Courses");
        await Expect(Page.GetByRole(AriaRole.Row, new() { Name = rowDescription })).ToBeVisibleAsync();

        // Goto the delete page
        await Page
            .GetByRole(AriaRole.Row, new() { Name = rowDescription })
            .GetByRole(AriaRole.Link, new() { Name = "Delete" })
            .ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        Page.Url.Should().StartWith($"{Configuration["PageBaseUrl:Http"]}/Courses/Delete");

        // Delete the course
        await Page
            .GetByRole(AriaRole.Button, new() { Name = "Delete" })
            .ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Ensure we are on the list page and the course is not present
        await Expect(Page).ToHaveURLAsync($"{Configuration["PageBaseUrl:Http"]}/Courses");
        await Expect(Page.GetByRole(AriaRole.Row, new() { Name = rowDescription })).ToBeHiddenAsync();
    }
}
