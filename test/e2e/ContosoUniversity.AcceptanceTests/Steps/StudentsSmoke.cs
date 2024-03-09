namespace ContosoUniversity.AcceptanceTests.Steps;

using System.Threading.Tasks;

using FluentAssertions;

using Models;

using Pages;

using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

[Binding]
public class StudentsSmoke(StudentsAreaPage page, ScenarioContext scenarioContext)
{
    private const string FeatureTag = "Students";

    private const string InitialListOfStudents = nameof(InitialListOfStudents);
    private const string SubmittedStudent = nameof(SubmittedStudent);
    private const string ListAfterStudentSubmitted = nameof(ListAfterStudentSubmitted);

    private readonly StudentsAreaPage _page = page;
    private readonly ScenarioContext _scenarioContext = scenarioContext;

    [Given(@"user is on the Students area landing page")]
    public async Task GivenUserIsOnTheStudentsAreaLandingPage()
    {
        await _page.NavigateAsync();

        _scenarioContext.Add(InitialListOfStudents, await _page.ScrapRenderedStudentsList());
    }

    [Then(@"user is able to view the following list of students")]
    public void ThenUserIsAbleToViewTheFollowingListOfStudents(Table table)
    {
        StudentTableRowModel[] renderedStudent = _scenarioContext
            .Get<StudentTableRowModel[]>(InitialListOfStudents);

        renderedStudent.Length.Should().Be(3);
        renderedStudent.Should().BeEquivalentTo(table.CreateSet<StudentTableRowModel>());
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
        var model = table.CreateInstance<StudentTableRowModel>();
        await _page.EnterStudentDetails(model);
        _scenarioContext.Add(SubmittedStudent, model);
    }

    [When(@"user submits the form")]
    [Scope(Tag = FeatureTag)]
    public async Task WhenUserSubmitsTheForm()
    {
        await _page.ClickSubmitButton();
    }

    [Then(@"user is redirected to the Students area landing page")]
    public void ThenUserIsRedirectedToTheStudentsAreaLandingPage()
    {
        _page.IsAtRoute("").Should().BeTrue();
    }

    [Then(@"user is able to view the full list of students")]
    public async Task ThenUserIsAbleToViewTheFullListOfStudents()
    {
        _scenarioContext.Add(ListAfterStudentSubmitted, await _page.ScrapRenderedStudentsList());
    }

    [Then(@"including the student just submitted")]
    public void ThenIncludingTheStudentJustSubmitted()
    {
        StudentTableRowModel[] listAfterSubmit =
            _scenarioContext.Get<StudentTableRowModel[]>(ListAfterStudentSubmitted);
        var submittedStudent = _scenarioContext.Get<StudentTableRowModel>(SubmittedStudent);

        listAfterSubmit.Length.Should().Be(3);
        listAfterSubmit.Should().Contain(submittedStudent);
    }

    [When(@"user clicks ""(.*)"" link for student ""(.*)""")]
    public async Task WhenUserClicksLinkForStudent(string link, string studentName)
    {
        await _page.ClickLinkOnStudentsTable(link, studentName);
    }

    [Then(@"the ""(.*)"" page with identifier opens successfully")]
    [Scope(Tag = FeatureTag)]
    public void ThenThePageWithIdentifierOpensSuccessfully(string pageRoute)
    {
        _page.IsAtRouteWithGuidIdentifier(pageRoute).Should().BeTrue();
    }

    [Then(@"user is able to see the following details on student")]
    public async Task ThenUserIsAbleToSeeTheFollowingDetailsOnStudent(Table table)
    {
        StudentTableRowModel details = await _page.ScrapRenderedStudentDetails();

        details.Should().Be(table.CreateInstance<StudentTableRowModel>());
    }

    [Then(@"user is able to see the following student details to edit")]
    public async Task ThenUserIsAbleToSeeTheFollowingStudentDetailsToEdit(Table table)
    {
        StudentTableRowModel details = await _page.ScrapRenderedStudentDetailsToEdit();

        details.Should().Be(table.CreateInstance<StudentTableRowModel>());
    }

    [Given(@"user is on the ""(.*)"" page for student ""(.*)""")]
    public async Task GivenUserIsOnThePageForStudent(string link, string studentName)
    {
        await GivenUserIsOnTheStudentsAreaLandingPage();
        await WhenUserClicksLinkForStudent(link, studentName);
    }

    [Then(@"excluding the student just deleted")]
    public void ThenExcludingTheStudentJustDeleted(Table table)
    {
        var removedCourse = table.CreateInstance<StudentTableRowModel>();
        StudentTableRowModel[] listAfterSubmit = _scenarioContext
            .Get<StudentTableRowModel[]>(ListAfterStudentSubmitted);

        listAfterSubmit.Length.Should().Be(3);
        listAfterSubmit.Should().NotContain(removedCourse);
    }
}
