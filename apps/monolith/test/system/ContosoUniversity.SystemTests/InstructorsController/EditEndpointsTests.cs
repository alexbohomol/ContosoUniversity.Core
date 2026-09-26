namespace ContosoUniversity.SystemTests.InstructorsController;

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

    [Test]
    public async Task PostEdit_WhenValidRequest_UpdatesInstructor()
    {
        // Arrange
        var createRequest = CreateInstructorRequest.Valid;
        var editRequest = EditInstructorRequest.Valid;
        await Page.CreateInstructor(createRequest);
        await Page.GotoAsync(Urls.InstructorsListPage);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Expect(Page.InstructorRow(createRequest.LastName)).ToBeVisibleAsync();
        await Page.AssertInstructorRow(createRequest.LastName, createRequest.FirstName, createRequest.HireDate, createRequest.Location);
        await Page.ClickLinkByRow("Edit", createRequest.LastName);
        await Page.AssertEditForm(createRequest);
        await Page.FillFormWith(editRequest);

        // Act
        await Page.ClickAsync("input[type=submit]");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        await Expect(Page).ToHaveURLAsync(Urls.InstructorsListPage);
        await Expect(Page.InstructorRow(createRequest.LastName)).ToBeHiddenAsync();
        await Expect(Page.InstructorRow(editRequest.LastName)).ToBeVisibleAsync();
        await Page.AssertInstructorRow(editRequest.LastName, editRequest.FirstName, editRequest.HireDate, editRequest.Location);
        await Page.ClickLinkByRow("Edit", editRequest.LastName);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Cleanup
        await Page.RemoveInstructor(editRequest.LastName);
    }

    [TestCaseSource(typeof(EditInstructorRequest), nameof(EditInstructorRequest.Invalids))]
    public async Task PostEdit_WhenInvalidRequest_ReturnsValidationErrorView(
        EditInstructorRequest editRequest,
        string errorMessage)
    {
        // Arrange
        CreateInstructorRequest request = CreateInstructorRequest.Valid;
        await Page.CreateInstructor(request);
        await Page.GotoAsync(Urls.InstructorsListPage);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Expect(Page.InstructorRow(request.LastName)).ToBeVisibleAsync();
        await Page.AssertInstructorRow(request.LastName, request.FirstName, request.HireDate, request.Location);
        await Page.ClickLinkByRow("Edit", request.LastName);
        await Page.AssertEditForm(request);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        string editUrl = Page.Url;
        editUrl.Should().StartWith(Urls.InstructorsEditPage);
        await Page.FillFormWith(editRequest);

        // Act
        await Page.ClickAsync("input[type=submit]");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        Page.Url.Should().Be(editUrl);
        await Expect(Page.GetByText(errorMessage, new() { Exact = true })).ToBeVisibleAsync();
        await Page.GotoAsync(Urls.InstructorsListPage);
        await Expect(Page.InstructorRow(editRequest.LastName)).ToBeHiddenAsync();
        await Expect(Page.InstructorRow(request.LastName)).ToBeVisibleAsync();
        await Page.AssertInstructorRow(request.LastName, request.FirstName, request.HireDate, request.Location);

        // Cleanup
        await Page.RemoveInstructor(request.LastName);
    }
}
