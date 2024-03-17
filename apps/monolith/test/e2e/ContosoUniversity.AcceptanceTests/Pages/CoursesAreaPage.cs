namespace ContosoUniversity.AcceptanceTests.Pages;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

using Models;

public class CoursesAreaPage(IBrowser browser, IConfiguration configuration)
    : PageObject(browser, configuration)
{
    private static class Inputs
    {
        public const string CourseCode = "#Request_CourseCode";
        public const string Title = "#Request_Title";
        public const string Credits = "#Request_Credits";
        public const string DepartmentId = "#Request_DepartmentId";
    }

    protected override string PagePath => $"{PageBaseUrl}/Courses";

    public async Task<CourseTableRowModel[]> ScrapRenderedCoursesList()
    {
        IReadOnlyList<IElementHandle> tableRows = await Page.QuerySelectorAllAsync("table > tbody > tr");

        Task<CourseTableRowModel>[] tasks = tableRows.Select(async x =>
        {
            IReadOnlyList<IElementHandle> tds = await x.QuerySelectorAllAsync("td");

            return new CourseTableRowModel(
                await tds[0].InnerTextAsync(),
                await tds[1].InnerTextAsync(),
                await tds[2].InnerTextAsync(),
                await tds[3].InnerTextAsync()
            );
        }).ToArray();

        return await Task.WhenAll(tasks);
    }

    public async Task EnterCourseDetails(CourseTableRowModel model)
    {
        if (model.CourseCode is not null)
        {
            await Page.FillAsync(Inputs.CourseCode, model.CourseCode);
        }

        await Page.FillAsync(Inputs.Title, model.Title);
        await Page.FillAsync(Inputs.Credits, model.Credits);

        await Page.SelectOptionAsync(Inputs.DepartmentId, new[]
        {
            new SelectOptionValue { Label = model.Department }
        });
    }

    public async Task ClickLinkOnCourseTable(string link, string courseCode)
    {
        await Page.ClickAsync($"table > tbody > tr:has(td:has-text('{courseCode}')) >> a:has-text('{link}')");
    }

    public async Task<CourseTableRowModel> ScrapRenderedCourseDetails()
    {
        IReadOnlyList<IElementHandle> tds = await Page.QuerySelectorAllAsync("div dl.row dd");

        return new CourseTableRowModel(
            await tds[0].InnerTextAsync(),
            await tds[1].InnerTextAsync(),
            await tds[2].InnerTextAsync(),
            await tds[3].InnerTextAsync()
        );
    }

    public async Task<CourseTableRowModel> ScrapRenderedCourseDetailsToEdit()
    {
        return new CourseTableRowModel(
            await (await Page.QuerySelectorAsync("form div.form-group div"))?.InnerTextAsync()!,
            await Page.InputValueAsync(Inputs.Title),
            await Page.InputValueAsync(Inputs.Credits),
            await Page.EvalOnSelectorAsync<string>(
                Inputs.DepartmentId,
                @"e => {
                    var opts = e.options;
                    return opts[opts.selectedIndex].text;
                }")
        );
    }
}
