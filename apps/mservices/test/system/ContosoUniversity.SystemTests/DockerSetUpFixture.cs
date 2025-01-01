namespace ContosoUniversity.SystemTests;

using System.IO;
using System.Text;

using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Model.Common;
using Ductus.FluentDocker.Model.Containers;
using Ductus.FluentDocker.Services;

using NUnit.Framework;

[SetUpFixture]
public class DockerSetUpFixture
{
    private static readonly string[] DockerComposeFiles =
    [
        GetFullPath("../../../../../../docker-compose.yml"),
        GetFullPath("../../../../../../docker-compose.override.yml")
    ];

    private static ICompositeService _dockerService;

    private static string GetFullPath(string relativePath) =>
        Path.GetFullPath(
            Path.Combine(
                Directory.GetCurrentDirectory(),
                (TemplateString)relativePath));

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _dockerService = new Builder()
            .UseContainer()
            .UseCompose()
            .FromFile(DockerComposeFiles)
            .RemoveOrphans()
            .Wait("cuweb", (service, _) =>
            {
                var cuweb = service.GetConfiguration(true);
                var healthStatus = cuweb.State.Health.Status;

                var builder = new StringBuilder();
                builder.Append($"{TestContext.CurrentContext.Test.Name}:");
                builder.Append(" Waiting for SUT healthy state.");
                builder.Append($" Current: {healthStatus}.");

                TestContext.Progress.WriteLine(builder.ToString());

                return healthStatus == HealthState.Healthy
                    ? -1    // stop awaiting, ready to go
                    : 1000; // wait another 1000ms
            })
            .Build();

        _dockerService.Start();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _dockerService.Dispose();
    }
}
