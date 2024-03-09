namespace ContosoUniversity.AcceptanceTests.Steps;

using System.Threading.Tasks;

using FluentAssertions;

using Models;

using Pages;

using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

[Binding]
public class CoursesSmoke(CoursesAreaPage page, ScenarioContext scenarioContext)
{
    private const string FeatureTag = "Courses";

    private const string InitialListOfCourses = nameof(InitialListOfCourses);
    private const string SubmittedCourse = nameof(SubmittedCourse);
    private const string ListAfterCourseSubmitted = nameof(ListAfterCourseSubmitted);

    [Given(@"user is on the Courses area landing page")]
    public async Task GivenUserIsOnTheCoursesAreaLandingPage()
    {
        await page.NavigateAsync();

        scenarioContext.Add(InitialListOfCourses, await page.ScrapRenderedCoursesList());
    }

    [Then(@"user is able to view the following list of courses")]
    public void ThenUserIsAbleToViewTheFollowingListOfCourses(Table table)
    {
        CourseTableRowModel[] renderedCourses = scenarioContext.Get<CourseTableRowModel[]>(InitialListOfCourses);

        renderedCourses.Length.Should().Be(7);
        renderedCourses.Should().BeEquivalentTo(table.CreateSet<CourseTableRowModel>());
    }

    [When(@"user clicks ""(.*)"" link")]
    [Scope(Tag = FeatureTag)]
    public async Task WhenUserClicksLink(string linkText)
    {
        await page.ClickLinkWithText(linkText);
    }

    [Then(@"the page title is ""(.*)""")]
    [Scope(Tag = FeatureTag)]
    public async Task ThenThePageTitleIs(string pageTitle)
    {
        (await page.HasTitle(pageTitle)).Should().BeTrue();
    }

    [Then(@"the ""(.*)"" page opens successfully")]
    [Scope(Tag = FeatureTag)]
    public void ThenThePageOpensSuccessfully(string pageRoute)
    {
        page.IsAtRoute(pageRoute).Should().BeTrue();
    }

    [Given(@"user is on the ""(.*)"" page")]
    [Scope(Tag = FeatureTag)]
    public async Task GivenUserIsOnThePage(string route)
    {
        await page.NavigateToRouteAsync(route);
    }

    [When(@"user enters following details on form")]
    [Scope(Tag = FeatureTag)]
    public async Task WhenUserEntersFollowingDetailsOnForm(Table table)
    {
        var model = table.CreateInstance<CourseTableRowModel>();
        await page.EnterCourseDetails(model);
        scenarioContext.Add(SubmittedCourse, model);
    }

    [When(@"user submits the form")]
    [Scope(Tag = FeatureTag)]
    public async Task WhenUserSubmitsTheForm()
    {
        await page.ClickSubmitButton();
    }

    [Then(@"user is able to view the full list of courses")]
    public async Task ThenUserIsAbleToViewTheFullListOfCourses()
    {
        scenarioContext.Add(ListAfterCourseSubmitted, await page.ScrapRenderedCoursesList());
    }

    [Then(@"including the course just submitted")]
    public void ThenIncludingTheCourseJustSubmitted()
    {
        CourseTableRowModel[] listAfterSubmit = scenarioContext.Get<CourseTableRowModel[]>(ListAfterCourseSubmitted);
        var submittedCourse = scenarioContext.Get<CourseTableRowModel>(SubmittedCourse);

        if (submittedCourse.CourseCode is null)
        {
            submittedCourse = submittedCourse with { CourseCode = "1010" };
        }

        listAfterSubmit.Length.Should().Be(8);
        listAfterSubmit.Should().Contain(submittedCourse);
    }

    [Then(@"user is redirected to the Courses area landing page")]
    public void ThenUserIsRedirectedToTheCoursesAreaLandingPage()
    {
        page.IsAtRoute("").Should().BeTrue();
    }

    [When(@"user clicks ""(.*)"" link for course ""(.*)""")]
    public async Task WhenUserClicksLinkForCourse(string link, string courseCode)
    {
        await page.ClickLinkOnCourseTable(link, courseCode);
    }

    [Then(@"the ""(.*)"" page with identifier opens successfully")]
    [Scope(Tag = FeatureTag)]
    public void ThenThePageWithIdentifierOpensSuccessfully(string pageRoute)
    {
        page.IsAtRouteWithGuidIdentifier(pageRoute).Should().BeTrue();
    }

    [Then(@"user is able to see the following details on course")]
    public async Task ThenUserIsAbleToSeeTheFollowingDetailsOnCourse(Table table)
    {
        CourseTableRowModel details = await page.ScrapRenderedCourseDetails();

        details.Should().Be(table.CreateInstance<CourseTableRowModel>());
    }

    [Then(@"user is able to see the following course details to edit")]
    public async Task ThenUserIsAbleToSeeTheFollowingCourseDetailsToEdit(Table table)
    {
        CourseTableRowModel details = await page.ScrapRenderedCourseDetailsToEdit();

        details.Should().Be(table.CreateInstance<CourseTableRowModel>());
    }

    [Given(@"user is on the ""(.*)"" page for course ""(.*)""")]
    public async Task GivenUserIsOnThePageForCourse(string link, string courseCode)
    {
        await GivenUserIsOnTheCoursesAreaLandingPage();
        await WhenUserClicksLinkForCourse(link, courseCode);
    }

    [Then(@"excluding the course just deleted")]
    public void ThenExcludingTheCourseJustDeleted(Table table)
    {
        var removedCourse = table.CreateInstance<CourseTableRowModel>();
        CourseTableRowModel[] listAfterSubmit = scenarioContext.Get<CourseTableRowModel[]>(ListAfterCourseSubmitted);

        listAfterSubmit.Length.Should().Be(7);
        listAfterSubmit.Should().NotContain(removedCourse);
    }
}
