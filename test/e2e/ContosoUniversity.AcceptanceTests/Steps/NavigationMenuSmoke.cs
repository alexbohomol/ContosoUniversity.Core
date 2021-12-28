namespace ContosoUniversity.AcceptanceTests.Steps;

using System.Threading.Tasks;

using FluentAssertions;

using Pages;

using TechTalk.SpecFlow;

[Binding]
public class NavigationMenuSmoke
{
    private readonly ContosoUniversityPage _page;

    public NavigationMenuSmoke(ContosoUniversityPage page)
    {
        _page = page;
    }

    [Given(@"user is on the site landing page")]
    public async Task GivenUserIsOnTheSiteLandingPage()
    {
        await _page.NavigateAsync();
    }

    [When(@"user clicks brand link in the navigation bar")]
    public async Task WhenUserClicksBrandLinkInTheNavigationBar()
    {
        await _page.ClickBrandLink();
    }

    [Then(@"the root of the site opens successfully")]
    public void ThenTheRootOfTheSiteOpensSuccessfully()
    {
        _page.IsAtRoute("/").Should().BeTrue();
    }

    [Then(@"the page title is ""(.*)""")]
    public async Task ThenThePageTitleIs(string pageTitle)
    {
        (await _page.HasTitle(pageTitle)).Should().BeTrue();
    }

    [When(@"user clicks Home link in the navigation bar")]
    public async Task WhenUserClicksHomeLinkInTheNavigationBar()
    {
        await _page.ClickHomeHeaderLink();
    }

    [When(@"user clicks About link in the navigation bar")]
    public async Task WhenUserClicksAboutLinkInTheNavigationBar()
    {
        await _page.ClickAboutHeaderLink();
    }

    [Then(@"the About area opens successfully")]
    public void ThenTheAboutAreaOpensSuccessfully()
    {
        _page.IsAtRoute("/Home/About").Should().BeTrue();
    }

    [When(@"user clicks Students link in the navigation bar")]
    public async Task WhenUserClicksStudentsLinkInTheNavigationBar()
    {
        await _page.ClickStudentsHeaderLink();
    }

    [Then(@"the Students area opens successfully")]
    public void ThenTheStudentsAreaOpensSuccessfully()
    {
        _page.IsAtRoute("/Students").Should().BeTrue();
    }

    [When(@"user clicks Courses link in the navigation bar")]
    public async Task WhenUserClicksCoursesLinkInTheNavigationBar()
    {
        await _page.ClickCoursesHeaderLink();
    }

    [Then(@"the Courses area opens successfully")]
    public void ThenTheCoursesAreaOpensSuccessfully()
    {
        _page.IsAtRoute("/Courses").Should().BeTrue();
    }

    [When(@"user clicks Instructors link in the navigation bar")]
    public async Task WhenUserClicksInstructorsLinkInTheNavigationBar()
    {
        await _page.ClickInstructorsHeaderLink();
    }

    [Then(@"the Instructors area opens successfully")]
    public void ThenTheInstructorsAreaOpensSuccessfully()
    {
        _page.IsAtRoute("/Instructors").Should().BeTrue();
    }

    [When(@"user clicks Departments link in the navigation bar")]
    public async Task WhenUserClicksDepartmentsLinkInTheNavigationBar()
    {
        await _page.ClickDepartmentsHeaderLink();
    }

    [Then(@"the Departments area opens successfully")]
    public void ThenTheDepartmentsAreaOpensSuccessfully()
    {
        _page.IsAtRoute("/Departments").Should().BeTrue();
    }
}