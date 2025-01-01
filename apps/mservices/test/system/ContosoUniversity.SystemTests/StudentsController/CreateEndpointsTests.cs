namespace ContosoUniversity.SystemTests.StudentsController;

using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

using NUnit.Framework;

public class CreateEndpointsTests : PageTest
{
    private static readonly SutUrls Urls =
        new(ServiceLocator.GetRequiredService<IConfiguration>());

    [TestCaseSource(typeof(CreateStudentRequest), nameof(CreateStudentRequest.Invalids))]
    public async Task PostCreate_WhenInvalidRequest_ReturnsValidationErrorView(
        CreateStudentRequest request,
        string errorMessage)
    {
        // Arrange
        await Page.GotoAsync(Urls.StudentsCreatePage);
        await Page.FillFormWith(request);
        // await Expect(Page.GetByText(errorMessage)).ToBeHiddenAsync();
        // TODO: why this assertion passes on CI only for Credits validation?

        // Act
        await Page.ClickAsync("input[type=submit]");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        await Expect(Page).ToHaveURLAsync(Urls.StudentsCreatePage);
        await Expect(Page.GetByText(errorMessage)).ToBeVisibleAsync();
    }

    [Test]
    public async Task PostCreate_WhenValidRequest_CreatesCourseAndRedirectsToListPage()
    {
        // Arrange
        await Page.GotoAsync(Urls.StudentsCreatePage);
        await Page.FillFormWith(CreateStudentRequest.Valid);

        // Act
        await Page.ClickAsync("input[type=submit]");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        await Expect(Page).ToHaveURLAsync(Urls.StudentsListPage);
        await Expect(Page.GetByRole(AriaRole.Row, new() { Name = "Alexander Alpert 2022-01-17" })).ToBeVisibleAsync();

        // Cleanup
        await Page.RemoveStudent("Alexander Alpert 2022-01-17");
    }
}
