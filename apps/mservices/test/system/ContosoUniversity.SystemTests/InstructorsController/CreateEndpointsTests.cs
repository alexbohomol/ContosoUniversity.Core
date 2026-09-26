namespace ContosoUniversity.SystemTests.InstructorsController;

using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

using NUnit.Framework;

public class CreateEndpointsTests : PageTest
{
    private static readonly SutUrls Urls =
        new(ServiceLocator.GetRequiredService<IConfiguration>());

    [Test]
    public async Task PostCreate_WhenValidRequest_CreatesInstructor()
    {
        // Arrange
        var request = CreateInstructorRequest.Valid;
        await Page.GotoAsync(Urls.InstructorsCreatePage);
        await Page.FillFormWith(request);

        // Act
        await Page.ClickAsync("input[type=submit]");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        await Expect(Page).ToHaveURLAsync(Urls.InstructorsListPage);
        await Expect(Page.InstructorRow(request.LastName)).ToBeVisibleAsync();
        await Page.AssertInstructorRow(request.LastName, request.FirstName, request.HireDate, request.Location);

        // Cleanup
        await Page.RemoveInstructor(request.LastName);
    }

    [TestCaseSource(typeof(CreateInstructorRequest), nameof(CreateInstructorRequest.Invalids))]
    public async Task PostCreate_WhenInvalidRequest_ReturnsValidationErrorView(
        CreateInstructorRequest request,
        string errorMessage)
    {
        // Arrange
        await Page.GotoAsync(Urls.InstructorsCreatePage);
        await Page.FillFormWith(request);

        // Act
        await Page.ClickAsync("input[type=submit]");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        await Expect(Page).ToHaveURLAsync(Urls.InstructorsCreatePage);
        await Expect(Page.GetByText(errorMessage, new() { Exact = true })).ToBeVisibleAsync();
        await Expect(Page.InstructorRow(request.LastName)).ToBeHiddenAsync();
    }
}
