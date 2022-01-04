namespace ContosoUniversity.AcceptanceTests.Steps;

using System.Threading.Tasks;

using FluentAssertions;

using Models;

using Pages;

using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

[Binding]
public class DepartmentsSmoke
{
    private const string FeatureTag = "Departments";

    private const string InitialListOfDepartments = nameof(InitialListOfDepartments);
    private const string SubmittedDepartment = nameof(SubmittedDepartment);
    private const string ListAfterDepartmentSubmitted = nameof(ListAfterDepartmentSubmitted);

    private readonly DepartmentsAreaPage _page;
    private readonly ScenarioContext _scenarioContext;

    public DepartmentsSmoke(DepartmentsAreaPage page, ScenarioContext scenarioContext)
    {
        _page = page;
        _scenarioContext = scenarioContext;
    }

    [Given(@"user is on the Departments area landing page")]
    public async Task GivenUserIsOnTheDepartmentsAreaLandingPage()
    {
        await _page.NavigateAsync();

        _scenarioContext.Add(InitialListOfDepartments, await _page.ScrapRenderedDepartmentsList());
    }

    [Then(@"user is able to view the following list of departments")]
    public void ThenUserIsAbleToViewTheFollowingListOfDepartments(Table table)
    {
        DepartmentTableRowModel[] renderedDepartment = _scenarioContext
            .Get<DepartmentTableRowModel[]>(InitialListOfDepartments);

        renderedDepartment.Length.Should().Be(4);
        renderedDepartment.Should().BeEquivalentTo(table.CreateSet<DepartmentTableRowModel>());
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
        var model = table.CreateInstance<DepartmentTableRowModel>();
        await _page.EnterDepartmentDetails(model);
        _scenarioContext.Add(SubmittedDepartment, model);
    }

    [When(@"user submits the form")]
    [Scope(Tag = FeatureTag)]
    public async Task WhenUserSubmitsTheForm()
    {
        await _page.ClickSubmitButton();
    }

    [Then(@"user is redirected to the Departments area landing page")]
    public void ThenUserIsRedirectedToTheDepartmentsAreaLandingPage()
    {
        _page.IsAtRoute("").Should().BeTrue();
    }

    [Then(@"user is able to view the full list of departments")]
    public async Task ThenUserIsAbleToViewTheFullListOfDepartments()
    {
        _scenarioContext.Add(ListAfterDepartmentSubmitted, await _page.ScrapRenderedDepartmentsList());
    }

    [Then(@"including the department just submitted")]
    public void ThenIncludingTheDepartmentJustSubmitted()
    {
        DepartmentTableRowModel[] listAfterSubmit =
            _scenarioContext.Get<DepartmentTableRowModel[]>(ListAfterDepartmentSubmitted);
        var submittedDepartment = _scenarioContext.Get<DepartmentTableRowModel>(SubmittedDepartment);

        listAfterSubmit.Length.Should().Be(5);
        listAfterSubmit.Should().Contain(submittedDepartment);
    }

    [When(@"user clicks ""(.*)"" link for department ""(.*)""")]
    public async Task WhenUserClicksLinkForDepartment(string link, string departmentName)
    {
        await _page.ClickLinkOnDepartmentsTable(link, departmentName);
    }

    [Then(@"the ""(.*)"" page with identifier opens successfully")]
    [Scope(Tag = FeatureTag)]
    public void ThenThePageWithIdentifierOpensSuccessfully(string pageRoute)
    {
        _page.IsAtRouteWithGuidIdentifier(pageRoute).Should().BeTrue();
    }

    [Then(@"user is able to see the following details on department")]
    public async Task ThenUserIsAbleToSeeTheFollowingDetailsOnDepartment(Table table)
    {
        DepartmentTableRowModel details = await _page.ScrapRenderedDepartmentDetails();

        details.Should().Be(table.CreateInstance<DepartmentTableRowModel>());
    }

    [Then(@"user is able to see the following department details to edit")]
    public async Task ThenUserIsAbleToSeeTheFollowingDepartmentDetailsToEdit(Table table)
    {
        DepartmentTableRowModel details = await _page.ScrapRenderedDepartmentDetailsToEdit();

        details.Should().Be(table.CreateInstance<DepartmentTableRowModel>());
    }

    [Given(@"user is on the ""(.*)"" page for department ""(.*)""")]
    public async Task GivenUserIsOnThePageForDepartment(string link, string departmentName)
    {
        await GivenUserIsOnTheDepartmentsAreaLandingPage();
        await WhenUserClicksLinkForDepartment(link, departmentName);
    }

    [Then(@"excluding the department just deleted")]
    public void ThenExcludingTheDepartmentJustDeleted(Table table)
    {
        var removedCourse = table.CreateInstance<DepartmentTableRowModel>();
        DepartmentTableRowModel[] listAfterSubmit = _scenarioContext
            .Get<DepartmentTableRowModel[]>(ListAfterDepartmentSubmitted);

        listAfterSubmit.Length.Should().Be(4);
        listAfterSubmit.Should().NotContain(removedCourse);
    }
}