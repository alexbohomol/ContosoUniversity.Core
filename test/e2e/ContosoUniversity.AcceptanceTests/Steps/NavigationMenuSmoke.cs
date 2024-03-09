namespace ContosoUniversity.AcceptanceTests.Steps;

using System.Threading.Tasks;

using FluentAssertions;

using Pages;

using TechTalk.SpecFlow;

[Binding]
public class NavigationMenuSmoke(ContosoUniversityPage page)
{
    private const string FeatureTag = "Navigation";

    [Given(@"user is on the site landing page")]
    public async Task GivenUserIsOnTheSiteLandingPage()
    {
        await page.NavigateAsync();
    }

    [When(@"user clicks brand link in the navigation bar")]
    public async Task WhenUserClicksBrandLinkInTheNavigationBar()
    {
        await page.ClickBrandLink();
    }

    [Then(@"the page title is ""(.*)""")]
    [Scope(Tag = FeatureTag)]
    public async Task ThenThePageTitleIs(string pageTitle)
    {
        (await page.HasTitle(pageTitle)).Should().BeTrue();
    }

    [When(@"user clicks ""(.*)"" link in the navigation bar")]
    public async Task WhenUserClicksLinkInTheNavigationBar(string linkText)
    {
        await page.ClickNavigationHeaderByText(linkText);
    }

    [Then(@"the ""(.*)"" area opens successfully")]
    public void ThenTheAreaOpensSuccessfully(string route)
    {
        page.IsAtRoute(route).Should().BeTrue();
    }
}
