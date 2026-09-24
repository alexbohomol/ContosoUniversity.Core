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

    [TestCaseSource(typeof(CreateInstructorRequest), nameof(CreateInstructorRequest.ValidInstructorVariants))]
    public async Task PostCreate_WhenValidRequest_CreatesDepartment(CreateInstructorRequest request)
    {
        // Arrange
        await Page.GotoAsync(Urls.DepartmentsCreatePage);
        await Page.FillFormWith(request);

        // Act
        await Page.ClickAsync("input[type=submit]");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        await Expect(Page).ToHaveURLAsync(Urls.DepartmentsListPage);
        await Expect(Page.DepartmentRow(request.Name)).ToBeVisibleAsync();
        await Page.AssertDepartmentRow(request.Name, request.Budget, request.StartDate, request.AdministratorName);

        // Cleanup
        await Page.RemoveDepartment(request.Name);
    }

    [TestCaseSource(typeof(CreateInstructorRequest), nameof(CreateInstructorRequest.Invalids))]
    public async Task PostCreate_WhenInvalidRequest_ReturnsValidationErrorView(
        CreateInstructorRequest request,
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
    }
}
