namespace ContosoUniversity.SystemTests.DepartmentsController;

using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

using NUnit.Framework;

public class DeleteEndpointsTests : PageTest
{
    private static readonly SutUrls Urls =
        new(ServiceLocator.GetRequiredService<IConfiguration>());

    [Test]
    public async Task PostDelete_RemovesExistingDepartment()
    {
        // Arrange
        await Page.CreateDepartment(CreateDepartmentRequest.Valid);
        await Expect(Page).ToHaveURLAsync(Urls.DepartmentsListPage);
        await Expect(Page.GetByRole(AriaRole.Row, new() { Name = CreateDepartmentRequest.Valid.Name })).ToBeVisibleAsync();
        await Page.ClickLinkByRow("Delete", CreateDepartmentRequest.Valid.Name);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        Page.Url.Should().StartWith(Urls.DepartmentsDeletePage);

        // Act
        await Page.ClickButton("Delete");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        await Expect(Page).ToHaveURLAsync(Urls.DepartmentsListPage);
        await Expect(Page.GetByRole(AriaRole.Row, new() { Name = CreateDepartmentRequest.Valid.Name })).ToBeHiddenAsync();
    }
}
