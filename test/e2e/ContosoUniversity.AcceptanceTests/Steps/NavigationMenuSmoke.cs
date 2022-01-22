namespace ContosoUniversity.AcceptanceTests.Steps;

using System.Threading.Tasks;

using FluentAssertions;

using Pages;

using TechTalk.SpecFlow;

[Binding]
public class NavigationMenuSmoke
{
    private const string FeatureTag = "Navigation";

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

    [Then(@"the page title is ""(.*)""")]
    [Scope(Tag = FeatureTag)]
    public async Task ThenThePageTitleIs(string pageTitle)
    {
        (await _page.HasTitle(pageTitle)).Should().BeTrue();
    }

    [When(@"user clicks ""(.*)"" link in the navigation bar")]
    public async Task WhenUserClicksLinkInTheNavigationBar(string linkText)
    {
        await _page.ClickNavigationHeaderByText(linkText);
    }

    [Then(@"the ""(.*)"" area opens successfully")]
    public void ThenTheAreaOpensSuccessfully(string route)
    {
        _page.IsAtRoute(route).Should().BeTrue();
    }
}