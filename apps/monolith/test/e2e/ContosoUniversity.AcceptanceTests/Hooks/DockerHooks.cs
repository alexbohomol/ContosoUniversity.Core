namespace ContosoUniversity.AcceptanceTests.Hooks;

using System.IO;

using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Model.Common;
using Ductus.FluentDocker.Services;

using Microsoft.Extensions.Configuration;

using TechTalk.SpecFlow;

[Binding]
public class DockerHooks
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

    [BeforeFeature]
    public static void StartDockerInfrastructure(IConfiguration configuration)
    {
        _dockerService = new Builder()
            .UseContainer()
            .UseCompose()
            .FromFile(DockerComposeFiles)
            .RemoveOrphans()
            .WaitForHttp("web", configuration["PageBaseUrl:Http"])
            .Build();

        _dockerService.Start();
    }

    [AfterFeature]
    public static void DisposeDockerInfrastructure()
    {
        _dockerService.Dispose();
    }
}
