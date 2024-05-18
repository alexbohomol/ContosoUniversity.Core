namespace ContosoUniversity.SystemTests.CoursesController;

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
        await Page.CreateCourse(CreateCourseRequests.Valid);
        await Expect(Page).ToHaveURLAsync(Urls.CoursesListPage);
        await Expect(Page.GetByRole(AriaRole.Row, new() { Name = "1111 Computers 5" })).ToBeVisibleAsync();
        await Page.ClickLinkByRow("Delete", "1111 Computers 5");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        Page.Url.Should().StartWith(Urls.CoursesDeletePage);

        // Act
        await Page.ClickButton("Delete");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        await Expect(Page).ToHaveURLAsync(Urls.CoursesListPage);
        await Expect(Page.GetByRole(AriaRole.Row, new() { Name = "1111 Computers 5" })).ToBeHiddenAsync();
    }
}
