namespace IntegrationTesting.SharedKernel;

using FluentAssertions;

using HealthChecks.UI.Core;

public static class HealthReportAssertions
{
    public static void ShouldBeHealthy(
        this UIHealthReport report,
        string[] expectedCheckNames,
        string[] expectedTags)
    {
        report.Should().NotBeNull();
        report.Status.Should().Be(UIHealthStatus.Healthy);
        // report.TotalDuration.Should().BeLessThan(TimeSpan.FromSeconds(1));
        report.Entries.Should().NotBeEmpty();
        report.Entries.Count.Should().Be(2);
        report.Entries.Keys.Should().BeEquivalentTo(expectedCheckNames);
        report.Entries.Values.SelectMany(x => x.Tags).Distinct().Should().BeEquivalentTo(expectedTags);
        report.Entries.Values.Should().AllSatisfy(ShouldBeHealthy);
    }

    private static void ShouldBeHealthy(this UIHealthReportEntry entry)
    {
        entry.Data.Should().BeEmpty();
        entry.Status.Should().Be(UIHealthStatus.Healthy);
        // entry.Duration.Should().BeLessThan(TimeSpan.FromSeconds(1));
        entry.Tags?.Should().NotBeNull();
        entry.Tags?.Count().Should().Be(4);
    }

    public static void ShouldBeUnhealthy(
        this UIHealthReport report,
        string[] expectedCheckNames,
        string[] expectedTags)
    {
        report.Should().NotBeNull();
        report.Status.Should().Be(UIHealthStatus.Unhealthy);
        // report.TotalDuration.Should().BeLessThan(TimeSpan.FromSeconds(1));
        report.Entries.Should().NotBeEmpty();
        report.Entries.Count.Should().Be(2);
        report.Entries.Keys.Should().BeEquivalentTo(expectedCheckNames);
        report.Entries.Values.SelectMany(x => x.Tags).Distinct().Should().BeEquivalentTo(expectedTags);
        report.Entries.Values.Should().AllSatisfy(ShouldBeUnhealthy);
    }

    private static void ShouldBeUnhealthy(this UIHealthReportEntry entry)
    {
        entry.Data.Should().BeEmpty();
        entry.Status.Should().Be(UIHealthStatus.Unhealthy);
        // entry.Duration.Should().BeLessThan(TimeSpan.FromSeconds(1));
        entry.Tags?.Should().NotBeNull();
        entry.Tags?.Count().Should().Be(4);
    }
}
