namespace ContosoUniversity.SystemTests.DepartmentsController;

using System;
using System.Globalization;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

using NUnit.Framework;
using NUnit.Framework.Interfaces;

public class EditEndpointsTests : PageTest
{
    private static readonly SutUrls Urls =
        new(ServiceLocator.GetRequiredService<IConfiguration>());

    [TestCaseSource(typeof(EditDepartmentRequest), nameof(EditDepartmentRequest.AdministratorTransitions))]
    public async Task PostEdit_WhenValidRequest_UpdatesDepartment(
        string initialAdministratorName,
        string updatedAdministratorName)
    {
        string token = Guid.NewGuid().ToString("N");
        string initialName = $"Informatics-{token}";
        string updatedName = $"Computers-{token}";
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
        string editUrl = null;

        try
        {
            await Page.CreateDepartment(initialRequest);
            await Expect(Page).ToHaveURLAsync(Urls.DepartmentsListPage);
            await AssertDepartmentRow(initialRequest);

            editUrl = await Page.GetByRole(AriaRole.Row, new() { Name = initialName })
                .GetByRole(AriaRole.Link, new() { Name = "Edit" })
                .GetAttributeAsync("href");
            editUrl.Should().NotBeNullOrEmpty();
            await Page.ClickLinkByRow("Edit", initialName);
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            Page.Url.Should().EndWith(editUrl);
            Page.Url.Should().StartWith(Urls.DepartmentsEditPage);
            await AssertEditForm(initialRequest);

            await Page.FillFormWith(updatedRequest);
            await Page.ClickAsync("input[type=submit]");
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await Expect(Page).ToHaveURLAsync(Urls.DepartmentsListPage);
            await AssertDepartmentRow(updatedRequest);
            await Expect(Page.DepartmentRow(initialName)).ToHaveCountAsync(0);

            await Page.ClickLinkByRow("Edit", updatedName);
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await AssertAdministratorSelection(updatedAdministratorName);
        }
        finally
        {
            await CleanupDepartment(updatedName);
            await CleanupDepartment(initialName);
        }
    }

    [TestCaseSource(typeof(EditDepartmentRequest), nameof(EditDepartmentRequest.Invalids))]
    public async Task PostEdit_WhenInvalidRequest_ReturnsValidationErrorView(
        EditDepartmentRequest request,
        string errorMessage)
    {
        string token = Guid.NewGuid().ToString("N");
        CreateDepartmentRequest initialRequest = CreateDepartmentRequest.Valid with
        {
            Name = $"Informatics-{token}"
        };
        EditDepartmentRequest invalidRequest = request with
        {
            Budget = initialRequest.Budget,
            StartDate = initialRequest.StartDate,
            AdministratorName = initialRequest.AdministratorName
        };
        string editUrl = null;

        try
        {
            await Page.CreateDepartment(initialRequest);
            await Expect(Page).ToHaveURLAsync(Urls.DepartmentsListPage);
            await AssertDepartmentRow(initialRequest);
            await Page.ClickLinkByRow("Edit", initialRequest.Name);
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            editUrl = Page.Url;
            Page.Url.Should().StartWith(Urls.DepartmentsEditPage);
            await Page.FillFormWith(invalidRequest);
            await Page.ClickAsync("input[type=submit]");
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            Page.Url.Should().EndWith(editUrl);
            await Expect(Page.GetByText(errorMessage, new() { Exact = true })).ToBeVisibleAsync();
            await Page.GotoAsync(Urls.DepartmentsListPage);
            await AssertDepartmentRow(initialRequest);
        }
        finally
        {
            await CleanupDepartment(initialRequest.Name);
        }
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

    private async Task CleanupDepartment(string name)
    {
        try
        {
            await Page.RemoveDepartment(name);
        }
        catch (PlaywrightException exception)
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed)
            {
                Assert.Fail($"Department cleanup failed for '{name}': {exception}");
            }

            await TestContext.Progress.WriteLineAsync($"Department cleanup failed for '{name}': {exception}");
        }
    }
}
