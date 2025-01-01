namespace Courses.Api.IntegrationTests;

using System.IO;
using System.Threading.Tasks;

using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Model.Common;
using Ductus.FluentDocker.Model.Containers;
using Ductus.FluentDocker.Services;

using Xunit;

public class SharedTestContext : IAsyncLifetime
{
    private static readonly string[] DockerComposeFiles =
    [
        GetFullPath("../../../../../../docker-compose.yml"),
        GetFullPath("../../../../../../docker-compose.override.yml")
    ];

    private static string GetFullPath(string relativePath) =>
        Path.GetFullPath(
            Path.Combine(
                Directory.GetCurrentDirectory(),
                (TemplateString)relativePath));

    private readonly ICompositeService _dockerService = new Builder()
        .UseContainer()
        .UseCompose()
        .FromFile(DockerComposeFiles)
        .RemoveOrphans()
        .Wait("cuweb", (service, _) =>
        {
            var cuweb = service.GetConfiguration(true);
            var healthStatus = cuweb.State.Health.Status;

            return healthStatus == HealthState.Healthy
                ? -1    // stop awaiting, ready to go
                : 1000; // wait another 1000ms
        })
        .Build();

    public Task InitializeAsync()
    {
        _dockerService.Start();
        _dockerService.Containers.FirstOrDefault(x => x.Name == "cuweb")?.Dispose();
        _dockerService.Containers.FirstOrDefault(x => x.Name == "courses-api")?.Dispose();
        _dockerService.Containers.FirstOrDefault(x => x.Name == "departments-api")?.Dispose();
        _dockerService.Containers.FirstOrDefault(x => x.Name == "students-api")?.Dispose();

        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        _dockerService.Dispose();

        return Task.CompletedTask;
    }
}
