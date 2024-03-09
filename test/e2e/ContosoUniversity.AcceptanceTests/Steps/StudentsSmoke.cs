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

    [Given(@"user is on the Students area landing page")]
    public async Task GivenUserIsOnTheStudentsAreaLandingPage()
    {
        await page.NavigateAsync();

        scenarioContext.Add(InitialListOfStudents, await page.ScrapRenderedStudentsList());
    }

    [Then(@"user is able to view the following list of students")]
    public void ThenUserIsAbleToViewTheFollowingListOfStudents(Table table)
    {
        StudentTableRowModel[] renderedStudent = scenarioContext
            .Get<StudentTableRowModel[]>(InitialListOfStudents);

        renderedStudent.Length.Should().Be(3);
        renderedStudent.Should().BeEquivalentTo(table.CreateSet<StudentTableRowModel>());
    }

    [Then(@"the page title is ""(.*)""")]
    [Scope(Tag = FeatureTag)]
    public async Task ThenThePageTitleIs(string pageTitle)
    {
        (await page.HasTitle(pageTitle)).Should().BeTrue();
    }

    [When(@"user clicks ""(.*)"" link")]
    [Scope(Tag = FeatureTag)]
    public async Task WhenUserClicksLink(string linkText)
    {
        await page.ClickLinkWithText(linkText);
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
        var model = table.CreateInstance<StudentTableRowModel>();
        await page.EnterStudentDetails(model);
        scenarioContext.Add(SubmittedStudent, model);
    }

    [When(@"user submits the form")]
    [Scope(Tag = FeatureTag)]
    public async Task WhenUserSubmitsTheForm()
    {
        await page.ClickSubmitButton();
    }

    [Then(@"user is redirected to the Students area landing page")]
    public void ThenUserIsRedirectedToTheStudentsAreaLandingPage()
    {
        page.IsAtRoute("").Should().BeTrue();
    }

    [Then(@"user is able to view the full list of students")]
    public async Task ThenUserIsAbleToViewTheFullListOfStudents()
    {
        scenarioContext.Add(ListAfterStudentSubmitted, await page.ScrapRenderedStudentsList());
    }

    [Then(@"including the student just submitted")]
    public void ThenIncludingTheStudentJustSubmitted()
    {
        StudentTableRowModel[] listAfterSubmit =
            scenarioContext.Get<StudentTableRowModel[]>(ListAfterStudentSubmitted);
        var submittedStudent = scenarioContext.Get<StudentTableRowModel>(SubmittedStudent);

        listAfterSubmit.Length.Should().Be(3);
        listAfterSubmit.Should().Contain(submittedStudent);
    }

    [When(@"user clicks ""(.*)"" link for student ""(.*)""")]
    public async Task WhenUserClicksLinkForStudent(string link, string studentName)
    {
        await page.ClickLinkOnStudentsTable(link, studentName);
    }

    [Then(@"the ""(.*)"" page with identifier opens successfully")]
    [Scope(Tag = FeatureTag)]
    public void ThenThePageWithIdentifierOpensSuccessfully(string pageRoute)
    {
        page.IsAtRouteWithGuidIdentifier(pageRoute).Should().BeTrue();
    }

    [Then(@"user is able to see the following details on student")]
    public async Task ThenUserIsAbleToSeeTheFollowingDetailsOnStudent(Table table)
    {
        StudentTableRowModel details = await page.ScrapRenderedStudentDetails();

        details.Should().Be(table.CreateInstance<StudentTableRowModel>());
    }

    [Then(@"user is able to see the following student details to edit")]
    public async Task ThenUserIsAbleToSeeTheFollowingStudentDetailsToEdit(Table table)
    {
        StudentTableRowModel details = await page.ScrapRenderedStudentDetailsToEdit();

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
        StudentTableRowModel[] listAfterSubmit = scenarioContext
            .Get<StudentTableRowModel[]>(ListAfterStudentSubmitted);

        listAfterSubmit.Length.Should().Be(3);
        listAfterSubmit.Should().NotContain(removedCourse);
    }
}
