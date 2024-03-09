namespace ContosoUniversity.AcceptanceTests.Steps;

using System.Threading.Tasks;

using FluentAssertions;

using Models;

using Pages;

using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

[Binding]
public class DepartmentsSmoke(DepartmentsAreaPage page, ScenarioContext scenarioContext)
{
    private const string FeatureTag = "Departments";

    private const string InitialListOfDepartments = nameof(InitialListOfDepartments);
    private const string SubmittedDepartment = nameof(SubmittedDepartment);
    private const string ListAfterDepartmentSubmitted = nameof(ListAfterDepartmentSubmitted);

    [Given(@"user is on the Departments area landing page")]
    public async Task GivenUserIsOnTheDepartmentsAreaLandingPage()
    {
        await page.NavigateAsync();

        scenarioContext.Add(InitialListOfDepartments, await page.ScrapRenderedDepartmentsList());
    }

    [Then(@"user is able to view the following list of departments")]
    public void ThenUserIsAbleToViewTheFollowingListOfDepartments(Table table)
    {
        DepartmentTableRowModel[] renderedDepartment = scenarioContext
            .Get<DepartmentTableRowModel[]>(InitialListOfDepartments);

        renderedDepartment.Length.Should().Be(4);
        renderedDepartment.Should().BeEquivalentTo(table.CreateSet<DepartmentTableRowModel>());
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
        var model = table.CreateInstance<DepartmentTableRowModel>();
        await page.EnterDepartmentDetails(model);
        scenarioContext.Add(SubmittedDepartment, model);
    }

    [When(@"user submits the form")]
    [Scope(Tag = FeatureTag)]
    public async Task WhenUserSubmitsTheForm()
    {
        await page.ClickSubmitButton();
    }

    [Then(@"user is redirected to the Departments area landing page")]
    public void ThenUserIsRedirectedToTheDepartmentsAreaLandingPage()
    {
        page.IsAtRoute("").Should().BeTrue();
    }

    [Then(@"user is able to view the full list of departments")]
    public async Task ThenUserIsAbleToViewTheFullListOfDepartments()
    {
        scenarioContext.Add(ListAfterDepartmentSubmitted, await page.ScrapRenderedDepartmentsList());
    }

    [Then(@"including the department just submitted")]
    public void ThenIncludingTheDepartmentJustSubmitted()
    {
        DepartmentTableRowModel[] listAfterSubmit =
            scenarioContext.Get<DepartmentTableRowModel[]>(ListAfterDepartmentSubmitted);
        var submittedDepartment = scenarioContext.Get<DepartmentTableRowModel>(SubmittedDepartment);

        listAfterSubmit.Length.Should().Be(5);
        listAfterSubmit.Should().Contain(submittedDepartment);
    }

    [When(@"user clicks ""(.*)"" link for department ""(.*)""")]
    public async Task WhenUserClicksLinkForDepartment(string link, string departmentName)
    {
        await page.ClickLinkOnDepartmentsTable(link, departmentName);
    }

    [Then(@"the ""(.*)"" page with identifier opens successfully")]
    [Scope(Tag = FeatureTag)]
    public void ThenThePageWithIdentifierOpensSuccessfully(string pageRoute)
    {
        page.IsAtRouteWithGuidIdentifier(pageRoute).Should().BeTrue();
    }

    [Then(@"user is able to see the following details on department")]
    public async Task ThenUserIsAbleToSeeTheFollowingDetailsOnDepartment(Table table)
    {
        DepartmentTableRowModel details = await page.ScrapRenderedDepartmentDetails();

        details.Should().Be(table.CreateInstance<DepartmentTableRowModel>());
    }

    [Then(@"user is able to see the following department details to edit")]
    public async Task ThenUserIsAbleToSeeTheFollowingDepartmentDetailsToEdit(Table table)
    {
        DepartmentTableRowModel details = await page.ScrapRenderedDepartmentDetailsToEdit();

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
        DepartmentTableRowModel[] listAfterSubmit = scenarioContext
            .Get<DepartmentTableRowModel[]>(ListAfterDepartmentSubmitted);

        listAfterSubmit.Length.Should().Be(4);
        listAfterSubmit.Should().NotContain(removedCourse);
    }
}
