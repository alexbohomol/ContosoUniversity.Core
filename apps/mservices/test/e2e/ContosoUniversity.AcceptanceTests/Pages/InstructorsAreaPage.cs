namespace ContosoUniversity.AcceptanceTests.Pages;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

using Models;

public class InstructorsAreaPage(IBrowser browser, IConfiguration configuration)
    : PageObject(browser, configuration)
{
    private static class Inputs
    {
        public const string LastName = "#Request_LastName";
        public const string FirstName = "#Request_FirstName";
        public const string HireDate = "#Request_HireDate";
    }

    protected override string PagePath => $"{PageBaseUrl}/Instructors";

    public async Task<InstructorTableRowModel[]> ScrapRenderedInstructorsList()
    {
        IReadOnlyList<IElementHandle> tableRows = await Page.QuerySelectorAllAsync("table > tbody > tr");

        Task<InstructorTableRowModel>[] tasks = tableRows.Select(async x =>
        {
            IReadOnlyList<IElementHandle> tds = await x.QuerySelectorAllAsync("td");

            return new InstructorTableRowModel(
                await tds[0].InnerTextAsync(),
                await tds[1].InnerTextAsync(),
                await tds[2].InnerTextAsync(),
                await tds[3].InnerTextAsync()
            );
        }).ToArray();

        return await Task.WhenAll(tasks);
    }

    public async Task EnterInstructorDetails(InstructorTableRowModel model)
    {
        await Page.FillAsync(Inputs.LastName, model.LastName);
        await Page.FillAsync(Inputs.FirstName, model.FirstName);
        await Page.FillAsync(Inputs.HireDate, model.HireDate);
    }

    public async Task ClickLinkOnInstructorsTable(string link, string instructorName)
    {
        await Page.ClickAsync($"table > tbody > tr:has(td:has-text('{instructorName}')) >> a:has-text('{link}')");
    }

    public async Task<InstructorTableRowModel> ScrapRenderedInstructorDetails()
    {
        IReadOnlyList<IElementHandle> tds = await Page.QuerySelectorAllAsync("div dl.row dd");

        return new InstructorTableRowModel(
            await tds[0].InnerTextAsync(),
            await tds[1].InnerTextAsync(),
            await tds[2].InnerTextAsync(),
            null);
    }

    public async Task<InstructorTableRowModel> ScrapRenderedInstructorDetailsToEdit()
    {
        return new InstructorTableRowModel(
            await Page.InputValueAsync(Inputs.LastName),
            await Page.InputValueAsync(Inputs.FirstName),
            await Page.InputValueAsync(Inputs.HireDate),
            null);
    }
}
