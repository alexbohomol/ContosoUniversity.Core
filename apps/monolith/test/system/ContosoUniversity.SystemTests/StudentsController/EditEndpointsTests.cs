namespace ContosoUniversity.SystemTests.StudentsController;

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

    [TestCaseSource(typeof(EditStudentRequest), nameof(EditStudentRequest.Invalids))]
    public async Task PostEdit_WhenInvalidRequest_ReturnsValidationErrorView(
        EditStudentRequest request,
        string errorMessage)
    {
        // Arrange
        await Page.CreateStudent(CreateStudentRequest.Valid);
        await Expect(Page).ToHaveURLAsync(Urls.StudentsListPage);
        await Expect(Page.GetByRole(AriaRole.Row, new() { Name = "Alexander Alpert 2022-01-17" })).ToBeVisibleAsync();
        await Page.ClickLinkByRow("Edit", "Alexander Alpert 2022-01-17");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        Page.Url.Should().StartWith(Urls.StudentsEditPage);
        await Page.FillFormWith(request);

        // Act
        await Page.ClickAsync("input[type=submit]");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        Page.Url.Should().StartWith(Urls.StudentsEditPage);
        await Expect(Page.GetByText(errorMessage)).ToBeVisibleAsync();

        // Cleanup
        await Page.RemoveStudent("Alexander Alpert 2022-01-17");
    }

    [Test]
    public async Task PostEdit_WhenValidRequest_UpdatesCourseAndRedirectsToListPage()
    {
        // Arrange
        await Page.CreateStudent(CreateStudentRequest.Valid);
        await Expect(Page).ToHaveURLAsync(Urls.StudentsListPage);
        await Expect(Page.GetByRole(AriaRole.Row, new() { Name = "Alexander Alpert 2022-01-17" })).ToBeVisibleAsync();
        await Page.ClickLinkByRow("Edit", "Alexander Alpert 2022-01-17");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        Page.Url.Should().StartWith(Urls.StudentsEditPage);
        await Page.FillFormWith(EditStudentRequest.Valid);

        // Act
        await Page.ClickAsync("input[type=submit]");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        await Expect(Page).ToHaveURLAsync(Urls.StudentsListPage);
        await Expect(Page.GetByRole(AriaRole.Row, new() { Name = "Alex Cupertino 2022-01-31" })).ToBeVisibleAsync();

        // Cleanup
        await Page.RemoveStudent("Alex Cupertino 2022-01-31");
    }
}
