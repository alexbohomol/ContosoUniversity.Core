namespace ContosoUniversity.AcceptanceTests.SystemTests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

public class SystemTest : PageTest
{
    protected static readonly IConfiguration Configuration = new ConfigurationBuilder()
        .AddJsonFile("testsettings.json", optional: false)
        .Build();

    protected async Task<Dictionary<Guid, string>> ScrapSelectListOptions(string optionsSelector)
    {
        IReadOnlyList<IElementHandle> options = await Page.QuerySelectorAllAsync(optionsSelector);

        var values = await Task.WhenAll(options.Select(async option => new
        {
            Value = await option.GetAttributeAsync("value"),
            Text = await option.InnerTextAsync()
        }));

        return values
            .Where(x => x.Value != string.Empty)
            .ToDictionary(
                x => Guid.Parse(x.Value),
                x => x.Text);
    }
}
