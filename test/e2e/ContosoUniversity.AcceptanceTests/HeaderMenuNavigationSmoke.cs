namespace ContosoUniversity.AcceptanceTests;

using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

using NUnit.Framework;

[Parallelizable(ParallelScope.Self)]
public class HeaderMenuNavigationSmoke : PageTest
{
    private static readonly IConfiguration Configuration = new ConfigurationBuilder()
        .AddJsonFile($"testsettings.json", optional: false)
        .Build();

    [TestCase("/", "Home Page - Contoso University")]
    [TestCase("/Home", "Home Page - Contoso University")]
    [TestCase("/Home/About", "Student Body Statistics - Contoso University")]
    [TestCase("/Students", "Index - Contoso University")]
    [TestCase("/Courses", "Courses - Contoso University")]
    [TestCase("/Instructors", "Instructors - Contoso University")]
    [TestCase("/Departments", "Departments - Contoso University")]
    public async Task Smoke_User_IsAbleTo_NavigateHeaderMenu(string pathUrl, string expectedTitle)
    {
        var pageUrl = $"{Configuration["PageBaseUrl:Https"]}{pathUrl}";
        IResponse gotoAsync = await Page.GotoAsync(pageUrl);
        string title = await Page.TitleAsync();

        title.Should().Be(expectedTitle);
        gotoAsync!.Ok.Should().BeTrue();
    }
}
