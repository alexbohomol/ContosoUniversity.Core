namespace ContosoUniversity.AcceptanceTests.Pages;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Playwright;

using Models;

public class InstructorsAreaPage : PageObject
{
    public InstructorsAreaPage(IBrowser browser) : base(browser)
    {
    }

    protected override string PagePath => "https://localhost:10001/Instructors";

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
        await Page.FillAsync("#LastName", model.LastName);
        await Page.FillAsync("#FirstName", model.FirstName);
        await Page.FillAsync("#HireDate", model.HireDate);
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
            await Page.InputValueAsync("#LastName"),
            await Page.InputValueAsync("#FirstName"),
            await Page.InputValueAsync("#HireDate"),
            null);
    }
}