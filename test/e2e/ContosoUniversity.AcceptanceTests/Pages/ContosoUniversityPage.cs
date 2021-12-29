namespace ContosoUniversity.AcceptanceTests.Pages;

using System.Threading.Tasks;

using Microsoft.Playwright;

public class ContosoUniversityPage : PageObject
{
    public ContosoUniversityPage(IBrowser browser) : base(browser)
    {
    }

    protected override string PagePath => "https://localhost:5001";

    public async Task ClickBrandLink()
    {
        await Page.ClickAsync("a.navbar-brand");
    }

    public async Task ClickHomeHeaderLink()
    {
        await Page.ClickAsync("a.nav-link[href='/']");
    }

    public async Task ClickAboutHeaderLink()
    {
        await Page.ClickAsync("a.nav-link[href='/Home/About']");
    }

    public async Task ClickStudentsHeaderLink()
    {
        await Page.ClickAsync("a.nav-link[href='/Students']");
    }

    public async Task ClickCoursesHeaderLink()
    {
        await Page.ClickAsync("a.nav-link[href='/Courses']");
    }

    public async Task ClickInstructorsHeaderLink()
    {
        await Page.ClickAsync("a.nav-link[href='/Instructors']");
    }

    public async Task ClickDepartmentsHeaderLink()
    {
        await Page.ClickAsync("a.nav-link[href='/Departments']");
    }
}