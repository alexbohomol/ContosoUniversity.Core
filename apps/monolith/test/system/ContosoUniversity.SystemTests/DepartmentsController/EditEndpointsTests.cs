namespace ContosoUniversity.SystemTests.DepartmentsController;

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

    [TestCaseSource(typeof(EditDepartmentRequest), nameof(EditDepartmentRequest.AdministratorTransitions))]
    public async Task PostEdit_WhenValidRequest_UpdatesDepartment(
        CreateDepartmentRequest initialRequest,
        EditDepartmentRequest updateRequest)
    {
        // Arrange
        await Page.CreateDepartment(initialRequest);
        await Expect(Page).ToHaveURLAsync(Urls.DepartmentsListPage);
        await Page.AssertDepartmentRow(initialRequest);
        await Page.ClickLinkByRow("Edit", initialRequest.Name);
        await Page.AssertEditForm(initialRequest);

        // Act
        await Page.FillFormWith(updateRequest);
        await Page.ClickAsync("input[type=submit]");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        await Expect(Page).ToHaveURLAsync(Urls.DepartmentsListPage);
        await Page.AssertDepartmentRow(updateRequest);
        await Expect(Page.DepartmentRow(initialRequest.Name)).ToHaveCountAsync(0);
        await Page.ClickLinkByRow("Edit", updateRequest.Name);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.AssertAdministratorSelection(updateRequest.AdministratorName);

        // Cleanup
        await Page.RemoveDepartment(updateRequest.Name);
    }

    [TestCaseSource(typeof(EditDepartmentRequest), nameof(EditDepartmentRequest.Invalids))]
    public async Task PostEdit_WhenInvalidRequest_ReturnsValidationErrorView(
        EditDepartmentRequest request,
        string errorMessage)
    {
        // Arrange
        CreateDepartmentRequest initialRequest = CreateDepartmentRequest.Valid;
        await Page.CreateDepartment(initialRequest);
        await Expect(Page).ToHaveURLAsync(Urls.DepartmentsListPage);
        await Page.AssertDepartmentRow(initialRequest);
        await Page.ClickLinkByRow("Edit", initialRequest.Name);
        await Page.AssertEditForm(initialRequest);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        string editUrl = Page.Url;
        Page.Url.Should().StartWith(Urls.DepartmentsEditPage);
        await Page.FillFormWith(request);

        // Act
        await Page.ClickAsync("input[type=submit]");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        Page.Url.Should().Be(editUrl);
        await Expect(Page.GetByText(errorMessage, new() { Exact = true })).ToBeVisibleAsync();
        await Page.GotoAsync(Urls.DepartmentsListPage);
        await Page.AssertDepartmentRow(initialRequest);

        // Cleanup
        await Page.RemoveDepartment(initialRequest.Name);
    }
}
