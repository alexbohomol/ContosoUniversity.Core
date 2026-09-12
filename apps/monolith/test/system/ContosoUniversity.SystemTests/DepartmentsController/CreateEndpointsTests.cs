namespace ContosoUniversity.SystemTests.DepartmentsController;

using System.Globalization;
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

    [TestCase(null, TestName = "PostCreate_WhenValidRequestWithoutAdministrator_CreatesDepartment")]
    [TestCase("Zheng, Roger", TestName = "PostCreate_WhenValidRequestWithAdministrator_CreatesDepartment")]
    public async Task PostCreate_WhenValidRequest_CreatesDepartment(string administratorName)
    {
        // Arrange
        string name = CreateDepartmentRequest.Valid.Name;
        CreateDepartmentRequest request = CreateDepartmentRequest.Valid with
        {
            Name = name,
            AdministratorName = administratorName
        };

        await Page.GotoAsync(Urls.DepartmentsCreatePage);
        await Page.FillFormWith(request);

        // Act
        await Page.ClickAsync("input[type=submit]");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        await Expect(Page).ToHaveURLAsync(Urls.DepartmentsListPage);
        await AssertDepartmentRow(request);
        await Page.ClickLinkByRow("Edit", name);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        Page.Url.Should().StartWith(Urls.DepartmentsEditPage);
        await AssertAdministratorSelection(request.AdministratorName);

        // Cleanup
        await Page.RemoveDepartment(name);
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

    private async Task AssertDepartmentRow(CreateDepartmentRequest request)
    {
        ILocator row = Page.DepartmentRow(request.Name);
        await Expect(row).ToHaveCountAsync(1);
        await Expect(row.Locator("td").Nth(0)).ToHaveTextAsync(request.Name);
        await Expect(row.Locator("td").Nth(1)).ToHaveTextAsync(request.Budget.ToString("0.00", CultureInfo.InvariantCulture));
        await Expect(row.Locator("td").Nth(2)).ToHaveTextAsync(request.StartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        await Expect(row.Locator("td").Nth(3)).ToHaveTextAsync(request.AdministratorName ?? string.Empty);
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
