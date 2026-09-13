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
        string initialAdministratorName,
        string updatedAdministratorName)
    {
        // Arrange
        CreateDepartmentRequest initialRequest = CreateDepartmentRequest.Valid with
        {
            AdministratorName = initialAdministratorName
        };
        EditDepartmentRequest updatedRequest = EditDepartmentRequest.Valid with
        {
            AdministratorName = updatedAdministratorName
        };
        await Page.CreateDepartment(initialRequest);
        await Expect(Page).ToHaveURLAsync(Urls.DepartmentsListPage);
        await Page.AssertDepartmentRow(initialRequest);

        string editUrl = await Page.GetByRole(AriaRole.Row, new() { Name = CreateDepartmentRequest.Valid.Name })
            .GetByRole(AriaRole.Link, new() { Name = "Edit" })
            .GetAttributeAsync("href");
        editUrl.Should().NotBeNullOrEmpty();
        await Page.ClickLinkByRow("Edit", CreateDepartmentRequest.Valid.Name);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        Page.Url.Should().EndWith(editUrl);
        Page.Url.Should().StartWith(Urls.DepartmentsEditPage);
        await Page.AssertEditForm(initialRequest);

        // Act
        await Page.FillFormWith(updatedRequest);
        await Page.ClickAsync("input[type=submit]");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        await Expect(Page).ToHaveURLAsync(Urls.DepartmentsListPage);
        await Page.AssertDepartmentRow(updatedRequest);
        await Expect(Page.DepartmentRow(CreateDepartmentRequest.Valid.Name)).ToHaveCountAsync(0);

        await Page.ClickLinkByRow("Edit", EditDepartmentRequest.Valid.Name);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.AssertAdministratorSelection(updatedAdministratorName);

        // Cleanup
        await Page.RemoveDepartment(EditDepartmentRequest.Valid.Name);
    }

    [TestCaseSource(typeof(EditDepartmentRequest), nameof(EditDepartmentRequest.Invalids))]
    public async Task PostEdit_WhenInvalidRequest_ReturnsValidationErrorView(
        EditDepartmentRequest request,
        string errorMessage)
    {
        // Arrange
        CreateDepartmentRequest initialRequest = CreateDepartmentRequest.Valid;
        EditDepartmentRequest invalidRequest = request with
        {
            Budget = initialRequest.Budget,
            StartDate = initialRequest.StartDate,
            AdministratorName = initialRequest.AdministratorName
        };
        await Page.CreateDepartment(initialRequest);
        await Expect(Page).ToHaveURLAsync(Urls.DepartmentsListPage);
        await Page.AssertDepartmentRow(initialRequest);
        await Page.ClickLinkByRow("Edit", initialRequest.Name);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        string editUrl = Page.Url;
        Page.Url.Should().StartWith(Urls.DepartmentsEditPage);
        await Page.FillFormWith(invalidRequest);

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
