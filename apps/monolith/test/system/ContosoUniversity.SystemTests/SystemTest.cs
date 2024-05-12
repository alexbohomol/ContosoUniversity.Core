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

    protected async Task CreateCourse(CreateCourseRequest request)
    {
        await Page.GotoAsync($"{Configuration["PageBaseUrl:Http"]}/Courses/Create");
        await FillFormWith(request);
        await Page.ClickAsync("input[type=submit]");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Ensure we are on the list page
        await Expect(Page).ToHaveURLAsync($"{Configuration["PageBaseUrl:Http"]}/Courses");
    }

    protected async Task RemoveCourseByRowDescription(string rowDescription)
    {
        // Goto the list page
        await Page.GotoAsync($"{Configuration["PageBaseUrl:Http"]}/Courses");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

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

    protected async Task ClickEditLinkByRowDescription(string rowDescription)
    {
        // Ensure we are on the list page and the course is present
        await Expect(Page).ToHaveURLAsync($"{Configuration["PageBaseUrl:Http"]}/Courses");
        await Expect(Page.GetByRole(AriaRole.Row, new() { Name = rowDescription })).ToBeVisibleAsync();

        // Goto the edit page
        await Page
            .GetByRole(AriaRole.Row, new() { Name = rowDescription })
            .GetByRole(AriaRole.Link, new() { Name = "Edit" })
            .ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        Page.Url.Should().StartWith($"{Configuration["PageBaseUrl:Http"]}/Courses/Edit");
    }
}
