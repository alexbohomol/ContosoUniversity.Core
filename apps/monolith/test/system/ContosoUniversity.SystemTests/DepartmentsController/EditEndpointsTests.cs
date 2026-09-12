namespace ContosoUniversity.SystemTests.DepartmentsController;

using System.Globalization;
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
        string initialName = "Informatics";
        string updatedName = "Computers";
        CreateDepartmentRequest initialRequest = CreateDepartmentRequest.Valid with
        {
            Name = initialName,
            AdministratorName = initialAdministratorName
        };
        EditDepartmentRequest updatedRequest = EditDepartmentRequest.Valid with
        {
            Name = updatedName,
            AdministratorName = updatedAdministratorName
        };
        await Page.CreateDepartment(initialRequest);
        await Expect(Page).ToHaveURLAsync(Urls.DepartmentsListPage);
        await AssertDepartmentRow(initialRequest);

        string editUrl = await Page.GetByRole(AriaRole.Row, new() { Name = initialName })
            .GetByRole(AriaRole.Link, new() { Name = "Edit" })
            .GetAttributeAsync("href");
        editUrl.Should().NotBeNullOrEmpty();
        await Page.ClickLinkByRow("Edit", initialName);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        Page.Url.Should().EndWith(editUrl);
        Page.Url.Should().StartWith(Urls.DepartmentsEditPage);
        await AssertEditForm(initialRequest);

        // Act
        await Page.FillFormWith(updatedRequest);
        await Page.ClickAsync("input[type=submit]");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        await Expect(Page).ToHaveURLAsync(Urls.DepartmentsListPage);
        await AssertDepartmentRow(updatedRequest);
        await Expect(Page.DepartmentRow(initialName)).ToHaveCountAsync(0);

        await Page.ClickLinkByRow("Edit", updatedName);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await AssertAdministratorSelection(updatedAdministratorName);

        // Cleanup
        await Page.RemoveDepartment(updatedName);
    }

    [TestCaseSource(typeof(EditDepartmentRequest), nameof(EditDepartmentRequest.Invalids))]
    public async Task PostEdit_WhenInvalidRequest_ReturnsValidationErrorView(
        EditDepartmentRequest request,
        string errorMessage)
    {
        // Arrange
        CreateDepartmentRequest initialRequest = CreateDepartmentRequest.Valid with
        {
            Name = "Informatics"
        };
        EditDepartmentRequest invalidRequest = request with
        {
            Budget = initialRequest.Budget,
            StartDate = initialRequest.StartDate,
            AdministratorName = initialRequest.AdministratorName
        };
        await Page.CreateDepartment(initialRequest);
        await Expect(Page).ToHaveURLAsync(Urls.DepartmentsListPage);
        await AssertDepartmentRow(initialRequest);
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
        await AssertDepartmentRow(initialRequest);

        // Cleanup
        await Page.RemoveDepartment(initialRequest.Name);
    }

    private async Task AssertDepartmentRow(CreateDepartmentRequest request)
    {
        ILocator row = Page.DepartmentRow(request.Name);
        await Expect(row).ToHaveCountAsync(1);
        await Expect(row.Locator("td").Nth(0)).ToHaveTextAsync(request.Name);
        await Expect(row.Locator("td").Nth(1)).ToHaveTextAsync(request.Budget.ToString("0.00", CultureInfo.InvariantCulture));
        await Expect(row.Locator("td").Nth(2)).ToHaveTextAsync(request.StartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        await Expect(row.Locator("td").Nth(3)).ToHaveTextAsync(request.AdministratorName ?? string.Empty);
    }

    private async Task AssertDepartmentRow(EditDepartmentRequest request)
    {
        ILocator row = Page.DepartmentRow(request.Name);
        await Expect(row).ToHaveCountAsync(1);
        await Expect(row.Locator("td").Nth(0)).ToHaveTextAsync(request.Name);
        await Expect(row.Locator("td").Nth(1)).ToHaveTextAsync(request.Budget.ToString("0.00", CultureInfo.InvariantCulture));
        await Expect(row.Locator("td").Nth(2)).ToHaveTextAsync(request.StartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        await Expect(row.Locator("td").Nth(3)).ToHaveTextAsync(request.AdministratorName ?? string.Empty);
    }

    private async Task AssertEditForm(CreateDepartmentRequest request)
    {
        await Expect(Page.Locator("#Request_Name")).ToHaveValueAsync(request.Name);
        await Expect(Page.Locator("#Request_Budget")).ToHaveValueAsync(request.Budget.ToString("0.00", CultureInfo.InvariantCulture));
        await Expect(Page.Locator("#Request_StartDate")).ToHaveValueAsync(request.StartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        await AssertAdministratorSelection(request.AdministratorName);
    }

    private async Task AssertAdministratorSelection(string administratorName)
    {
        if (administratorName is null)
        {
            await Expect(Page.Locator("#Request_AdministratorId")).ToHaveValueAsync(string.Empty);
            return;
        }

        await Expect(Page.Locator("#Request_AdministratorId").Locator("option:checked"))
            .ToHaveTextAsync(administratorName);
    }

}
