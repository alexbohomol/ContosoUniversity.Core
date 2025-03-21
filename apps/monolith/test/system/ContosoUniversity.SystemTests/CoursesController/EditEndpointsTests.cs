namespace ContosoUniversity.SystemTests.CoursesController;

using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

using NUnit.Framework;

public class EditEndpointsTests : PageTest
{
    private static readonly SutUrls Urls =
        new(ServiceLocator.GetRequiredService<IConfiguration>());

    [TestCaseSource(typeof(EditCourseRequest), nameof(EditCourseRequest.Invalids))]
    public async Task PostEdit_WhenInvalidRequest_ReturnsValidationErrorView(
        EditCourseRequest request,
        string errorMessage)
    {
        // Arrange
        await Page.CreateCourse(CreateCourseRequest.Valid);
        await Expect(Page).ToHaveURLAsync(Urls.CoursesListPage);
        await Expect(Page.GetByRole(AriaRole.Row, new() { Name = "1111 Computers 5" })).ToBeVisibleAsync();
        await Page.ClickLinkByRow("Edit", "1111 Computers 5");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        Page.Url.Should().StartWith(Urls.CoursesEditPage);
        await Page.FillFormWith(request);

        // Act
        await Page.ClickAsync("input[type=submit]");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        Page.Url.Should().StartWith(Urls.CoursesEditPage);
        await Expect(Page.GetByText(errorMessage)).ToBeVisibleAsync();

        // Cleanup
        await Page.RemoveCourse("1111 Computers 5");
    }

    [Test]
    public async Task PostEdit_WhenValidRequest_UpdatesCourseAndRedirectsToListPage()
    {
        // Arrange
        await Page.CreateCourse(CreateCourseRequest.Valid);
        await Expect(Page).ToHaveURLAsync(Urls.CoursesListPage);
        await Expect(Page.GetByRole(AriaRole.Row, new() { Name = "1111 Computers 5" })).ToBeVisibleAsync();
        await Page.ClickLinkByRow("Edit", "1111 Computers 5");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        Page.Url.Should().StartWith(Urls.CoursesEditPage);
        await Page.FillFormWith(EditCourseRequest.Valid);

        // Act
        await Page.ClickAsync("input[type=submit]");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        await Expect(Page).ToHaveURLAsync(Urls.CoursesListPage);
        await Expect(Page.GetByRole(AriaRole.Row, new() { Name = "1111 Computers Algebra 3" })).ToBeVisibleAsync();

        // Cleanup
        await Page.RemoveCourse("1111 Computers Algebra 3");
    }
}
