namespace ContosoUniversity.SystemTests.StudentsController;

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
    public async Task PostDelete_RemovesExistingCourse()
    {
        // Arrange
        await Page.CreateStudent(CreateStudentRequests.Valid);
        await Expect(Page).ToHaveURLAsync(Urls.StudentsListPage);
        await Expect(Page.GetByRole(AriaRole.Row, new() { Name = "Alexander Alpert 2022-01-17" })).ToBeVisibleAsync();
        await Page.ClickLinkByRow("Delete", "Alexander Alpert 2022-01-17");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        Page.Url.Should().StartWith(Urls.StudentsDeletePage);

        // Act
        await Page.ClickButton("Delete");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        await Expect(Page).ToHaveURLAsync(Urls.StudentsListPage);
        await Expect(Page.GetByRole(AriaRole.Row, new() { Name = "Alexander Alpert 2022-01-17" })).ToBeHiddenAsync();
    }
}
