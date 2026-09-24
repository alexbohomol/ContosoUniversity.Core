namespace ContosoUniversity.SystemTests.InstructorsController;

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
    public async Task PostDelete_RemovesExistingInstructor()
    {
        // Arrange
        await Page.CreateInstructor(CreateInstructorRequest.Valid);
        await Expect(Page).ToHaveURLAsync(Urls.InstructorsListPage);
        await Expect(Page.InstructorRow(CreateInstructorRequest.Valid.LastName)).ToBeVisibleAsync();
        await Page.ClickLinkByRow("Delete", CreateInstructorRequest.Valid.LastName);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        Page.Url.Should().StartWith(Urls.InstructorsDeletePage);

        // Act
        await Page.ClickButton("Delete");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        await Expect(Page).ToHaveURLAsync(Urls.InstructorsListPage);
        await Expect(Page.InstructorRow(CreateInstructorRequest.Valid.LastName)).ToBeHiddenAsync();
    }
}
