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

    [Ignore("Temporarily ignored")]
    [TestCaseSource(typeof(EditDepartmentRequest), nameof(EditDepartmentRequest.AdministratorTransitions))]
    public async Task PostEdit_WhenValidRequest_UpdatesDepartment(
        CreateInstructorRequest createRequest,
        EditDepartmentRequest editRequest)
    {
        // Arrange
        await Page.CreateInstructor(createRequest);
        await Page.GotoAsync(Urls.DepartmentsListPage);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        // await Expect(Page.DepartmentRow(createRequest.Name)).ToBeVisibleAsync();
        // await Page.AssertDepartmentRow(createRequest.Name, createRequest.Budget, createRequest.StartDate, createRequest.AdministratorName);
        // await Page.ClickLinkByRow("Edit", createRequest.Name);
        await Page.AssertEditForm(createRequest);
        await Page.FillFormWith(editRequest);

        // Act
        await Page.ClickAsync("input[type=submit]");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        await Expect(Page).ToHaveURLAsync(Urls.DepartmentsListPage);
        // await Expect(Page.DepartmentRow(createRequest.Name)).ToBeHiddenAsync();
        await Expect(Page.InstructorRow(editRequest.Name)).ToBeVisibleAsync();
        // await Page.AssertDepartmentRow(editRequest.Name, editRequest.Budget, editRequest.StartDate, editRequest.AdministratorName);
        await Page.ClickLinkByRow("Edit", editRequest.Name);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.AssertAdministratorSelection(editRequest.AdministratorName);

        // Cleanup
        await Page.RemoveInstructor(editRequest.Name);
    }

    [Ignore("Temporarily ignored")]
    [TestCaseSource(typeof(EditDepartmentRequest), nameof(EditDepartmentRequest.Invalids))]
    public async Task PostEdit_WhenInvalidRequest_ReturnsValidationErrorView(
        EditDepartmentRequest editRequest,
        string errorMessage)
    {
        // Arrange
        CreateInstructorRequest request = CreateInstructorRequest.Valid;
        await Page.CreateInstructor(request);
        await Page.GotoAsync(Urls.DepartmentsListPage);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        // await Expect(Page.DepartmentRow(request.Name)).ToBeVisibleAsync();
        // await Page.AssertDepartmentRow(request.Name, request.Budget, request.StartDate, request.AdministratorName);
        // await Page.ClickLinkByRow("Edit", request.Name);
        await Page.AssertEditForm(request);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        string editUrl = Page.Url;
        editUrl.Should().StartWith(Urls.DepartmentsEditPage);
        await Page.FillFormWith(editRequest);

        // Act
        await Page.ClickAsync("input[type=submit]");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        Page.Url.Should().Be(editUrl);
        await Expect(Page.GetByText(errorMessage, new() { Exact = true })).ToBeVisibleAsync();
        await Page.GotoAsync(Urls.DepartmentsListPage);
        await Expect(Page.InstructorRow(editRequest.Name)).ToBeHiddenAsync();
        // await Expect(Page.DepartmentRow(request.Name)).ToBeVisibleAsync();
        // await Page.AssertDepartmentRow(request.Name, request.Budget, request.StartDate, request.AdministratorName);

        // Cleanup
        // await Page.RemoveDepartment(request.Name);
    }
}
