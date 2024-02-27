namespace ContosoUniversity.AcceptanceTests.Pages;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Playwright;

using Models;

public class CoursesAreaPage : PageObject
{
    public CoursesAreaPage(IBrowser browser) : base(browser)
    {
    }

    protected override string PagePath => "http://localhost:10000/Courses";

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
            await Page.FillAsync("#CourseCode", model.CourseCode);
        }

        await Page.FillAsync("#Title", model.Title);
        await Page.FillAsync("#Credits", model.Credits);

        await Page.SelectOptionAsync("#DepartmentId", new[]
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
            await Page.InputValueAsync("#Title"),
            await Page.InputValueAsync("#Credits"),
            await Page.EvalOnSelectorAsync<string>(
                "#DepartmentId",
                @"e => {
                    var opts = e.options; 
                    return opts[opts.selectedIndex].text;
                }")
        );
    }
}
