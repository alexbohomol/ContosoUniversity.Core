namespace ContosoUniversity.AcceptanceTests.Pages;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

using Models;

public class StudentsAreaPage(IBrowser browser, IConfiguration configuration)
    : PageObject(browser, configuration)
{
    private static class Inputs
    {
        public const string LastName = "#Request_LastName";
        public const string FirstName = "#Request_FirstName";
        public const string EnrollmentDate = "#Request_EnrollmentDate";
    }

    protected override string PagePath => $"{PageBaseUrl}/Students";

    public async Task<StudentTableRowModel[]> ScrapRenderedStudentsList()
    {
        IReadOnlyList<IElementHandle> tableRows = await Page.QuerySelectorAllAsync("table > tbody > tr");

        Task<StudentTableRowModel>[] tasks = tableRows.Select(async x =>
        {
            IReadOnlyList<IElementHandle> tds = await x.QuerySelectorAllAsync("td");

            return new StudentTableRowModel(
                await tds[0].InnerTextAsync(),
                await tds[1].InnerTextAsync(),
                await tds[2].InnerTextAsync());
        }).ToArray();

        return await Task.WhenAll(tasks);
    }

    public async Task EnterStudentDetails(StudentTableRowModel model)
    {
        await Page.FillAsync(Inputs.LastName, model.LastName);
        await Page.FillAsync(Inputs.FirstName, model.FirstName);
        await Page.FillAsync(Inputs.EnrollmentDate, model.EnrollmentDate);
    }

    public async Task ClickLinkOnStudentsTable(string link, string studentName)
    {
        await Page.ClickAsync($"table > tbody > tr:has(td:has-text('{studentName}')) >> a:has-text('{link}')");
    }

    public async Task<StudentTableRowModel> ScrapRenderedStudentDetails()
    {
        IReadOnlyList<IElementHandle> tds = await Page.QuerySelectorAllAsync("div dl.row dd");

        return new StudentTableRowModel(
            await tds[0].InnerTextAsync(),
            await tds[1].InnerTextAsync(),
            await tds[2].InnerTextAsync());
    }

    public async Task<StudentTableRowModel> ScrapRenderedStudentDetailsToEdit()
    {
        return new StudentTableRowModel(
            await Page.InputValueAsync(Inputs.LastName),
            await Page.InputValueAsync(Inputs.FirstName),
            await Page.InputValueAsync(Inputs.EnrollmentDate));
    }
}
