namespace ContosoUniversity.Mvc.IntegrationTests;

using System.Linq;

using FluentAssertions;

using HealthChecks.UI.Core;

public static class HealthChecksAssertions
{
    private static readonly string[] CheckNames =
    [
        "sql-courses-reads",
        "sql-courses-writes",
        "sql-students-reads",
        "sql-students-writes",
        "sql-departments-reads",
        "sql-departments-writes"
    ];

    private static readonly string[] Tags =
    [
        "db",
        "sql",
        "courses",
        "students",
        "departments",
        "reads",
        "writes"
    ];

    public static void ShouldBeSuccessful(this UIHealthReport report)
    {
        report.Should().NotBeNull();
        report.Status.Should().Be(UIHealthStatus.Healthy);
        // report.TotalDuration.Should().BeLessThan(TimeSpan.FromSeconds(1));
        report.Entries.Should().NotBeEmpty();
        report.Entries.Count.Should().Be(6);
        report.Entries.Keys.Should().BeEquivalentTo(CheckNames);
        report.Entries.Values.SelectMany(x => x.Tags).Distinct().Should().BeEquivalentTo(Tags);
        report.Entries.Values.Should().AllSatisfy(x => x.ShouldBeSuccessful());
    }

    private static void ShouldBeSuccessful(this UIHealthReportEntry entry)
    {
        entry.Data.Should().BeEmpty();
        entry.Status.Should().Be(UIHealthStatus.Healthy);
        // entry.Duration.Should().BeLessThan(TimeSpan.FromSeconds(1));
        entry.Tags?.Should().NotBeNull();
        entry.Tags?.Count().Should().Be(4);
    }
}
