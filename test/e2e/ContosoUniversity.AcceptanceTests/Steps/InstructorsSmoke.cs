namespace ContosoUniversity.AcceptanceTests.Steps;

using System.Threading.Tasks;

using FluentAssertions;

using Models;

using Pages;

using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

[Binding]
public class InstructorsSmoke
{
    private const string FeatureTag = "Instructors";

    private const string InitialListOfInstructors = nameof(InitialListOfInstructors);
    private const string SubmittedInstructor = nameof(SubmittedInstructor);
    private const string ListAfterInstructorSubmitted = nameof(ListAfterInstructorSubmitted);

    private readonly InstructorsAreaPage _page;
    private readonly ScenarioContext _scenarioContext;

    public InstructorsSmoke(InstructorsAreaPage page, ScenarioContext scenarioContext)
    {
        _page = page;
        _scenarioContext = scenarioContext;
    }

    [Given(@"user is on the Instructors area landing page")]
    public async Task GivenUserIsOnTheInstructorsAreaLandingPage()
    {
        await _page.NavigateAsync();

        _scenarioContext.Add(InitialListOfInstructors, await _page.ScrapRenderedInstructorsList());
    }

    [Then(@"user is able to view the following list of instructors")]
    public void ThenUserIsAbleToViewTheFollowingListOfInstructors(Table table)
    {
        InstructorTableRowModel[] renderedInstructor = _scenarioContext
            .Get<InstructorTableRowModel[]>(InitialListOfInstructors);

        renderedInstructor.Length.Should().Be(5);
        renderedInstructor.Should().BeEquivalentTo(table.CreateSet<InstructorTableRowModel>());
    }

    [Then(@"the page title is ""(.*)""")]
    [Scope(Tag = FeatureTag)]
    public async Task ThenThePageTitleIs(string pageTitle)
    {
        (await _page.HasTitle(pageTitle)).Should().BeTrue();
    }

    [When(@"user clicks ""(.*)"" link")]
    [Scope(Tag = FeatureTag)]
    public async Task WhenUserClicksLink(string linkText)
    {
        await _page.ClickLinkWithText(linkText);
    }

    [Then(@"the ""(.*)"" page opens successfully")]
    [Scope(Tag = FeatureTag)]
    public void ThenThePageOpensSuccessfully(string pageRoute)
    {
        _page.IsAtRoute(pageRoute).Should().BeTrue();
    }

    [Given(@"user is on the ""(.*)"" page")]
    [Scope(Tag = FeatureTag)]
    public async Task GivenUserIsOnThePage(string route)
    {
        await _page.NavigateToRouteAsync(route);
    }

    [When(@"user enters following details on form")]
    [Scope(Tag = FeatureTag)]
    public async Task WhenUserEntersFollowingDetailsOnForm(Table table)
    {
        var model = table.CreateInstance<InstructorTableRowModel>();
        await _page.EnterInstructorDetails(model);
        _scenarioContext.Add(SubmittedInstructor, model);
    }

    [When(@"user submits the form")]
    [Scope(Tag = FeatureTag)]
    public async Task WhenUserSubmitsTheForm()
    {
        await _page.ClickSubmitButton();
    }

    [Then(@"user is redirected to the Instructors area landing page")]
    public void ThenUserIsRedirectedToTheInstructorsAreaLandingPage()
    {
        _page.IsAtRoute("").Should().BeTrue();
    }

    [Then(@"user is able to view the full list of instructors")]
    public async Task ThenUserIsAbleToViewTheFullListOfInstructors()
    {
        _scenarioContext.Add(ListAfterInstructorSubmitted, await _page.ScrapRenderedInstructorsList());
    }

    [Then(@"including the instructor just submitted")]
    public void ThenIncludingTheInstructorJustSubmitted()
    {
        InstructorTableRowModel[] listAfterSubmit =
            _scenarioContext.Get<InstructorTableRowModel[]>(ListAfterInstructorSubmitted);
        var submittedInstructor = _scenarioContext.Get<InstructorTableRowModel>(SubmittedInstructor);

        listAfterSubmit.Length.Should().Be(6);
        listAfterSubmit.Should().Contain(submittedInstructor);
    }

    [When(@"user clicks ""(.*)"" link for instructor ""(.*)""")]
    public async Task WhenUserClicksLinkForInstructor(string link, string instructorName)
    {
        await _page.ClickLinkOnInstructorsTable(link, instructorName);
    }

    [Then(@"the ""(.*)"" page with identifier opens successfully")]
    [Scope(Tag = FeatureTag)]
    public void ThenThePageWithIdentifierOpensSuccessfully(string pageRoute)
    {
        _page.IsAtRouteWithGuidIdentifier(pageRoute).Should().BeTrue();
    }

    [Then(@"user is able to see the following details on instructor")]
    public async Task ThenUserIsAbleToSeeTheFollowingDetailsOnInstructor(Table table)
    {
        InstructorTableRowModel details = await _page.ScrapRenderedInstructorDetails();

        details.Should().Be(table.CreateInstance<InstructorTableRowModel>());
    }

    [Then(@"user is able to see the following instructor details to edit")]
    public async Task ThenUserIsAbleToSeeTheFollowingInstructorDetailsToEdit(Table table)
    {
        InstructorTableRowModel details = await _page.ScrapRenderedInstructorDetailsToEdit();

        details.Should().Be(table.CreateInstance<InstructorTableRowModel>());
    }

    [Given(@"user is on the ""(.*)"" page for instructor ""(.*)""")]
    public async Task GivenUserIsOnThePageForInstructor(string link, string instructorName)
    {
        await GivenUserIsOnTheInstructorsAreaLandingPage();
        await WhenUserClicksLinkForInstructor(link, instructorName);
    }

    [Then(@"excluding the instructor just deleted")]
    public void ThenExcludingTheInstructorJustDeleted(Table table)
    {
        var removedCourse = table.CreateInstance<InstructorTableRowModel>();
        InstructorTableRowModel[] listAfterSubmit = _scenarioContext
            .Get<InstructorTableRowModel[]>(ListAfterInstructorSubmitted);

        listAfterSubmit.Length.Should().Be(5);
        listAfterSubmit.Should().NotContain(removedCourse);
    }
}
