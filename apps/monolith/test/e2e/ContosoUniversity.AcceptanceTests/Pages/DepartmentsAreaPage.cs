namespace ContosoUniversity.AcceptanceTests.Pages;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

using Models;

public class DepartmentsAreaPage(IBrowser browser, IConfiguration configuration)
    : PageObject(browser, configuration)
{
    protected override string PagePath => $"{PageBaseUrl}/Departments";

    public async Task<DepartmentTableRowModel[]> ScrapRenderedDepartmentsList()
    {
        IReadOnlyList<IElementHandle> tableRows = await Page.QuerySelectorAllAsync("table > tbody > tr");

        Task<DepartmentTableRowModel>[] tasks = tableRows.Select(async x =>
        {
            IReadOnlyList<IElementHandle> tds = await x.QuerySelectorAllAsync("td");

            return new DepartmentTableRowModel(
                await tds[0].InnerTextAsync(),
                await tds[1].InnerTextAsync(),
                await tds[2].InnerTextAsync(),
                await tds[3].InnerTextAsync()
            );
        }).ToArray();

        return await Task.WhenAll(tasks);
    }

    public async Task EnterDepartmentDetails(DepartmentTableRowModel model)
    {
        await Page.FillAsync("#Name", model.Name);
        await Page.FillAsync("#Budget", model.Budget);
        await Page.FillAsync("#StartDate", model.StartDate);

        await Page.SelectOptionAsync("#AdministratorId", new[]
        {
            new SelectOptionValue { Label = model.Administrator }
        });
    }

    public async Task ClickLinkOnDepartmentsTable(string link, string departmentName)
    {
        await Page.ClickAsync($"table > tbody > tr:has(td:has-text('{departmentName}')) >> a:has-text('{link}')");
    }

    public async Task<DepartmentTableRowModel> ScrapRenderedDepartmentDetails()
    {
        IReadOnlyList<IElementHandle> tds = await Page.QuerySelectorAllAsync("div dl.row dd");

        return new DepartmentTableRowModel(
            await tds[0].InnerTextAsync(),
            await tds[1].InnerTextAsync(),
            await tds[2].InnerTextAsync(),
            await tds[3].InnerTextAsync()
        );
    }

    public async Task<DepartmentTableRowModel> ScrapRenderedDepartmentDetailsToEdit()
    {
        return new DepartmentTableRowModel(
            await Page.InputValueAsync("#Name"),
            await Page.InputValueAsync("#Budget"),
            await Page.InputValueAsync("#StartDate"),
            await Page.EvalOnSelectorAsync<string>(
                "#AdministratorId",
                @"e => {
                    var opts = e.options;
                    return opts[opts.selectedIndex].text;
                }")
        );
    }
}
