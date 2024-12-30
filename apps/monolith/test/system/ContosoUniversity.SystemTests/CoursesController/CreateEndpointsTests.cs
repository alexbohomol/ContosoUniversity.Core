namespace ContosoUniversity.SystemTests.CoursesController;

using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

using NUnit.Framework;

public class CreateEndpointsTests : PageTest
{
    private static readonly SutUrls Urls =
        new(ServiceLocator.GetRequiredService<IConfiguration>());

    [TestCaseSource(typeof(CreateCourseRequest), nameof(CreateCourseRequest.Invalids))]
    public async Task PostCreate_WhenInvalidRequest_ReturnsValidationErrorView(
        CreateCourseRequest request,
        string errorMessage)
    {
        // Arrange
        await Page.GotoAsync(Urls.CoursesCreatePage);
        await Page.FillFormWith(request);
        // await Expect(Page.GetByText(errorMessage)).ToBeHiddenAsync();
        // TODO: why this assertion passes on CI only for Credits validation?

        // Act
        await Page.ClickAsync("input[type=submit]");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        await Expect(Page).ToHaveURLAsync(Urls.CoursesCreatePage);
        await Expect(Page.GetByText(errorMessage)).ToBeVisibleAsync();
    }

    [Test]
    public async Task PostCreate_WhenValidRequest_CreatesCourseAndRedirectsToListPage()
    {
        // Arrange
        await Page.GotoAsync(Urls.CoursesCreatePage);
        await Page.FillFormWith(CreateCourseRequest.Valid);

        // Act
        await Page.ClickAsync("input[type=submit]");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        await Expect(Page).ToHaveURLAsync(Urls.CoursesListPage);
        await Expect(Page.GetByRole(AriaRole.Row, new() { Name = "1111 Computers 5" })).ToBeVisibleAsync();

        // Cleanup
        await Page.RemoveCourse("1111 Computers 5");
    }
}
