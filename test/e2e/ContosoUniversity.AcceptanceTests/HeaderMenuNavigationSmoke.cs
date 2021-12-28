namespace ContosoUniversity.AcceptanceTests;

using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

using NUnit.Framework;

[Parallelizable(ParallelScope.Self)]
public class HeaderMenuNavigationSmoke : PageTest
{
    private const string SiteUrl = "https://localhost:5001";

    [Ignore("we will play further with SpecFlow-based E2E testing")]
    [Test]
    [TestCase($"{SiteUrl}/", "Home Page - Contoso University")]
    [TestCase($"{SiteUrl}/Home", "Home Page - Contoso University")]
    [TestCase($"{SiteUrl}/Home/About", "Student Body Statistics - Contoso University")]
    [TestCase($"{SiteUrl}/Students", "Index - Contoso University")]
    [TestCase($"{SiteUrl}/Courses", "Courses - Contoso University")]
    [TestCase($"{SiteUrl}/Instructors", "Instructors - Contoso University")]
    [TestCase($"{SiteUrl}/Departments", "Departments - Contoso University")]
    public async Task Smoke_User_IsAbleTo_NavigateHeaderMenu(string url, string expectedTitle)
    {
        IResponse gotoAsync = await Page.GotoAsync(url);
        string title = await Page.TitleAsync();

        title.Should().Be(expectedTitle);
        gotoAsync!.Ok.Should().BeTrue();
    }
}