namespace ContosoUniversity.SystemTests.DepartmentsController;

using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

using NUnit.Framework;

public class CreateEndpointsTests : PageTest
{
    private static readonly SutUrls Urls =
        new(ServiceLocator.GetRequiredService<IConfiguration>());

    [TestCaseSource(typeof(CreateDepartmentRequest), nameof(CreateDepartmentRequest.ValidInstructorInvariants))]
    public async Task PostCreate_WhenValidRequest_CreatesDepartment(CreateDepartmentRequest request)
    {
        // Arrange
        await Page.GotoAsync(Urls.DepartmentsCreatePage);
        await Page.FillFormWith(request);

        // Act
        await Page.ClickAsync("input[type=submit]");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        await Expect(Page).ToHaveURLAsync(Urls.DepartmentsListPage);
        await Page.AssertDepartmentRow(request);
        await Page.ClickLinkByRow("Edit", CreateDepartmentRequest.Valid.Name);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        Page.Url.Should().StartWith(Urls.DepartmentsEditPage);
        await Page.AssertAdministratorSelection(request.AdministratorName);

        // Cleanup
        await Page.RemoveDepartment(CreateDepartmentRequest.Valid.Name);
    }

    [TestCaseSource(typeof(CreateDepartmentRequest), nameof(CreateDepartmentRequest.Invalids))]
    public async Task PostCreate_WhenInvalidRequest_ReturnsValidationErrorView(
        CreateDepartmentRequest request,
        string errorMessage)
    {
        // Arrange
        await Page.GotoAsync(Urls.DepartmentsCreatePage);
        await Page.FillFormWith(request);

        // Act
        await Page.ClickAsync("input[type=submit]");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        await Expect(Page).ToHaveURLAsync(Urls.DepartmentsCreatePage);
        await Expect(Page.GetByText(errorMessage, new() { Exact = true })).ToBeVisibleAsync();
        await Expect(Page.DepartmentRow(request.Name)).ToHaveCountAsync(0);

        // Cleanup
        await Page.RemoveDepartment(request.Name);
    }
}
