namespace ContosoUniversity.SystemTests.DepartmentsController;

using System;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

using NUnit.Framework;
using NUnit.Framework.Interfaces;

public class DeleteEndpointsTests : PageTest
{
    private static readonly SutUrls Urls =
        new(ServiceLocator.GetRequiredService<IConfiguration>());

    [Test]
    public async Task PostDelete_RemovesExistingDepartment()
    {
        string name = $"Informatics-{Guid.NewGuid():N}";
        CreateDepartmentRequest request = CreateDepartmentRequest.Valid with { Name = name };

        try
        {
            await Page.CreateDepartment(request);
            await Expect(Page).ToHaveURLAsync(Urls.DepartmentsListPage);
            ILocator row = Page.DepartmentRow(name);
            await Expect(row).ToHaveCountAsync(1);
            string deleteUrl = await row.GetByRole(AriaRole.Link, new() { Name = "Delete" })
                .GetAttributeAsync("href");
            deleteUrl.Should().NotBeNullOrEmpty();
            await Page.ClickLinkByRow("Delete", name);
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            Page.Url.Should().EndWith(deleteUrl);
            Page.Url.Should().StartWith(Urls.DepartmentsDeletePage);

            await Page.ClickButton("Delete");
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await Expect(Page).ToHaveURLAsync(Urls.DepartmentsListPage);
            await Expect(Page.DepartmentRow(name)).ToHaveCountAsync(0);
        }
        finally
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
}
