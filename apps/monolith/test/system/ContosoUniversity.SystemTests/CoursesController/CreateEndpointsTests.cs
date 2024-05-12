namespace ContosoUniversity.SystemTests.CoursesController;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Playwright;

using Mvc.ViewModels.Courses;

using NUnit.Framework;

public class CreateEndpointsTests : SystemTest
{
    private static readonly string FormUrl = $"{Configuration["PageBaseUrl:Http"]}/Courses/Create";

    private static readonly CreateCourseRequest ValidRequest = new()
    {
        CourseCode = 1111,
        Title = "Computers",
        Credits = 5,
        DepartmentId = new Guid("dab7e678-e3e7-4471-8282-96fe52e5c16f")
    };

    [TestCaseSource(nameof(ValidationRequests))]
    public async Task PostCreate_WhenInvalidRequest_ReturnsValidationErrorView(
        CreateCourseRequest request,
        string errorMessage)
    {
        // Arrange
        await Page.GotoAsync(FormUrl);
        await FillFormWith(request);
        // await Expect(Page.GetByText(errorMessage)).ToBeHiddenAsync();
        // TODO: why this assertion passes on CI only for Credits validation?

        // Act
        await Page.ClickAsync("input[type=submit]");
        await Expect(Page).ToHaveURLAsync(FormUrl);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        await Expect(Page.GetByText(errorMessage)).ToBeVisibleAsync();
    }

    [Test]
    public async Task PostCreate_WhenValidRequest_CreatesCourseAndRedirectsToListPage()
    {
        // Arrange
        await Page.GotoAsync(FormUrl);
        await FillFormWith(ValidRequest);

        // Act
        await Page.ClickAsync("input[type=submit]");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        await Expect(Page).ToHaveURLAsync($"{Configuration["PageBaseUrl:Http"]}/Courses");
        await Expect(Page.GetByRole(AriaRole.Row, new() { Name = "1111 Computers 5" })).ToBeVisibleAsync();

        // Cleanup created course
        await RemoveCourseByRowDescription("1111 Computers 5");
    }

    public static IEnumerable<TestCaseData> ValidationRequests => new[]
    {
        new TestCaseData(
            ValidRequest with { CourseCode = 999 },
            "Course code can have a value from 1000 to 9999."),
        new TestCaseData(
            ValidRequest with { CourseCode = 10000 },
            "Course code can have a value from 1000 to 9999."),
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