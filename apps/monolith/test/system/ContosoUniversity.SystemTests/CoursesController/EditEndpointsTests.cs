namespace ContosoUniversity.SystemTests.CoursesController;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

using Mvc.ViewModels.Courses;

using NUnit.Framework;

public class EditEndpointsTests : PageTest
{
    private static readonly string FormUrl;

    private static readonly EditCourseRequest ValidRequest = new()
    {
        Title = "Computers Algebra",
        Credits = 3,
        DepartmentId = new Guid("dab7e678-e3e7-4471-8282-96fe52e5c16f")
    };

    static EditEndpointsTests()
    {
        IConfiguration configuration =
            ServiceLocator.GetRequiredService<IConfiguration>();

        FormUrl = $"{configuration["PageBaseUrl:Http"]}/Courses/Edit";
    }

    [TestCaseSource(nameof(ValidationRequests))]
    public async Task PostEdit_WhenInvalidRequest_ReturnsValidationErrorView(
        EditCourseRequest request,
        string errorMessage)
    {
        // Arrange
        await Page.CreateCourse(new CreateCourseRequest
        {
            CourseCode = 1111,
            Title = "Computers",
            Credits = 5,
            DepartmentId = new Guid("dab7e678-e3e7-4471-8282-96fe52e5c16f")
        });
        await Page.ClickEditLinkByRowDescription("1111 Computers 5");
        await Page.FillFormWith(request);

        // Act
        await Page.ClickAsync("input[type=submit]");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        Page.Url.Should().StartWith(FormUrl);

        // Assert
        await Expect(Page.GetByText(errorMessage)).ToBeVisibleAsync();

        // Cleanup created course
        await Page.RemoveCourseByRowDescription("1111 Computers 5");
    }

    [Test]
    public async Task PostEdit_WhenValidRequest_UpdatesCourseAndRedirectsToListPage()
    {
        // Arrange
        await Page.CreateCourse(new CreateCourseRequest
        {
            CourseCode = 1111,
            Title = "Computers",
            Credits = 5,
            DepartmentId = new Guid("dab7e678-e3e7-4471-8282-96fe52e5c16f")
        });
        await Page.ClickEditLinkByRowDescription("1111 Computers 5");
        await Page.FillFormWith(ValidRequest);

        // Act
        await Page.ClickAsync("input[type=submit]");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        // await Expect(Page).ToHaveURLAsync($"{Configuration["PageBaseUrl:Http"]}/Courses");
        await Expect(Page.GetByRole(AriaRole.Row, new() { Name = "1111 Computers Algebra 3" })).ToBeVisibleAsync();

        // Cleanup created course
        await Page.RemoveCourseByRowDescription("1111 Computers Algebra 3");
    }

    public static IEnumerable<TestCaseData> ValidationRequests => new[]
    {
        new TestCaseData(
            ValidRequest with { Title = "#@" },
            "The field 'Title' must be a string with a minimum length of 3 and a maximum length of 50."),
        new TestCaseData(
            ValidRequest with { Title = "123456789012345678901234567890123456789012345678901" },
            "The field 'Title' must be a string with a minimum length of 3 and a maximum length of 50."),
        new TestCaseData(
            ValidRequest with { Credits = -1 },
            "The field 'Credits' must be between 0 and 5."),
        new TestCaseData(
            ValidRequest with { Credits = 6 },
            "The field 'Credits' must be between 0 and 5."),

        //     "The DepartmentId field is required.",
    };
}
